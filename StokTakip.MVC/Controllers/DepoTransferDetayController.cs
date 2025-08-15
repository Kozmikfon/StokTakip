using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using System.Threading.Tasks;

namespace StokTakip.MVC.Controllers
{
    public class DepoTransferDetayController : Controller
    {
        private readonly IDepoTransferDetayService _detayService;
        private readonly IDepoTransferService _transferService;

        public DepoTransferDetayController(IDepoTransferDetayService detayService, IDepoTransferService transferService)
        {
            _detayService = detayService;
            _transferService = transferService;
        }

        // 🧾 Transfer detaylarını listele
        public async Task<IActionResult> Index(int transferId)
        {
            var result = await _detayService.GetByTransferIdAsync(transferId);  // <-- isim düzeltildi
            ViewBag.TransferId = transferId;

            if (result.ResultStatus == ResultStatus.Success)
                return View(result.Data);

            TempData["ErrorMessage"] = result.Info;
            return RedirectToAction("Index", "DepoTransfer");
        }

        // ➕ Yeni detay formu (GET)
        public async Task<IActionResult> Create(int transferId)
        {
            var transfer = await _transferService.GetAsync(transferId);
            if (transfer == null || transfer.Data == null)
            {
                TempData["ErrorMessage"] = "Transfer bilgisi bulunamadı.";
                return RedirectToAction("Index", "DepoTransfer");
            }

            var dto = new DepoTransferDetayCreateDto
            {
                TransferId = transferId
            };

            return View(dto);
        }

        // ➕ Yeni detay kaydı (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepoTransferDetayCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _detayService.CreateAsync(dto);   // <-- isim düzeltildi

            if (result.ResultStatus == ResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Info;
                return RedirectToAction("Index", new { transferId = dto.TransferId });
            }

            ModelState.AddModelError("", result.Info);
            return View(dto);
        }

        // 🗑️ Detay silme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int transferId)
        {
            var result = await _detayService.DeleteAsync(id);    // <-- isim düzeltildi

            if (result.ResultStatus == ResultStatus.Success)
                TempData["SuccessMessage"] = result.Info;
            else
                TempData["ErrorMessage"] = result.Info;

            return RedirectToAction("Index", new { transferId });
        }

        // ℹ️ Tekil detay GET arayüzde yok -> bu action'ı şimdilik kapatalım
        // Eğer gerekli ise, IDepoTransferDetayService'e GetAsync(int detayId) ekleyelim.
        /*
        public async Task<IActionResult> Details(int id, int transferId)
        {
            var all = await _detayService.GetByTransferIdAsync(transferId);
            var item = all.Data?.FirstOrDefault(x => x.Id == id);
            if (item != null) return View(item);

            TempData["ErrorMessage"] = "Detay bulunamadı.";
            return RedirectToAction("Index", new { transferId });
        }
        */
    }
}
