using ECommerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

    }
}
