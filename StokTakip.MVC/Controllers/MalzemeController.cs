using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Service.Abstract;
using StokTakip.Entities.Dtos.MalzemeDtos;
using StokTakip.Shared.Utilities.ComplexTypes; // ResultStatus için

namespace StokTakip.MVC.Controllers
{
    
    public class MalzemeController : Controller
    {
        private readonly IMalzemeService _malzemeService;

        public MalzemeController(IMalzemeService malzemeService)
        {
            _malzemeService = malzemeService;
        }

        // GET: /Malzeme
        public async Task<IActionResult> Index()
        {
            var res = await _malzemeService.GetAllAsync();

            if (res.ResultStatus != ResultStatus.Success)
            {
                TempData["Error"] = string.IsNullOrWhiteSpace(res.Info)
                    ? "Malzeme listesi alınamadı."
                    : res.Info;

                return View(new List<MalzemeListDto>());
            }

            return View(res.Data ?? new List<MalzemeListDto>());
        }

        // GET: /Malzeme/Create
        public IActionResult Create()
        {
            return View(new MalzemeCreateDto());
        }

        // POST: /Malzeme/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MalzemeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var res = await _malzemeService.CreateAsync(dto);

            if (res.ResultStatus != ResultStatus.Success)
            {
                TempData["Error"] = string.IsNullOrWhiteSpace(res.Info)
                    ? "Malzeme oluşturulamadı."
                    : res.Info;

                return View(dto);
            }

            TempData["Success"] = string.IsNullOrWhiteSpace(res.Info)
                ? "Malzeme başarıyla oluşturuldu."
                : res.Info;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var res = await _malzemeService.GetAsync(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
            {
                TempData["Error"] = string.IsNullOrWhiteSpace(res.Info) ? "Kayıt bulunamadı." : res.Info;
                return RedirectToAction(nameof(Index));
            }
            return View(res.Data); // Views/Malzeme/Details.cshtml (MalzemeDto)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _malzemeService.DeleteAsync(id);

            if (res.ResultStatus != ResultStatus.Success)
                TempData["Error"] = "Malzeme silinirken bir hata oluştu.";
            else
                TempData["Success"] = "Malzeme başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }

    }
}
