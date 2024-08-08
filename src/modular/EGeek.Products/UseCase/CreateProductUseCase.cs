using System.Security.Claims;
using EGeek.Common.Help;
using EGeek.Contracts.Id;
using EGeek.Products.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Products.UseCase;

internal static class CreateProductUseCase
{
    [Authorize]
    public static async Task<Created<int>> Action(
        ClaimsPrincipal principal, 
        CreateProductRequest request, 
        IProductRepository productRepository,
        IMediator mediator)
    {
        var query = new GetUserQuery(principal.GetEmail());
        var result = await mediator.Send(query);
        
        BadException.ThrowIfWithMessage(!result.IsWorker, "Only workers can create products");
        
        var product = new Product(request, principal.GetEmail());
        await productRepository.Create(product);
        
        return TypedResults.Created($"/v1/products/{product.Id}", product.Id);
    }
}