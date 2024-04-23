using System.Security.Claims;
using EGeekApp.Helper;
using EGeekApp.Request;
using EGeekApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGeekapi.Controllers;

[Authorize]
[Route("v1/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductRequest productRequest)
    {
        var id = await _productService.Create(productRequest, UserHelper.GetEmail(User.Claims));
        return Ok(id);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductRequest productRequest)
    {
        await _productService.Update(id, productRequest, UserHelper.GetEmail(User.Claims));
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAll();
        return Ok(products);
    }
}