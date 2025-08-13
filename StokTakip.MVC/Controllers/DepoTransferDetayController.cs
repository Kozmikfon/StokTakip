using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Service.Abstract;

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

        // Listeleme
        public async Task<IActionResult> Index(int transferId)
        {
            var result = await _detayService.GetAllByTransferIdAsync(transferId);
            ViewBag.TransferId = transferId;

            if (result.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
                return View(result.Data);

            TempData["ErrorMessage"] = result.Info;
            return RedirectToAction("Index", "DepoTransfer");
        }

        // GET: Create
        public IActionResult Create(int transferId)
        {
            var dto = new DepoTransferDetayCreateDto { TransferId = transferId };
            return View(dto);
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(DepoTransferDetayCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _detayService.Create(dto);

            if (result.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Info;
                return RedirectToAction("Index", new { transferId = dto.TransferId });
            }

            ModelState.AddModelError("", result.Info);
            return View(dto);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id, int transferId)
        {
            var result = await _detayService.Delete(id);

            if (result.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
                TempData["SuccessMessage"] = result.Info;
            else
                TempData["ErrorMessage"] = result.Info;

            return RedirectToAction("Index", new { transferId });
        }
    }
}
