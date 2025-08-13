using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.MalzemeDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StokTakip.MVC.Controllers
{
    public class IrsaliyeDetayController : Controller
    {
        private readonly IIrsaliyeDetayService _detayService;
        private readonly IIrsaliyeService _irsaliyeService;     // Entry ekranında başlığı göstermek için
        private readonly IMalzemeService _malzemeService;       // Dropdown doldurmak için

        public IrsaliyeDetayController(
            IIrsaliyeDetayService detayService,
            IIrsaliyeService irsaliyeService,
            IMalzemeService malzemeService)
        {
            _detayService = detayService;
            _irsaliyeService = irsaliyeService;
            _malzemeService = malzemeService;
        }

        // --- COMMON: LOOKUPS ---

        private async Task LoadMalzemeAsync()
        {
            var m = await _malzemeService.GetAllAsync(); // IDataResult<List<MalzemeListDto>>
            var list = m.Data ?? new List<MalzemeListDto>();
            ViewBag.Malzemeler = new SelectList(list, "Id", "malzemeAdi");
        }

        // --- JSON LIST (AJAX tablo için) ---

        // GET: /IrsaliyeDetay/List?irsaliyeId=5
        [HttpGet]
        public async Task<IActionResult> List(int irsaliyeId)
        {
            var res = await _detayService.GetByIrsaliyeIdAsync(irsaliyeId);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
                return Json(new { ok = false, message = res.Info ?? "Detaylar getirilemedi." });

            return Json(new { ok = true, data = res.Data });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int irsaliyeId, IrsaliyeDetayCreateDto model)
        {
            // DTO'da irsaliyeId [Required]; route ile geldiği için model içinde olmayabilir:
            ModelState.Remove(nameof(IrsaliyeDetayCreateDto.irsaliyeId));
            model.irsaliyeId = irsaliyeId;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Geçersiz detay verisi.";
                return RedirectToAction("Details", "Irsaliye", new { id = irsaliyeId });
            }

            var res = await _detayService.CreateAsync(model);
            if (res.ResultStatus == ResultStatus.Success)
            {
                TempData["Ok"] = "Detay eklendi ve stok güncellendi.";
                return RedirectToAction("Details", "Irsaliye", new { id = irsaliyeId });
            }

            TempData["Error"] = res.Info ?? "Detay eklenemedi.";
            return RedirectToAction("Details", "Irsaliye", new { id = irsaliyeId });
        }

        // POST: /IrsaliyeDetay/Delete
        // Details sayfasındaki "Sil" butonu buraya post eder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int irsaliyeId, int detayId)
        {
            var res = await _detayService.DeleteAsync(detayId);
            if (res.ResultStatus == ResultStatus.Success)
            {
                TempData["Ok"] = "Detay silindi ve stok geri alındı.";
                return RedirectToAction("Details", "Irsaliye", new { id = irsaliyeId });
            }

            TempData["Error"] = res.Info ?? "Detay silinemedi.";
            return RedirectToAction("Details", "Irsaliye", new { id = irsaliyeId });
        }

        [HttpGet]
        public async Task<IActionResult> Entry(int irsaliyeId)
        {
            var header = await _irsaliyeService.Get(irsaliyeId);
            if (header.ResultStatus != ResultStatus.Success || header.Data == null)
                return NotFound();

            var dets = await _detayService.GetByIrsaliyeIdAsync(irsaliyeId);

            await LoadMalzemeAsync();
            ViewBag.Irsaliye = header.Data.Irsaliye; // Entry sayfasında üst bilgiler için
            return View(dets.Data ?? new List<IrsaliyeDetayDto>()); // Views/IrsaliyeDetay/Entry.cshtml
        }

        // POST: /IrsaliyeDetay/AddLineEntry
        // Entry ekranından satır ekle ve Entry'e geri dön
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLineEntry(int irsaliyeId, IrsaliyeDetayCreateDto model)
        {
            ModelState.Remove(nameof(IrsaliyeDetayCreateDto.irsaliyeId));
            model.irsaliyeId = irsaliyeId;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Geçersiz satır.";
                return RedirectToAction(nameof(Entry), new { irsaliyeId });
            }

            var res = await _detayService.CreateAsync(model);
            TempData[res.ResultStatus == ResultStatus.Success ? "Ok" : "Error"]
                = res.Info ?? (res.ResultStatus == ResultStatus.Success ? "Satır eklendi." : "Satır eklenemedi.");
            return RedirectToAction(nameof(Entry), new { irsaliyeId });
        }

        // POST: /IrsaliyeDetay/DeleteEntry
        // Entry ekranından satır sil ve Entry'e geri dön
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEntry(int irsaliyeId, int detayId)
        {
            var res = await _detayService.DeleteAsync(detayId);
            TempData[res.ResultStatus == ResultStatus.Success ? "Ok" : "Error"]
                = res.Info ?? (res.ResultStatus == ResultStatus.Success ? "Satır silindi." : "Satır silinemedi.");
            return RedirectToAction(nameof(Entry), new { irsaliyeId });
        }
    }
}
