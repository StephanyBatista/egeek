using EGeekApp.Helper;
using EGeekApp.Request;
using EGeekApp.Response;
using EGeekDomain;
using EGeekInfra.Integration;
using EGeekinfra.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EGeekApp.Service;

public class OrderService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly PaymentClient _paymentClient;

    public OrderService(ApplicationDbContext context, UserManager<User> userManager, PaymentClient paymentClient)
    {
        _context = context;
        _userManager = userManager;
        _paymentClient = paymentClient;
    }
    
    public async Task<int> Create(List<OrderItemRequest> items, string creatorEmail)
    {
        var creator = await _userManager.FindByEmailAsync(creatorEmail);
        if (creator == null) throw new ArgumentNullException("User not found");
        if (items == null ||!items.Any()) throw new ArgumentNullException("Item is required");
        
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            CreatedBy = creator,
        };

        decimal totalAmount = 0;
        foreach (var item in items)
        {
            var product = _context.Products
                .FirstOrDefault(p => 
                    p.Id == item.ProductId && 
                    p.QuantityInStock >= item.Quantity);
            if (product == null)
                throw new ArgumentException("Out of stock");

            order.Items.Add(
                new OrderItem
                {
                    Product = product,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

            product.QuantityInStock -= item.Quantity; 
            _context.Products.Update(product);
            totalAmount += product.Price * item.Quantity;
        }

        order.TotalAmount = totalAmount;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order.Id;
    }
    
    public async Task<PaymentResponse> Payment(int id, OrderPaymentRequest request, string updaterEmail)
    {
        if (string.IsNullOrEmpty(request.CreditCardNumber) ||
            request.CreditCardNumber.Length != 16 ||
            !long.TryParse(request.CreditCardNumber, out _))
            throw new ArgumentException("Credit card is invalid");
        
        if (string.IsNullOrEmpty(request.CreditCardCode) ||
            request.CreditCardCode.Length != 3 ||
            !int.TryParse(request.CreditCardCode, out _))
            throw new ArgumentException("Credit card code is invalid");
        
        var order = _context.Orders.Include(o => o.CreatedBy).FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new ArgumentException("Order not found");
        if (order.Status != OrderStatus.Pending)
            throw new ArgumentException("Order must be pending to be paid");
        if(order.CreatedBy.Email != updaterEmail)
            throw new ArgumentException("You are not allowed to update this order");

        var result = await _paymentClient.Pay(request.CreditCardNumber, request.CreditCardCode);
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = order.CreatedBy;
        var response = new PaymentResponse();
        if (!result.Success)
        {
            order.Status = OrderStatus.PaymentNotAuthorized;
            response.Authorized = false;
            //TODO: missing return to stock
        }
        else
        {
            order.Address = request.Address;
            order.CreditCardMasked = result.CreditCardMasked;
            order.TransactionId = result.TransactionId;
            order.TransactionDate = result.TransactionDate;
            order.Status = OrderStatus.Paid;
            order.ShippingStatus = ShippingStatus.Preparing;
            order.ShippingDate = DateTime.UtcNow;
            response.Authorized = true;
        }
        
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return response;
    }
    
    public async Task UpdateShipping(int id, ShippingStatus status, string workerEmail)
    {
        var order = _context.Orders.Include(o => o.CreatedBy).FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new ArgumentException("Order not found");
        
        if(order.Status != OrderStatus.Paid)
            throw new ArgumentException("Order is not ready to be shipped");

        if (order.ShippingStatus == ShippingStatus.Delivered)
            throw new ArgumentException("Order is already delivered");
        
        if (order.ShippingStatus == ShippingStatus.Preparing && status == ShippingStatus.Delivered)
            throw new ArgumentException("Order was not sent yet");
        
        var user = await _userManager.FindByEmailAsync(workerEmail);
        if (user == null) throw new ArgumentNullException("User not found");
        UserHelper.ThrowExceptionIfUserIsNotWorker(await _userManager.GetClaimsAsync(user));

        order.ShippingStatus = status;
        order.ShippingDate = DateTime.UtcNow;
        
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
    
    public OrderResponse Get(int id, string buyerEmail)
    {
        var order = _context.Orders
            .Include(o => o.CreatedBy)
            .Include(order => order.Items)
            .ThenInclude(orderItem => orderItem.Product)
            .FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new ArgumentException("Order not found");
        
        if(order.CreatedBy.Email != buyerEmail)
            throw new ArgumentException("View order not allowed");

        return FillOrderResponse(order);
    }

    private static OrderResponse FillOrderResponse(Order order)
    {
        var orderResponse = new OrderResponse
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status.ToString(),
            Address = order.Address,
            TotalAmount = order.TotalAmount,
            ShippingStatus = order.ShippingStatus.ToString(),
            ShippingDate = order.ShippingDate,
            Items = order.Items.Select(i => new OrderItemResponse{
                ProductId = i.Product.Id,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        return orderResponse;
    }
}