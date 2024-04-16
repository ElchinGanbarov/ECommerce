using ECommerce.Application.Features.Commands.Products.CreateProduct;
using ECommerce.Application.Features.Queries.GetAllProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class ProductComponent : ViewComponent
    {
       private readonly IMediator _mediator;

        public ProductComponent(IMediator mediator)
        {
            _mediator = mediator; 
        }

        public async Task<IViewComponentResult> InvokeAsync(string title)
        {
            GetAllProductQueryResponse response = await _mediator.Send(new GetAllProductQueryRequest(Name: "Apple"));

            ViewBag.Title = title;
            return View();
        }

    }
}
