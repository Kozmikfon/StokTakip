using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Dtos.MalzemeDtos;
using StokTakip.Entities.Enums;
using StokTakip.Entities.ViewModels;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StokTakip.MVC.Controllers
{
    public class IrsaliyeController : Controller
    {
        private readonly IIrsaliyeService _irsaliyeService;
        private readonly IIrsaliyeDetayService _detayService;
        private readonly ICariService _cariService;
        private readonly IMalzemeService _malzemeService;
        private readonly IDepoService _depoService;

        public IrsaliyeController(
            IIrsaliyeService irsaliyeService,
            IIrsaliyeDetayService detayService,
            IMalzemeService malzemeService,
            IDepoService depoService,
            ICariService cariService)
        {
            _irsaliyeService = irsaliyeService;
            _detayService = detayService;
            _malzemeService = malzemeService;
            _depoService = depoService;
            _cariService = cariService;
        }

        // Dropdown'ları güvenle doldur (Value/Text projeksiyonu)
        private async Task PopulateDropdownsAsync(IrsaliyePageVm vm)
        {
            var cariler = (await _cariService.GetAllAsync()).Data ?? new List<CariListDto>();
            vm.Cariler = new SelectList(
                cariler.Select(c => new { Value = c.Id, Text = c.Unvan }),
                "Value", "Text",
                vm.PostModel?.CarId > 0 ? vm.PostModel.CarId : (int?)null
            );

            var depolar = (await _depoService.GetAllAsync()).Data ?? new List<DepoListDto>();
            vm.Depolar = new SelectList(
                depolar.Select(d => new { Value = d.Id, Text = d.DepoAd }),
                "Value", "Text",
                vm.PostModel?.DepoId > 0 ? vm.PostModel.DepoId : (int?)null
            );

            var malzemeler = (await _malzemeService.GetAllAsync()).Data ?? new List<MalzemeListDto>();
            vm.Malzemeler = new SelectList(
                malzemeler.Select(m => new { Value = m.Id, Text = m.malzemeAdi }),
                "Value", "Text"
            );
        }

        // Liste
        public async Task<IActionResult> Index()
        {
            var list = await _irsaliyeService.GetAllAsync();
            return View(list.Data ?? new List<IrsaliyeListDto>());
        }

        // Edit (tek sayfa) – id yoksa DB'ye yazmadan boş form göster
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            IrsaliyeDto dto;

            if (id.GetValueOrDefault() > 0)
            {
                var res = await _irsaliyeService.Get(id.Value);
                if (res.ResultStatus != ResultStatus.Success || res.Data == null)
                {
                    TempData["Error"] = res.Info ?? "Kayıt bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }
                dto = res.Data;
            }
            else
            {
                // Boş form (Id=0). İlk kaydet’te header DB’ye gidecek.
                dto = new IrsaliyeDto
                {
                    Id = 0,
                    irsaliyeNo = $"IRS-{DateTime.Now:yyyyMMdd-HHmmssfff}",
                    irsaliyeTarihi = DateTime.Now,
                    irsaliyeTipi = IrsaliyeTipi.Giris,
                    Detaylar = new List<IrsaliyeDetayDto>()
                };
            }

            var vm = new IrsaliyePageVm
            {
                Irsaliye = dto,
                PostModel = new IrsaliyeUpdateDto
                {
                    Id = dto.Id, // 0 olabilir
                    IrsaliyeNo = dto.irsaliyeNo,
                    CarId = dto.carId,
                    IrsaliyeTarihi = dto.irsaliyeTarihi,
                    DepoId = dto.depoId,
                    IrsaliyeTipi = dto.irsaliyeTipi,
                    Aciklama = dto.aciklama,
                    Detaylar = dto.Detaylar?.Select(d => new IrsaliyeDetayCreateDto
                    {
                        irsaliyeId = d.irsaliyeId,
                        malzemeId = d.malzemeId,
                        miktar = d.miktar,
                        birimFiyat = d.birimFiyat,
                        seriNo = d.seriNo
                    }).ToList()
                }
            };

            await PopulateDropdownsAsync(vm);
            return View(vm);
        }

        // Kaydet (Taslak upsert)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind(Prefix = "PostModel")] IrsaliyeUpdateDto post)
        {
            // Detaylar AJAX ile geliyor → validasyondan çıkar
            ModelState.Remove(nameof(post.Detaylar));

            if (!ModelState.IsValid)
            {
                // Teşhis için (geçici): hangi alan patlıyor gör
                var errs = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");
                TempData["Error"] = "Form hatalı: " + string.Join(" | ", errs);

                // VM’yi yeniden kur (dropdown’lar için)
                var vm = await BuildVmAsync(post.Id, post);
                return View("Edit", vm);
            }

            // İlk kayıt: header oluştur
            if (post.Id == 0)
            {
                var createDto = new IrsaliyeCreateDto
                {
                    IrsaliyeNo = post.IrsaliyeNo?.Trim(),
                    CarId = post.CarId,
                    DepoId = post.DepoId,
                    IrsaliyeTipi = post.IrsaliyeTipi,
                    IrsaliyeTarihi = post.IrsaliyeTarihi,
                    Aciklama = post.Aciklama
                };
                var created = await _irsaliyeService.CreateHeaderAsync(createDto);
                if (created.ResultStatus != ResultStatus.Success || created.Data == null)
                {
                    TempData["Error"] = created.Info ?? "Başlık oluşturulamadı.";
                    var vmFail = await BuildVmAsync(0, post);
                    return View("Edit", vmFail);
                }
                post.Id = created.Data.Id;
            }

            var res = await _irsaliyeService.UpsertAsync(post);
            if (res.ResultStatus == ResultStatus.Success)
            {
                TempData["Success"] = "Taslak kaydedildi.";
                return RedirectToAction(nameof(Edit), new { id = post.Id });
            }

            TempData["Error"] = res.Info ?? "Kayıt sırasında hata oluştu.";
            var vmError = await BuildVmAsync(post.Id, post);
            return View("Edit", vmError);
        }

        // Edit ekranı tekrar çizmek için küçük yardımcı
        private async Task<IrsaliyePageVm> BuildVmAsync(int id, IrsaliyeUpdateDto post)
        {
            IrsaliyeDto dto;
            if (id > 0)
            {
                var res = await _irsaliyeService.Get(id);
                dto = res.Data ?? new IrsaliyeDto { Id = id, Detaylar = new List<IrsaliyeDetayDto>() };
            }
            else
            {
                dto = new IrsaliyeDto
                {
                    Id = 0,
                    irsaliyeNo = post.IrsaliyeNo ?? $"IRS-{DateTime.Now:yyyyMMdd-HHmmssfff}",
                    irsaliyeTarihi = post.IrsaliyeTarihi == default ? DateTime.Now : post.IrsaliyeTarihi,
                    irsaliyeTipi = post.IrsaliyeTipi,
                    aciklama = post.Aciklama,
                    carId = post.CarId,
                    depoId = post.DepoId,
                    Detaylar = new List<IrsaliyeDetayDto>()
                };
            }

            var vm = new IrsaliyePageVm
            {
                Irsaliye = dto,
                PostModel = post
            };
            await PopulateDropdownsAsync(vm);
            return vm;
        }

        // Satır ekle (AJAX) – Taslak
        [HttpPost]
        public async Task<IActionResult> AddLine([FromBody] IrsaliyeDetayCreateDto dto)
        {
            if (dto == null) return BadRequest("Geçersiz veri.");
            var addRes = await _irsaliyeService.AddLineAsync(dto.irsaliyeId, dto);
            if (addRes.ResultStatus != ResultStatus.Success)
                return BadRequest(addRes.Info ?? "Satır eklenemedi.");

            var list = await _detayService.GetByIrsaliyeIdAsync(dto.irsaliyeId);
            return Ok(list.Data ?? new List<IrsaliyeDetayDto>());
        }

        // Satır sil (AJAX) – Taslak (soft)
        [HttpPost]
        public async Task<IActionResult> RemoveLine(int irsaliyeId, int detayId)
        {
            var delRes = await _irsaliyeService.RemoveLineAsync(irsaliyeId, detayId);
            if (delRes.ResultStatus != ResultStatus.Success)
                return BadRequest(delRes.Info ?? "Satır silinemedi.");

            var list = await _detayService.GetByIrsaliyeIdAsync(irsaliyeId);
            return Ok(list.Data ?? new List<IrsaliyeDetayDto>());
        }

        // Talep Oluştur (Onay)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TalepOlustur(int id)
        {
            var res = await _irsaliyeService.TalepOlusturAsync(id);
            if (res.ResultStatus == ResultStatus.Success)
            {
                TempData["Success"] = "İrsaliye onaylandı.";
                return RedirectToAction(nameof(Edit), new { id });
            }

            TempData["Error"] = res.Info ?? "Onay sırasında hata oluştu.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Sil (soft) – sadece Taslak
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _irsaliyeService.Delete(id);
            if (res.ResultStatus == ResultStatus.Success)
            {
                TempData["Success"] = "İrsaliye silindi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = res.Info ?? "Silme sırasında hata oluştu.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Details (salt okunur)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var res = await _irsaliyeService.Get(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
            {
                TempData["Error"] = res.Info ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            return View(res.Data);
        }

        // Print (yazdırılabilir)
        [HttpGet]
        public async Task<IActionResult> Print(int id)
        {
            var res = await _irsaliyeService.Get(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
            {
                return NotFound();
            }
            return View(res.Data);
        }
    }
}
