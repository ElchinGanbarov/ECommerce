using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ECommerceMVC.Utils;

namespace ECommerceMVC.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string lang = context.RouteData.Values["lang"]?.ToString() ?? string.Empty;
            ViewData["LanguageCode"] = lang;

            var cultureName = CultureHelper.TryGetCulture(lang);


            var culture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            base.OnActionExecuting(context);
        }
    }
}
