using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StokTakip.Data.Abstract;
using StokTakip.Data.Concrete.EFcore.Contexts;
using StokTakip.Data.Concrete.EFcore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
       private readonly StokContext _context;
        private EfDepoRepository _efdepoRepository;
        private EfCariRepository _efcariRepository;
        private EfStokRepository _efstokRepository;
        private EfIrsaliyeRepository _efirsaliyeRepository;
        private EfDepoTransferDetayRepository _efdepoTransferDetayRepository;
        private EfIrsaliyeDetayRepository _efirsaliyeDetayRepository;
        private EfKullaniciRepository _efkullaniciRepository;
        private EfLogTakipRepository _eflogTakipRepository;
        private EfMalzemeRepository _efmalzemeRepository;
        private EfDepoTransferRepository _efdepoTransferRepository;


        public UnitOfWork(StokContext context)
        {
            _context = context;
            

        }
        // _unitOfWord.Depo.Add(entity);
        // _unitOfWork.SaveAsync();
        public IDepoRepository Depo => _efdepoRepository ??= new EfDepoRepository(_context);

        public ICariRepository Cari => _efcariRepository ??= new EfCariRepository(_context);

        public IStokRepository Stok => _efstokRepository ??= new EfStokRepository(_context);

        public IIrsaliyeRepository Irsaliye => _efirsaliyeRepository ??= new EfIrsaliyeRepository(_context);

        public IDepoTransferDetayRepository DepoTransferDetay => _efdepoTransferDetayRepository ??= new EfDepoTransferDetayRepository(_context);

        public IIrsaliyeDetayRepository IrsaliyeDetay => _efirsaliyeDetayRepository ??= new EfIrsaliyeDetayRepository(_context);

        public IKullaniciRepository Kullanici => _efkullaniciRepository ??= new EfKullaniciRepository(_context);

        public IStokRepository Stoklar => _efstokRepository ??= new EfStokRepository(_context);

        public ILogTakipRepository LogTakip => _eflogTakipRepository ??= new EfLogTakipRepository(_context);

        public IMalzemeRepository Malzeme => _efmalzemeRepository ??= new EfMalzemeRepository(_context);
        public IDepoTransferRepository DepoTransfer => _efdepoTransferRepository ??= new EfDepoTransferRepository(_context);

        public DbContext Context => _context;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        //  IDepoTransferRepository IUnitOfWork.DepoTransfer => throw new NotImplementedException();

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();//veritabanı bağlantısını kapatılır
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
