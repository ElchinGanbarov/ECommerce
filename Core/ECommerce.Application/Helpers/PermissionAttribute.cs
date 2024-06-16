using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : PermissionBaseAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public PermissionAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionAttribute(params Enums.Permission[] permissions) : base(permissions != null
                ? Array.ConvertAll(permissions, value => (int)value)
                : null, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="permissions"></param>
        public PermissionAttribute(Enums.Action action, params Enums.Permission[] permissions) : base(permissions != null
            ? Array.ConvertAll(permissions, value => (int)value)
            : null, (int?)action)
        {
        }
    }
}
