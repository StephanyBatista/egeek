using System.Security.Claims;
using EGeekApp.Helper;
using EGeekApp.Request;
using EGeekApp.Response;
using EGeekDomain;
using EGeekinfra.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EGeekApp.Service;

public class ProductService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public ProductService(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<int> Create(ProductRequest request, string creatorEmail)
    {
        var creator = await _userManager.FindByEmailAsync(creatorEmail);
        if (creator == null) throw new ArgumentNullException("User not found");
        
        UserHelper.ThrowExceptionIfUserIsNotWorker(await _userManager.GetClaimsAsync(creator));
        
        if (string.IsNullOrEmpty(request.Name)) throw new ArgumentNullException("Name is required");
        if (string.IsNullOrEmpty(request.Description)) throw new ArgumentNullException("Description is required");
        if (request.Price <= 0) throw new ArgumentNullException("Price must be greater than 0");
        if (request.QuantityInStock <= 0) throw new ArgumentNullException("QuantityInStock must be greater than 0");
        if (request.WeightInGrams <= 0) throw new ArgumentNullException("WeightInGrams must be greater than 0");
        if (request.HeightInCentimeters <= 0) throw new ArgumentNullException("WeightInCentimeters must be greater than 0");
        if (request.WidthInCentimeters <= 0) throw new ArgumentNullException("WidthInCentimeters must be greater than 0");

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CreatedBy = creator,
            CreatedAt = DateTime.UtcNow,
            QuantityInStock = request.QuantityInStock,
            WeightInGrams = request.WeightInGrams,
            HeightInCentimeters = request.HeightInCentimeters,
            WidthInCentimeters = request.WidthInCentimeters
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }

    public async Task Update(int id, ProductRequest request, string updaterEmail)
    {
        var updater = await _userManager.FindByEmailAsync(updaterEmail);
        if (updater == null) throw new ArgumentNullException("User not found");
        UserHelper.ThrowExceptionIfUserIsNotWorker(await _userManager.GetClaimsAsync(updater));
        
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new ArgumentException("Product not found");

        if (!string.IsNullOrEmpty(request.Name))
            product.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Description))
            product.Description = request.Description;
        if (request.Price > 0)
            product.Price = request.Price;
        if (request.QuantityInStock > 0)
            product.QuantityInStock = request.QuantityInStock;
        if (request.WeightInGrams > 0)
            product.WeightInGrams = request.WeightInGrams;
        if (request.HeightInCentimeters > 0)
            product.HeightInCentimeters = request.HeightInCentimeters;
        if (request.WidthInCentimeters > 0)
            product.WidthInCentimeters = request.WidthInCentimeters;
        
        product.UpdatedBy = updater;
        product.UpdatedAt = DateTime.UtcNow;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductResponse>> GetAll()
    {
        var products = await _context.Products.Include(product => product.CreatedBy).ToListAsync();

        var productResponses = products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedBy.Email,
            QuantityInStock = p.QuantityInStock,
            WeightInGrams = p.WeightInGrams,
            HeightInCentimeters = p.HeightInCentimeters,
            WidthInCentimeters = p.WidthInCentimeters
        }).ToList();

        return productResponses;
    }
}