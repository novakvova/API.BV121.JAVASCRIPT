﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Data.Entities
{
    [Table("tblProductImages")]
    public class ProductImageEntity : BaseEntity<int>
    {
        [Required, StringLength(200)]
        public string Name { get; set; }
        //послідовність слідування фото у товарі
        public short Priority { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
