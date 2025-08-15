using StokTakip.Entities.Concrete;
using StokTakip.Shared.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Abstract
{
    public interface IStokRepository : IEntityRepository<Stok>
    {
        Task<decimal> GetKalanMiktarAsync(int depoId, int malzemeId);
        Task<Dictionary<int, decimal>> GetKalanMiktarlarAsync(int depoId, IEnumerable<int> malzemeIdList);
    }
}
