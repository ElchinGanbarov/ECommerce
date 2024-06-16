using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Enums
{
    /// <summary>
    /// CRUD
    /// </summary>
    [Flags]
    public enum Action
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Create
        /// </summary>
        Create = 1,

        /// <summary>
        /// Read
        /// </summary>
        Read = 2,

        /// <summary>
        /// 
        /// </summary>
        All = Create | Read
    }
}
