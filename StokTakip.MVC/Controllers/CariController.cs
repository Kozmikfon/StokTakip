using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;

namespace StokTakip.MVC.Controllers
{
    public class CariController : Controller
    {
         ICariService _cariService;
        public CariController(ICariService cariService)
        {
            _cariService = cariService;
        }
        public async Task<IActionResult> Index()
        {
            // Hizli not: Serviste GetAllAsync yoksa ekleyelim (IDataResult<List<CariDto>>)
            var res = await _cariService.GetAllAsync();
            if (res.ResultStatus != ResultStatus.Success)
            {
                TempData["Error"] = res.Info ?? "Cari listesi alınamadı.";
                return View(new List<CariDto>());
            }
           
            return View(res.Data ?? new List<CariListDto>());
            

        }

        // GET: /Cari/Create
        public IActionResult Create()
        {
            return View(new CariCreateDto());
        }

        // POST: /Cari/Create
        [HttpPost]
        public async Task<IActionResult> Create(CariCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var res = await _cariService.CreateAsync(dto);
            if (res.ResultStatus != ResultStatus.Success)
            {
                TempData["Error"] = res.Info ?? "Cari oluşturulamadı.";
                return View(dto);
            }

            TempData["Success"] = res.Info ?? "Cari başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Cari/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var res = await _cariService.GetAsync(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
            {
                TempData["Error"] = res.Info ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            return View(res.Data);
        }

        // GET: /Cari/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _cariService.GetAsync(id);
            if (res.ResultStatus != ResultStatus.Success || res.Data == null)
            {
                TempData["Error"] = res.Info ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new CariUpdateDto
            {
                Id = res.Data.Id,
                Unvan = res.Data.Unvan,
                Telefon = res.Data.Telefon,
                Email = res.Data.Email,
                Adres = res.Data.Adres,
                VergiNo = res.Data.VergiNo,
                VergiDairesi = res.Data.VergiDairesi
            };
            return View(vm);
        }

        // POST: /Cari/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(CariUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var res = await _cariService.UpdateAsync(dto);
            if (res.ResultStatus != ResultStatus.Success)
            {
                TempData["Error"] = res.Info ?? "Cari güncellenemedi.";
                return View(dto);
            }

            TempData["Success"] = res.Info ?? "Cari güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cari/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _cariService.DeleteAsync(id);
            TempData[res.ResultStatus == ResultStatus.Success ? "Success" : "Error"] =
                res.Info ?? (res.ResultStatus == ResultStatus.Success ? "Silindi" : "Silinemedi");
            return RedirectToAction(nameof(Index));
        }
    }
}