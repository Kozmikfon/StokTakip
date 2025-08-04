using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IDepoRepository Depo { get; }
        ICariRepository Cari { get; }
        IStokRepository Stok { get; }
        IIrsaliyeRepository Irsaliye { get; }

        IDepoTransferDetayRepository DepoTransferDetay { get; }
        IIrsaliyeDetayRepository IrsaliyeDetay { get; }
        IKullaniciRepository Kullanici { get; } 
        IStokRepository Stoklar { get; }
        ILogTakipRepository LogTakip { get; }
        IMalzemeRepository Malzeme { get; }

        Task<int> SaveAsync();





    }
}
