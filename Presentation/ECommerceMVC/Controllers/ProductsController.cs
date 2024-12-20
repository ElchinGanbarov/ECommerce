﻿using ECommerce.Application.Features.Commands.Products.CreateProduct;
using ECommerce.Application.Features.Queries.GetAllProduct;
using ECommerceMVC.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace ECommerceMVC.Controllers
{
    public class ProductsController : BaseController
    {

        readonly IMediator _mediator;
        readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator,
                                 ILogger<ProductsController> logger )
        {
            _mediator = mediator;
            _logger = logger;
        }
  
        public async Task<IActionResult> Index()
        {
            CreateProductCommandRequest createProductCommandRequest = new CreateProductCommandRequest(Name:"Elchin",Stock:1,Price:50);
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);

            if (!response.Success) return BadRequest(response.Message); 

            return View();
        }
        public async Task<IActionResult> GetAllProduct()
        {
            GetAllProductQueryRequest createProductCommandRequest = new GetAllProductQueryRequest(Name:"Apple");
            GetAllProductQueryResponse response = await _mediator.Send(createProductCommandRequest);
            return View(response);
        }
    }
}
