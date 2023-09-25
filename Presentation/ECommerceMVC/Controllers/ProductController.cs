using ECommerce.Application.Features.Commands.Products.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Controllers
{
    public class ProductController : Controller
    {

        readonly IMediator _mediator;
        readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator,
                                 ILogger<ProductController> logger )
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            CreateProductCommandRequest createProductCommandRequest = new CreateProductCommandRequest
            {
                Name = "ASUS ROG STRIX",
                Price = 50,
                Stock = 5
            };

            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);

            return View();
        }
    }
}
