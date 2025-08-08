using StokTakip.Entities.Dtos.LogTakipDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface ILogTakipService
    {
        Task<IResult> CreateAsync(LogTakipDto dto);
    }
}
