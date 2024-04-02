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

    public async Task Create(ProductRequest productRequest, string creatorEmail)
    {
        var creator = await _userManager.FindByEmailAsync(creatorEmail);
        if (creator == null) throw new ArgumentNullException("User not found");

        var product = new Product
        {
            Name = productRequest.Name,
            Description = productRequest.Description,
            CreatedBy = creator,
            CreatedAt = DateTime.Now
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ProductRequest productRequest, string updaterEmail)
    {
        var product = await _context.Products.FindAsync(productRequest.Id);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }

        product.Name = productRequest.Name;
        product.Description = productRequest.Description;
        
        var updater = await _userManager.FindByEmailAsync(updaterEmail);
        if (updater == null) throw new ArgumentNullException("User not found");
        product.UpdatedBy = updater;
        product.UpdatedAt = DateTime.Now;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductResponse>> GetAll()
    {
        var products = await _context.Products.ToListAsync();

        var productResponses = products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
        }).ToList();

        return productResponses;
    }
}