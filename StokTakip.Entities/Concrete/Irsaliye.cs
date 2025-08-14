using Microsoft.AspNetCore.Mvc.ModelBinding;
using StokTakip.Entities.Enums;
using StokTakip.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Concrete
{
    public class Irsaliye : EntityBase, IEntity
    {
        //[Key]
        //public int irsaliyeId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("İrsaliye Numarası:")]
        public string irsaliyeNo { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]

        [DisplayName("Kullanıcı Numarası:")]
        public int carId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("İrsaliye Tarihi:")]
        public DateTime irsaliyeTarihi { get; set; } = DateTime.Now;

        
        [DisplayName("toplam Tutar:")]
        public decimal? toplamTutar { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("İrsaliye Tipi:")]
        public Enums.IrsaliyeTipi irsaliyeTipi { get; set; }


        [DisplayName("Açıklama:")]
        public string? aciklama { get; set; }


        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Durum:")]
        public IrsaliyeDurumu durum { get; set; } = IrsaliyeDurumu.Taslak; // <- bool → enum


        [BindNever]
        [ForeignKey(nameof(carId))]
        public Cari? cari { get; set; }

        [Required]
        [DisplayName("Depo:")]
        public int depoId { get; set; }

        [ForeignKey(nameof(depoId))]
        public Depo? depo { get; set; }


        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Durum:")]
        


        public ICollection<IrsaliyeDetay> irsaliyeDetaylari { get; set; } = new List<IrsaliyeDetay>();
    }
}
