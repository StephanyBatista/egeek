using EGeekApp.Helper;
using EGeekApp.Request;
using EGeekApp.Service;
using EGeekDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGeekapi.Controllers;

[Authorize]
[Route("v1/orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] List<OrderItemRequest> request)
    {
        var id = await _orderService.Create(request, UserHelper.GetEmail(User.Claims));
        return Ok(id);
    }
    
    [HttpGet("{id}")]
    public IActionResult Payment([FromRoute] int id)
    {
        var response = _orderService.Get(id, UserHelper.GetEmail(User.Claims));
        return Ok(response);
    }

    [HttpPost("{id}/payment")]
    public async Task<IActionResult> Payment([FromRoute] int id, [FromBody] OrderPaymentRequest request)
    {
        var response = await _orderService.Payment(id, request, UserHelper.GetEmail(User.Claims));
        return Ok(response);
    }
    
    [HttpPut("{id}/shipping/sent")]
    public async Task<IActionResult> UpdateShippingToSent([FromRoute] int id)
    {
        await _orderService.UpdateShipping(id, ShippingStatus.Sent, UserHelper.GetEmail(User.Claims));
        return Ok();
    }
    
    [HttpPut("{id}/shipping/delivered")]
    public async Task<IActionResult> UpdateShippingToDelivered([FromRoute] int id)
    {
        await _orderService.UpdateShipping(id, ShippingStatus.Delivered, UserHelper.GetEmail(User.Claims));
        return Ok();
    }
}