using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Dtos.DepoTransferDtos;
using StokTakip.Service.Abstract;
using System.Threading.Tasks;

namespace StokTakip.Web.Controllers
{
    
    public class DepoTransferController : Controller
    {
        private readonly IDepoTransferService _transferService;
        private readonly IDepoTransferDetayService _detayService;

        public DepoTransferController(IDepoTransferService transferService,
                                      IDepoTransferDetayService detayService)
        {
            _transferService = transferService;
            _detayService = detayService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _transferService.GetAllAsync();
            return View(res.Data); // IEnumerable<DepoTransferListDto>
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var res = await _transferService.GetAsync(id);
            if (res.ResultStatus != Shared.Utilities.ComplexTypes.ResultStatus.Success || res.Data == null)
            {
                TempData["Error"] = res.Info ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Satırlar
            var lines = await _transferService.GetLinesAsync(id);
            res.Data.Detaylar = lines.Data ?? res.Data.Detaylar;

            return View(res.Data); // DepoTransferDto (Detaylar dolu)
        }

        // ---------- CREATE (Taslak) ----------
        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepoTransferCreateDto { TransferTarihi = System.DateTime.Now });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepoTransferCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var res = await _transferService.CreateHeaderAsync(dto);
            if (res.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
            {
                TempData["Success"] = "Taslak oluşturuldu.";
                return RedirectToAction(nameof(Edit), new { id = res.Data.Id });
            }

            TempData["Error"] = res.Info ?? "Oluşturma başarısız.";
            return View(dto);
        }

        // ---------- EDIT (yalnız Taslak) ----------
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _transferService.GetAsync(id);
            if (res.ResultStatus != Shared.Utilities.ComplexTypes.ResultStatus.Success || res.Data == null)
            {
                TempData["Error"] = res.Info ?? "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Durum Onaylı ise salt okunur sayfaya yönlendirmek istersen:
            if (res.Data.Durum == Entities.Enums.TransferDurumu.Onayli)
                return RedirectToAction(nameof(Details), new { id });

            // Satırları yükle
            var lines = await _transferService.GetLinesAsync(id);
            res.Data.Detaylar = lines.Data ?? res.Data.Detaylar;

            // Edit view’ında header alanlarını DepoTransferUpdateDto ile bind edeceğiz
            var updateDto = new DepoTransferUpdateDto
            {
                Id = res.Data.Id,
                TransferNo = res.Data.TransferNo,
                KaynakDepoId = res.Data.KaynakDepoId,
                HedefDepoId = res.Data.HedefDepoId,
                TransferTarihi = res.Data.TransferTarihi,
                Aciklama = res.Data.Aciklama,
                SeriNo = res.Data.SeriNo
            };

            ViewBag.Transfer = res.Data; // başlık + detaylar görünsün
            return View(updateDto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepoTransferUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                // ViewBag.Transfer için başlığı tekrar getir
                var re = await _transferService.GetAsync(dto.Id);
                ViewBag.Transfer = re.Data;
                return View(dto);
            }

            var res = await _transferService.UpsertAsync(dto);
            if (res.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
            {
                TempData["Success"] = "Taslak güncellendi.";
                return RedirectToAction(nameof(Edit), new { id = dto.Id });
            }

            TempData["Error"] = res.Info ?? "Güncelleme başarısız.";
            // Başlık/detay gösterimi için yeniden yükle
            var re2 = await _transferService.GetAsync(dto.Id);
            ViewBag.Transfer = re2.Data;
            return View(dto);
        }

        // ---------- DELETE (soft, yalnız Taslak) ----------
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _transferService.DeleteAsync(id);
            if (res.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
            {
                TempData["Success"] = "Transfer silindi.";
            }
            else TempData["Error"] = res.Info ?? "Silme başarısız.";

            return RedirectToAction(nameof(Index));
        }

        // ---------- LINES (Taslak) ----------
        [HttpPost]
        public async Task<IActionResult> AddLine(DepoTransferDetayCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz satır verisi.");

            var res = await _transferService.AddLineAsync(dto.TransferId, dto);
            if (res.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
                return Ok("Satır eklendi.");

            return BadRequest(res.Info ?? "Satır eklenemedi.");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLine(int transferId, int detayId)
        {
            var res = await _transferService.RemoveLineAsync(transferId, detayId);
            if (res.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
                return Ok("Satır silindi.");

            return BadRequest(res.Info ?? "Satır silinemedi.");
        }

        [HttpGet]
        public async Task<IActionResult> GetLines(int id)
        {
            var res = await _transferService.GetLinesAsync(id);
            return PartialView("_Lines", res.Data); // _Lines.cshtml: IEnumerable<DepoTransferDetayDto>
        }

        // ---------- ONAY ----------
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(int id)
        {
            var user = User?.Identity?.Name ?? "system";
            var res = await _transferService.TalepOlusturAsync(id, user);

            if (res.ResultStatus == Shared.Utilities.ComplexTypes.ResultStatus.Success)
            {
                TempData["Success"] = "Onay başarılı; stoklara işlendi.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["Error"] = res.Info ?? "Onay sırasında hata oluştu.";
            return RedirectToAction(nameof(Edit), new { id });
        }
    }
}
