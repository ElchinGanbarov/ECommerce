﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ECommerceMVC.Utils;
using ECommerce.Domain.Errors;
using ECommerceMVC.Extensions;

namespace ECommerceMVC.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;
        private readonly Dictionary<ErrorType, Func<string?, IEnumerable<string>?, ObjectResult>> _errorHandlers;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
            _errorHandlers = new Dictionary<ErrorType, Func<string?, IEnumerable<string>?, ObjectResult>>
            {
                { ErrorType.Conflict, (details, errors) => ConflictResponse(details, errors) },
                { ErrorType.NotFound, (details, errors) => NotFoundResponse(details, errors) },
                { ErrorType.BadRequest, (details, errors) => BadRequestResponse(details, errors) },
                { ErrorType.Validation, (details, errors) => ValidationResponse(details, errors) },
                { ErrorType.Unexpected, (details, errors) => UnexpectedResponse(details, errors) }
            };
        }

        protected ObjectResult HandleError(IDomainError error)
        {
            if (_errorHandlers.TryGetValue(error.ErrorType, out var handler))
            {
                return handler(error.ErrorMessage, error.Errors);
            }

            throw new InvalidOperationException($"Unsupported error type: {error.ErrorMessage}");
        }

        protected ObjectResult NotFoundResponse(string? message = null, IEnumerable<string>? errors = null) =>
            NotFound(ProblemDetailsFactory.CreateNotFound(HttpContext, message, errors));

        protected ObjectResult BadRequestResponse(string? details = null, IEnumerable<string>? errors = null) =>
            BadRequest(ProblemDetailsFactory.CreateBadRequest(HttpContext, details, errors));

        protected ObjectResult ConflictResponse(string? details = null, IEnumerable<string>? errors = null) =>
            Conflict(ProblemDetailsFactory.CreateConflict(HttpContext, details, errors));

        protected ObjectResult ValidationResponse(string? details = null, IEnumerable<string>? errors = null) =>
            BadRequest(ProblemDetailsFactory.CreateValidation(HttpContext, details, errors));

        protected ObjectResult UnexpectedResponse(string? details = null, IEnumerable<string>? errors = null) =>
          BadRequest(ProblemDetailsFactory.CreateUnexpectedResponse(HttpContext, details, errors));
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string lang = context.RouteData.Values["lang"]?.ToString() ?? string.Empty;
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
