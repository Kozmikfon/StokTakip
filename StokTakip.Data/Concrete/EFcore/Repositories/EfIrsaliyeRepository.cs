using Microsoft.EntityFrameworkCore;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Shared.Data.Concrete.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Concrete.EFcore.Repositories
{
    public class EfIrsaliyeRepository : EfEntityRepositoryBase<Irsaliye>,IIrsaliyeRepository
    {
        public EfIrsaliyeRepository(DbContext context) : base(context)
        {
        }
    }
}
