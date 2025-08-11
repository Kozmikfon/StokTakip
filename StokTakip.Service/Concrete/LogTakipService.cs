using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Data.Concrete;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.LogTakipDtos;
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
    public class LogTakipService : ILogTakipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LogTakipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(LogTakipDto dto)
        {
            try
            {
                var entity = _mapper.Map<LogTakip>(dto);
                entity.CreatedTime = DateTime.Now;
                entity.ModifiedTime = DateTime.Now;
                entity.IsActive = true;
                entity.IsDelete = false;
                entity.CreatedByName = dto.kullaniciAdi ?? "Sistem";
                entity.ModifiedByName = dto.kullaniciAdi ?? "Sistem";

                await _unitOfWork.LogTakip.AddAsync(entity);
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, "Log kaydı başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.Error, "Log kaydı sırasında bir hata oluştu.", ex);
            }
        }
    }
}
