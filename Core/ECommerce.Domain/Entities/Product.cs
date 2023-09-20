using ECommerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public required int Stock { get; set; }
        public required float Price { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //public ICollection<Order> Orders { get; set; }
        //public ICollection<ProductImageFile>? ProductImageFiles { get; set; }
        public virtual ICollection<BasketItem>? BasketItems { get; set; }
    }
}
