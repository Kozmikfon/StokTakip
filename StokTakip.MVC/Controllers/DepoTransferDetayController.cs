using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;

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
            var result = await _detayService.GetAllByTransferIdAsync(transferId);
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

            var result = await _detayService.Create(dto);

            if (result.ResultStatus == ResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Info;
                return RedirectToAction("Index", new { transferId = dto.TransferId });
            }

            ModelState.AddModelError("", result.Info);
            return View(dto);
        }

        // Detay silme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int transferId)
        {
            var result = await _detayService.Delete(id);

            if (result.ResultStatus == ResultStatus.Success)
                TempData["SuccessMessage"] = result.Info;
            else
                TempData["ErrorMessage"] = result.Info;

            return RedirectToAction("Index", new { transferId });
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var result = await _detayService.Get(id);
            if (result.ResultStatus == ResultStatus.Success)
                return View(result.Data);

            TempData["ErrorMessage"] = result.Info;
            return RedirectToAction("Index", new { transferId = 1 });
        }
    }
}
