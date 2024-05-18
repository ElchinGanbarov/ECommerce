namespace ECommerceMVC.MIddlewares
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("lang"))
                return false;

            var culture = values["lang"]?.ToString();
            return new List<string> { "en", "az", "ar" }.Contains(culture ?? string.Empty);
        }
    }
}
