using Microsoft.AspNetCore.Mvc;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Service.Abstract;
using System.Threading.Tasks;

namespace StokTakip.WebUI.Controllers
{
    // Sadece Detay (satır) işlemlerini AJAX ile yönetir.
    // Stok oynamaz; sadece Taslakta çalışır.
    public class DetayController : Controller
    {
        private readonly IIrsaliyeDetayService _detayService;
        private readonly IIrsaliyeService _irsaliyeService;

        public DetayController(IIrsaliyeDetayService detayService, IIrsaliyeService irsaliyeService)
        {
            _detayService = detayService;
            _irsaliyeService = irsaliyeService;
        }

        // Satır ekle (Taslak)
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] IrsaliyeDetayCreateDto dto)
        {
            if (dto == null) return BadRequest("Geçersiz veri.");

            var addRes = await _irsaliyeService.AddLineAsync(dto.irsaliyeId, dto);
            if (addRes.ResultStatus != StokTakip.Shared.Utilities.ComplexTypes.ResultStatus.Success)
                return BadRequest(addRes.Info ?? "Satır eklenemedi.");

            var list = await _detayService.GetByIrsaliyeIdAsync(dto.irsaliyeId);
            return Ok(list.Data); // tabloyu yeniden basmak için
        }

        // Satır sil (Taslak)
        [HttpPost]
        public async Task<IActionResult> Remove(int irsaliyeId, int detayId)
        {
            var delRes = await _irsaliyeService.RemoveLineAsync(irsaliyeId, detayId);
            if (delRes.ResultStatus != StokTakip.Shared.Utilities.ComplexTypes.ResultStatus.Success)
                return BadRequest(delRes.Info ?? "Satır silinemedi.");

            var list = await _detayService.GetByIrsaliyeIdAsync(irsaliyeId);
            return Ok(list.Data);
        }
    }
}
