using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Enums;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Entities.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using StokTakip.Shared.Utilities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class DepoTransferDetayService : IDepoTransferDetayService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DepoTransferDetayService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        private static void SoftDelete(EntityBase e)
        {
            e.IsDelete = true;
            e.IsActive = false;
            e.ModifiedTime = DateTime.Now;
        }

        public async Task<IDataResult<DepoTransferDetayDto>> CreateAsync(DepoTransferDetayCreateDto dto)
        {
            if (dto == null)
                return new DataResult<DepoTransferDetayDto>(ResultStatus.Error, "Geçersiz veri.", null);

            var tr = await _uow.DepoTransfer.GetAsync(t => t.Id == dto.TransferId && !t.IsDelete);
            if (tr == null)
                return new DataResult<DepoTransferDetayDto>(ResultStatus.Error, "Transfer bulunamadı.", null);

            if (tr.durum != TransferDurumu.Taslak)
                return new DataResult<DepoTransferDetayDto>(ResultStatus.Error, "Onaylı transfere satır eklenemez.", null);

            var det = _mapper.Map<DepoTransferDetay>(dto);
            det.IsActive = true;
            det.IsDelete = false;
            det.CreatedTime = DateTime.Now;
            det.ModifiedTime = DateTime.Now;

            await _uow.DepoTransferDetay.AddAsync(det);
            await _uow.SaveAsync();

            var outDto = _mapper.Map<DepoTransferDetayDto>(det);
            return new DataResult<DepoTransferDetayDto>(ResultStatus.Success, "Satır eklendi (Taslak).", outDto);
        }

        public async Task<IResult> DeleteAsync(int detayId)
        {
            var det = await _uow.DepoTransferDetay.GetAsync(d => d.Id == detayId && !d.IsDelete);
            if (det == null)
                return new Result(ResultStatus.Error, "Detay bulunamadı.");

            var tr = await _uow.DepoTransfer.GetAsync(t => t.Id == det.transferId && !t.IsDelete);
            if (tr == null)
                return new Result(ResultStatus.Error, "Transfer bulunamadı.");

            if (tr.durum != TransferDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı transferden satır silinemez.");

            SoftDelete(det);
            await _uow.DepoTransferDetay.UpdateAsync(det);
            await _uow.SaveAsync();

            return new Result(ResultStatus.Success, "Satır silindi (soft).");
        }

        public async Task<IDataResult<List<DepoTransferDetayDto>>> GetByTransferIdAsync(int transferId)
        {
            var list = await _uow.DepoTransferDetay.GetAllAsync(
                d => d.transferId == transferId && !d.IsDelete,
                d => d.malzeme);

            var dto = _mapper.Map<List<DepoTransferDetayDto>>(list);
            return new DataResult<List<DepoTransferDetayDto>>(ResultStatus.Success, dto);
        }
    }
}
