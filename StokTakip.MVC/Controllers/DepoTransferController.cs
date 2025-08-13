using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.DepoTransferDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using System.Threading.Tasks;

namespace StokTakip.MVC.Controllers
{
    public class DepoTransferController : Controller
    {
        private readonly IDepoTransferService _depoTransferService;

        public DepoTransferController(IDepoTransferService depoTransferService)
        {
            _depoTransferService = depoTransferService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _depoTransferService.GetAllByNonDeleteAsync();
            return View(result.Data); // List<DepoTransferListDto>
        }

        public IActionResult Create()
        {
            return View(); // View içine depo dropdown'ları koyarız
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepoTransferCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _depoTransferService.CreateAsync(dto);

            if (result.ResultStatus == ResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Info; // örneğin: "Transfer başarıyla oluşturuldu"
                return RedirectToAction("Create", "DepoTransferDetay", new { transferId = result.Data.Id });
            }

            ModelState.AddModelError("", result.Info); // hata mesajı gösterilecek
            return View(dto);
        }


        public async Task<IActionResult> Details(int id)
        {
            var result = await _depoTransferService.GetAsync(id);
            if (result.Data == null)
                return NotFound();

            return View(result.Data); // View'da detaylara link verilir
        }
    }
}
