using Microsoft.EntityFrameworkCore;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.ViewModels.StokDurumu;
using StokTakip.Shared.Data.Concrete.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Concrete.EFcore.Repositories
{
    public class EfStokRepository : EfEntityRepositoryBase<Stok>,IStokRepository       
    {
        private readonly DbContext _context;
        public EfStokRepository(DbContext context):base(context)=>_context = context;

        public async Task<decimal> GetKalanMiktarAsync(int depoId, int malzemeId)
        {
            var val = await _context.Set<StokDurumView>()
                .AsNoTracking()
                .Where(v => v.DepoId == depoId && v.MalzemeId == malzemeId)
                .Select(v => (decimal?)v.KalanMiktar)
                .FirstOrDefaultAsync();
            return val ?? 0m;
        }

        public async Task<Dictionary<int, decimal>> GetKalanMiktarlarAsync(int depoId, IEnumerable<int> malzemeIdList)
        {
            var ids = malzemeIdList?.Distinct().ToList() ?? new List<int>();
            if (ids.Count == 0) return new Dictionary<int, decimal>();

            var rows = await _context.Set<StokDurumView>()
                .AsNoTracking()
                .Where(v => v.DepoId == depoId && ids.Contains(v.MalzemeId))
                .Select(v => new { v.MalzemeId, v.KalanMiktar })
                .ToListAsync();

            var dict = ids.ToDictionary(id => id, _ => 0m);
            foreach (var r in rows) dict[r.MalzemeId] = r.KalanMiktar;
            return dict;
        }
    }
}
