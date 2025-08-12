using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Dtos.MalzemeDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StokTakip.MVC.Controllers
{
    public class IrsaliyeController : Controller
    {
        private readonly IIrsaliyeService _irsaliyeService;
        private readonly IIrsaliyeDetayService _irsaliyeDetayService;
        private readonly IDepoService _depoService;
        private readonly ICariService _cariService;
        private readonly IMalzemeService _malzemeService;

        public IrsaliyeController(
            IIrsaliyeService irsaliyeService,
            IIrsaliyeDetayService irsaliyeDetayService,
            IDepoService depoService,
            ICariService cariService,
            IMalzemeService malzemeService)
        {
            _irsaliyeService = irsaliyeService;
            _irsaliyeDetayService = irsaliyeDetayService;
            _depoService = depoService;
            _cariService = cariService;
            _malzemeService = malzemeService;
        }

        private async Task LoadLookupsAsync()
        {
            var depolarRes = await _depoService.GetAllAsync();
            var carilerRes = await _cariService.GetAllAsync();
            var malzemelerRes = await _malzemeService.GetAllAsync();

            ViewBag.Cariler = (carilerRes.Data ?? new List<CariListDto>())
                .Where(c => c != null)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Unvan ?? string.Empty
                })
                .ToList();

            ViewBag.Depolar = (depolarRes.Data ?? new List<DepoListDto>())
                .Where(d => d != null)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.DepoAd ?? string.Empty
                })
                .ToList();

            ViewBag.Malzemeler = (malzemelerRes.Data ?? new List<MalzemeListDto>())
                .Where(m => m != null)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.malzemeAdi ?? string.Empty
                })
                .ToList();
        }


        // LIST
        public async Task<IActionResult> Index()
        {
            var res = await _irsaliyeService.GetAllByNonDeleteAsync();
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
                return View(new List<IrsaliyeDto>());
            return View(res.Data); // @model List<IrsaliyeDto>
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var res = await _irsaliyeService.Get(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
                return NotFound();

            await LoadLookupsAsync(); // satır ekleme formu için malzemeler lazım
            return View(res.Data);    // @model IrsaliyeDto
        }

        // CREATE GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadLookupsAsync();
            return View(new IrsaliyeCreateDto()); // Detaylar YOK
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IrsaliyeCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();      // <-- ZORUNLU
                return View(model);
            }

            var res = await _irsaliyeService.CreateHeaderAsync(model);
            if (res.ResultStatus == ResultStatus.Success && res.Data != null)
                return RedirectToAction("Entry", "IrsaliyeDetay", new { irsaliyeId = res.Data.Irsaliye.Id });

            ModelState.AddModelError("", res.Info ?? "Başlık kaydedilemedi.");
            await LoadLookupsAsync();          // <-- ZORUNLU
            return View(model);
        }


        // EDIT GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _irsaliyeService.Get(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
                return NotFound();

            await LoadLookupsAsync();

            var h = res.Data.Irsaliye;
            var dto = new IrsaliyeUpdateDto
            {
                Id = h.Id,
                DepoId = h.depoId,
                IrsaliyeTipi = h.irsaliyeTipi,
                Tarih = h.irsaliyeTarihi,
                Detaylar = res.Data.Detaylar?.Select(d => new IrsaliyeDetayCreateDto
                {
                    irsaliyeId = h.Id, // serviste yine set ediyorsun; 0 da olabilir
                    malzemeId = d.malzemeId,
                    miktar = d.miktar,
                    birimFiyat = d.birimFiyat,
                    seriNo = d.seriNo
                }).ToList()
            };
            return View(dto); // @model IrsaliyeUpdateDto
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IrsaliyeUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return View(model);
            }

            if (model.Detaylar == null || !model.Detaylar.Any())
            {
                ModelState.AddModelError("", "En az bir irsaliye satırı giriniz.");
                await LoadLookupsAsync();
                return View(model);
            }

            var res = await _irsaliyeService.Update(model);
            if (res.ResultStatus == ResultStatus.Success && res.Data != null)
                return RedirectToAction(nameof(Details), new { id = res.Data.Irsaliye.Id });

            ModelState.AddModelError("", res.Info ?? "Güncelleme sırasında bir hata oluştu.");
            await LoadLookupsAsync();
            return View(model);
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _irsaliyeService.Delete(id);
            if (res.ResultStatus == ResultStatus.Success)
                return RedirectToAction(nameof(Index));

            TempData["Error"] = res.Info ?? "Silme sırasında bir hata oluştu.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // --- Satır Bazlı (AJAX) ---
        [HttpPost]
        public async Task<IActionResult> AddLine(int irsaliyeId, IrsaliyeDetayCreateDto line)
        {
            ModelState.Remove(nameof(IrsaliyeDetayCreateDto.irsaliyeId));
            line.irsaliyeId = irsaliyeId;

            if (!ModelState.IsValid)
                return BadRequest("Geçersiz satır.");

            var res = await _irsaliyeService.AddLineAsync(irsaliyeId, line);
            if (res.ResultStatus == ResultStatus.Success) return Ok();
            return BadRequest(res.Info ?? "Satır eklenemedi.");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLine(int irsaliyeId, int detayId)
        {
            var res = await _irsaliyeService.RemoveLineAsync(irsaliyeId, detayId);
            if (res.ResultStatus == ResultStatus.Success) return Ok();
            return BadRequest(res.Info ?? "Satır silinemedi.");
        }
    }
}
