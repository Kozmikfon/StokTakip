using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;

namespace StokTakip.MVC.Controllers
{
    public class DepoController : Controller
    {
        private readonly IDepoService _depoService;
        public DepoController(IDepoService depoService)
        {
            _depoService = depoService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _depoService.GetAllAsync(); // Not: Serviste GetAll yoksa ekleyin
            if (result.ResultStatus == ResultStatus.Success)
                return View(result.Data);

            TempData["Error"] = result.Info ?? "Depo listesi alınamadı.";
            return View(new List<DepoListDto>());
        }

        // GET: /Depo/Details/5
        // GET: /Depo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _depoService.Get(id);
            if (result.ResultStatus != ResultStatus.Success || result.Data == null)
            {
                TempData["Error"] = result.Info ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        // GET: /Depo/Create
        public IActionResult Create() => View(new DepoCreateDto());

        // POST: /Depo/Create
        [HttpPost]
        public async Task<IActionResult> Create(DepoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _depoService.Create(dto);
            if (result.ResultStatus == ResultStatus.Success)
            {
                TempData["Success"] = "Depo başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Info ?? "Depo eklenemedi.";
            return View(dto);
        }

        // POST: /Depo/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(DepoUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _depoService.Update(dto);
            if (result.ResultStatus == ResultStatus.Success)
            {
                TempData["Success"] = "Depo güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Info ?? "Güncelleme başarısız.";
            return View(dto);
        }
        // POST: /Depo/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _depoService.Delete(id);
            TempData[result.ResultStatus == ResultStatus.Success ? "Success" : "Error"] =
                result.ResultStatus == ResultStatus.Success ? "Kayıt silindi." : (result.Info ?? "Silme başarısız.");
            return RedirectToAction(nameof(Index));
        }
    }
}
