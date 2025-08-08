using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Dtos.StokDtos;
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
    public class StokService : IStokService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StokService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<List<StokDto>>> GetAllAsync()
        {
            var stoklar= await _unitOfWork.Stok.GetAllAsync(null,s=>s.Malzeme,s=>s.Depo,s=>s.cari);
            var stokDtoList = _mapper.Map<List<StokDto>>(stoklar);
            return new DataResult<List<StokDto>>(ResultStatus.Success, stokDtoList);
        }

        public async Task<IDataResult<List<StokDto>>> GetByDepoIdAsync(int depoId)
        {
            var stoklar=await _unitOfWork.Stok.GetAllAsync(x => x.DepoId == depoId, s => s.Malzeme, s => s.Depo, s => s.cari);
            var dtoList = _mapper.Map<List<StokDto>>(stoklar);
            return new DataResult<List<StokDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<StokDto>>> GetByMalzemeIdAsync(int malzemeId)
        {
            var stoklar= await _unitOfWork.Stok.GetAllAsync(x => x.MalzemeId == malzemeId, s => s.Malzeme, s => s.Depo, s => s.cari);
            var dtoList = _mapper.Map<List<StokDto>>(stoklar);
            return new DataResult<List<StokDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<decimal>> GetKalanStokAsync(int malzemeId, int depoId)
        {
            var stoklar =await _unitOfWork.Stok.GetAllAsync(x => x.MalzemeId == malzemeId && x.DepoId == depoId);
            decimal kalanStok = stoklar.Sum(s => s.Miktar);
            return new DataResult<decimal>(ResultStatus.Success, kalanStok);
        }
    }
}
