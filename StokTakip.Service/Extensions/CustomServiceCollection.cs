using Microsoft.Build.Logging;
using Microsoft.Extensions.DependencyInjection;
using StokTakip.Data.Abstract;
using StokTakip.Data.Concrete;
using StokTakip.Data.Concrete.EFcore.Contexts;
using StokTakip.Service.Abstract;
using StokTakip.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Extensions
{
    public static class CustomServiceCollection
    {
        public static IServiceCollection MyCustomService(this IServiceCollection services)
        {
            services.AddDbContext<StokContext>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IDepoService, DepoService>();
            services.AddScoped<IDepoTransferDetayService,DepoTransferDetayService>();
            services.AddScoped<IDepotransferService,DepoTransferService>();
            services.AddScoped<IMalzemeService, MalzemeService>();
            services.AddScoped<IStokService, StokService>();
            services.AddScoped<IIrsaliyeService, IrsaliyeService>();
            services.AddScoped<IIrsaliyeDetayService, IrsaliyeDetayService>();
            services.AddScoped<ICariService, CariService>();
            services.AddScoped<ILogTakipService, LogTakipService>();
            return services;

        }
    }
}
