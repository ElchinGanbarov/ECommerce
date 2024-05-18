using ECommerce.Application.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Const
{
    public static class Config
    {
        public static MailOptions Mail { get; set; } = new MailOptions();
    }
}
