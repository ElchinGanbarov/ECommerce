using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<string>
    {
        public virtual ICollection<Endpoint>? Endpoints { get; set; }
    }
}
