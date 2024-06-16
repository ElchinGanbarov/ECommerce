using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionBaseAttribute : AuthorizeAttribute, IAuthorizationFilter, IFilterMetadata
    {
        private static readonly string[] EmptyArray = Array.Empty<string>();

        //
        // Summary:
        //     List of claim types in token
        private string[] _claimTypes = EmptyArray;

        private int[] _permissions;

        public int[] Permissions
        {
            get
            {
                return _permissions;
            }
            set
            {
                _permissions = value;
                _claimTypes = SplitString(value);
            }
        }

        public int? Action { get; set; }

        public PermissionBaseAttribute()
        {
        }

        public PermissionBaseAttribute(int[] permissions, int? action)
            : this()
        {
            Permissions = permissions;
            Action = action;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ClaimsPrincipal user = context.HttpContext.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated || _claimTypes.Length == 0)
            {
                return;
            }

            if (Action.HasValue)
            {
                string[] claimTypes = _claimTypes;
                foreach (string type in claimTypes)
                {
                    Claim claim = user.FindFirst(type);
                    if (claim != null && int.TryParse(claim.Value, out var result) && BitwiseHelper.HasFlag(result, Action.Value))
                    {
                        return;
                    }
                }
            }
            else
            {
                string[] claimTypes = _claimTypes;
                foreach (string type2 in claimTypes)
                {
                    if (user.FindFirst(type2) != null)
                    {
                        return;
                    }
                }
            }

            context.Result = new ForbidResult();
        }

        public static string[] SplitString(int[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
            {
                return EmptyArray;
            }

            return permissions.Select(delegate (int piece)
            {
                int num = piece;
                return "pid_" + num;
            }).ToArray();
        }
    }
}
