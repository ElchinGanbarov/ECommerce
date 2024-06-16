using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Enums
{
    public enum Permission
    {
        User = 100,
        Role = 200,

        Language = 1000,
        Country = 1100,
        City = 1200,
        ProjectType = 1300,
        Blog = 1400,
        Home = 1500,


        ProfileValidation = 2000,
        Project = 2100,
        Order = 2200,
        ProjectExit = 2300,
        CapitalGain = 2400,
        ShareSellRequest = 2500,
        ServiceFee = 2600,
        InvestmentProjectDistribution = 3000,

        ManagementAndMaintenanceProjectOperations = 4000,
        CapitalExpenseOperations = 4100,
        BulkOrder = 4200,

    }
}
