using System.Security.Claims;
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
        if(User.Claims.Any(c => c.Type == "Worker" && c.Value == "True"))
            return Unauthorized();
        
        var creatorEmail = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        await _productService.Create(productRequest, creatorEmail);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductRequest productRequest)
    {
        if(User.Claims.Any(c => c.Type == "Worker" && c.Value == "True"))
            return Unauthorized();

        var updaterEmail = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        await _productService.Update(productRequest, updaterEmail);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if(User.Claims.Any(c => c.Type == "Worker" && c.Value == "True"))
            return Unauthorized();
        
        var products = await _productService.GetAll();
        return Ok(products);
    }
}