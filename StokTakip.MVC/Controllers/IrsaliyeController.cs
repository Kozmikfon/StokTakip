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

namespace StokTakip.WebUI.Controllers
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

        // Dropdownları güvenli doldur (Value/Text projeksiyonu)
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

        // Edit (tek sayfa) – id yoksa dinamik taslak oluştur
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
                // Dinamik default (ilk kayıtlar)
                var cariler = (await _cariService.GetAllAsync()).Data ?? new List<CariListDto>();
                var depolar = (await _depoService.GetAllAsync()).Data ?? new List<DepoListDto>();

                if (!cariler.Any() || !depolar.Any())
                {
                    TempData["Error"] = "Önce en az bir Cari ve Depo kaydı oluşturmalısınız.";
                    return RedirectToAction(nameof(Index));
                }

                var createDto = new IrsaliyeCreateDto
                {
                    IrsaliyeNo = $"IRS-{DateTime.Now:yyyyMMdd-HHmmssfff}",
                    CarId = cariler.First().Id,
                    DepoId = depolar.First().Id,
                    IrsaliyeTipi = IrsaliyeTipi.Giris,
                    IrsaliyeTarihi = DateTime.Now
                };

                var created = await _irsaliyeService.CreateHeaderAsync(createDto);
                if (created.ResultStatus != ResultStatus.Success || created.Data == null)
                {
                    TempData["Error"] = created.Info ?? "Başlık oluşturulamadı.";
                    return RedirectToAction(nameof(Index));
                }
                dto = created.Data;
            }

            var vm = new IrsaliyePageVm
            {
                Irsaliye = dto,
                PostModel = new IrsaliyeUpdateDto
                {
                    Id = dto.Id,
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
        public async Task<IActionResult> Save(IrsaliyePageVm model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model);
                TempData["Error"] = "Form hatalı. Lütfen kontrol edin.";
                return View("Edit", model);
            }

            var res = await _irsaliyeService.UpsertAsync(model.PostModel);
            if (res.ResultStatus == ResultStatus.Success)
            {
                TempData["Success"] = "Taslak kaydedildi.";
                return RedirectToAction(nameof(Edit), new { id = model.PostModel.Id });
            }

            TempData["Error"] = res.Info ?? "Kayıt sırasında hata oluştu.";
            await PopulateDropdownsAsync(model);
            return View("Edit", model);
        }

        // Satır ekle (AJAX) – Taslak
        [HttpPost]
        public async Task<IActionResult> AddLine([FromBody] IrsaliyeDetayCreateDto dto)
        {
            if (dto == null) return BadRequest("Geçersiz veri.");

            var res = await _irsaliyeService.AddLineAsync(dto.irsaliyeId, dto);
            if (res.ResultStatus != ResultStatus.Success)
                return BadRequest(res.Info ?? "Satır eklenemedi.");

            var list = await _detayService.GetByIrsaliyeIdAsync(dto.irsaliyeId);
            return Ok(list.Data ?? new List<IrsaliyeDetayDto>());
        }

        // Satır sil (AJAX) – Taslak (soft)
        [HttpPost]
        public async Task<IActionResult> RemoveLine(int irsaliyeId, int detayId)
        {
            var res = await _irsaliyeService.RemoveLineAsync(irsaliyeId, detayId);
            if (res.ResultStatus != ResultStatus.Success)
                return BadRequest(res.Info ?? "Satır silinemedi.");

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
