using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Dtos.DepoTransferDtos;
using StokTakip.Service.Abstract;
using StokTakip.Service.Concrete;
using StokTakip.Shared.Utilities.ComplexTypes;
using System.Threading.Tasks;

namespace StokTakip.MVC.Controllers
{
    public class DepoTransferController : Controller
    {
        private readonly IDepoTransferService _depoTransferService;
        private readonly IMalzemeService _malzemeService; 

        public DepoTransferController(IDepoTransferService depoTransferService, IMalzemeService malzemeService)
        {
            _depoTransferService = depoTransferService;
            _malzemeService = malzemeService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _depoTransferService.GetAllByNonDeleteAsync();
            return View(result.Data); // List<DepoTransferListDto>
        }

        [HttpGet]
        public async Task<IActionResult> Create(int transferId)
        {
            var dto = new DepoTransferCreateDto();
            return View(dto); // Burası Views/DepoTransfer/Create.cshtml açmalı
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
