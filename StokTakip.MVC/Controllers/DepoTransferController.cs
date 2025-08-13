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
        private readonly IDepoService _depoService;

        public DepoTransferController(IDepoTransferService depoTransferService, IMalzemeService malzemeService,IDepoService depoService)
        {
            _depoTransferService = depoTransferService;
            _malzemeService = malzemeService;
            _depoService = depoService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _depoTransferService.GetAllByNonDeleteAsync();
            return View(result.Data); // List<DepoTransferListDto>
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var depolar = await _depoService.GetAllAsync();
            var selectList = depolar.Data.Select(d => new SelectListItem
            {
                Text = d.DepoAd,
                Value = d.Id.ToString()
            }).ToList();

            ViewBag.KaynakDepolar = selectList;
            ViewBag.HedefDepolar = selectList;

            return View(new DepoTransferCreateDto());
        }



        [HttpPost]
        public async Task<IActionResult> Create(DepoTransferCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Hatalı model durumunda dropdown'lar tekrar yüklenmeli
                var depolar = await _depoService.GetAllAsync();
                var selectList = depolar.Data.Select(d => new SelectListItem
                {
                    Text = d.DepoAd,
                    Value = d.Id.ToString()
                }).ToList();

                ViewBag.KaynakDepolar = selectList;
                ViewBag.HedefDepolar = selectList;

                return View(dto);
            }

            var result = await _depoTransferService.CreateAsync(dto);

            if (result.ResultStatus == ResultStatus.Success)
            {
                TempData["SuccessMessage"] = result.Info;
                return RedirectToAction("Create", "DepoTransferDetay", new { transferId = result.Data.Id });
            }

            // Başarısız durumda da dropdown'lar tekrar yüklenmeli
            var depolarFail = await _depoService.GetAllAsync();
            var selectListFail = depolarFail.Data.Select(d => new SelectListItem
            {
                Text = d.DepoAd,
                Value = d.Id.ToString()
            }).ToList();

            ViewBag.KaynakDepolar = selectListFail;
            ViewBag.HedefDepolar = selectListFail;

            ModelState.AddModelError("", result.Info);
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
