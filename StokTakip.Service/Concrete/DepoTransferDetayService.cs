using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Enums;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using StokTakip.Shared.Utilities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class DepoTransferDetayService : IDepoTransferDetayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepoTransferDetayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<DepoTransferDetayDto>> Create(DepoTransferDetayCreateDto dto)
        {
            var entity = _mapper.Map<DepoTransferDetay>(dto);
            entity.CreatedTime = DateTime.Now;
            entity.ModifiedTime = DateTime.Now;
            entity.IsActive = true;
            entity.IsDelete = false;
            entity.CreatedByName = "Admin";
            entity.ModifiedByName = "Admin";

            await _unitOfWork.DepoTransferDetay.AddAsync(entity);

            // Stok hareketi: Çıkış (kaynak depo)
            var transfer = await _unitOfWork.DepoTransfer.GetAsync(x => x.Id == dto.TransferId);
            if (transfer == null)
                return new DataResult<DepoTransferDetayDto>(ResultStatus.Error, "Transfer bilgisi bulunamadı.",null);


            await _unitOfWork.Stok.AddAsync(new StokTakip.Entities.Concrete.Stok
            {
                MalzemeId = dto.MalzemeId,
                DepoId = transfer.kaynakDepoId,
                HareketTarihi = DateTime.Now,
                Miktar = -dto.Miktar,
                HareketTipi = StokHareketTipi.TransferCikis,
                ReferansId = dto.TransferId,
                Aciklama = dto.Aciklama,
                SeriNo = dto.SeriNo,
                IsActive = true,
                IsDelete = false,
                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                CreatedByName = "Admin",
                ModifiedByName = "Admin"
            });

            // Stok hareketi: Giriş (hedef depo)
            await _unitOfWork.Stok.AddAsync(new StokTakip.Entities.Concrete.Stok
            {
                MalzemeId = dto.MalzemeId,
                DepoId = transfer.hedefDepoId,
                HareketTarihi = DateTime.Now,
                Miktar = dto.Miktar,
                HareketTipi = StokHareketTipi.TransferGiris,
                ReferansId = dto.TransferId,
                Aciklama = dto.Aciklama,
                SeriNo = dto.SeriNo,
                IsActive = true,
                IsDelete = false,
                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                CreatedByName = "Admin",
                ModifiedByName = "Admin"
            });

            await _unitOfWork.SaveAsync();

            var resultDto = _mapper.Map<DepoTransferDetayDto>(entity);
            return new DataResult<DepoTransferDetayDto>(ResultStatus.Success,"Depo transfer detay başarıyla oluşturuldu.",resultDto,null);

        }

        public async Task<IResult> Delete(int id)
        {
            var entity = await _unitOfWork.DepoTransferDetay.GetAsync(x => x.Id == id);
            if (entity == null)
                {
                return new Result(ResultStatus.Error, "Hata, istenilen depo transfer detayı bulunamadı.");
            }


            entity.IsDelete = true;
            entity.IsActive = false;
            entity.ModifiedTime = DateTime.Now;
            entity.ModifiedByName = "Admin";

            await _unitOfWork.DepoTransferDetay.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Depo transfer detayı başarıyla silindi.");


        }

        public async Task<IDataResult<DepoTransferDetayDto>> Get(int id)
        {
            var detay = await _unitOfWork.DepoTransferDetay.GetAsync(x => x.Id == id, d => d.malzeme);
            if (detay == null)
                {
                return new DataResult<DepoTransferDetayDto>(ResultStatus.Error, "Hata, istenilen depo transfer detayı bulunamadı.", null);
            }


            var dto = _mapper.Map<DepoTransferDetayDto>(detay);
            return new DataResult<DepoTransferDetayDto>(ResultStatus.Success, "Depo transfer detayı başarıyla getirildi.", dto);

        }

        public async Task<IDataResult<List<DepoTransferDetayDto>>> GetAllByNonDeleteAsync()
        {
            var detaylar = await _unitOfWork.DepoTransferDetay.GetAllAsync(x => !x.IsDelete, x => x.malzeme, x => x.depoTransfer);
            var dtoList= _mapper.Map<List<DepoTransferDetayDto>>(detaylar);
            return new DataResult<List<DepoTransferDetayDto>>(ResultStatus.Success, "Depo transfer detayları başarıyla getirildi.", dtoList);

        }

        public async Task<IDataResult<List<DepoTransferDetayDto>>> GetAllByTransferIdAsync(int transferId)
        {
            var detaylar= await _unitOfWork.DepoTransferDetay.GetAllAsync(x => x.transferId == transferId && !x.IsDelete, x => x.malzeme, x => x.depoTransfer);
            if (detaylar == null || !detaylar.Any())
            {
                return new DataResult<List<DepoTransferDetayDto>>(ResultStatus.Error, "Transfer detayları bulunamadı.", null);
            }
            var dtoList = _mapper.Map<List<DepoTransferDetayDto>>(detaylar);
            return new DataResult<List<DepoTransferDetayDto>>(ResultStatus.Success, "Transfer detayları başarıyla getirildi.", dtoList);
        }
    }
}
