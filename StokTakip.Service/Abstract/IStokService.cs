using StokTakip.Entities.Dtos.StokDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IStokService
    {
        Task<IDataResult<List<StokDto>>> GetAllAsync(); // tüm stok hareketleri
        Task<IDataResult<List<StokDto>>> GetByMalzemeIdAsync(int malzemeId); // belirli malzeme
        Task<IDataResult<List<StokDto>>> GetByDepoIdAsync(int depoId); // belirli depo
        Task<IDataResult<decimal>> GetKalanStokAsync(int malzemeId, int depoId); // kalan stok

    }
}
