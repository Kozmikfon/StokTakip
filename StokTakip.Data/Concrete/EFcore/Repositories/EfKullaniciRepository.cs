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
    public class EfKullaniciRepository : EfEntityRepositoryBase<Kullanici>,IKullaniciRepository
    {
        public EfKullaniciRepository(DbContext context) : base(context)
        {
        }
    }
}
