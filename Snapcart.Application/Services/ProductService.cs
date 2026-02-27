using Snapcart.Application.Dtos;
using Snapcart.Application.Interfaces;
using Snapcart.Domain.Entities;
using Snapcart.Infrastructure.Interfaces;

namespace Snapcart.Application.Services;

public class ProductService(
    IProductRepository productRepository,
    IListRepository listRepository,
    IUserRepository userRepository,
    IBuyRepository buyRepository,
    IDetectProductService detectProductService) : IProductService
{
    public async Task<IEnumerable<Product>> GetProductsByUserAsync(Guid userId)
    {
        var list = await GetOrCreateActiveListAsync(userId);
        return await productRepository.GetProductsByListIdAsync(list.Id);
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await productRepository.GetProductByIdAsync(productId);
    }

    public async Task<Product> AddProductAsync(Guid userId, ProductDto productDto)
    {
        var list = await GetOrCreateActiveListAsync(userId);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Brand = productDto.Brand,
            ListId = list.Id
        };

        return await productRepository.AddProductAsync(product);
    }

    public async Task<Product> UpdateProductAsync(Guid productId, ProductDto productDto)
    {
        var product = await productRepository.GetProductByIdAsync(productId);
        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        product.Name = productDto.Name;
        product.Brand = productDto.Brand;
        product.Quantity = productDto.Quantity;
        product.Price = productDto.Price;
        product.Category = productDto.Category;

        return await productRepository.UpdateProductAsync(product);
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await productRepository.GetProductByIdAsync(productId);
        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        await productRepository.DeleteProductAsync(productId);
    }

    public async Task<Product?> DetectAndMarkProductAsync(Guid userId, Stream imageStream, string fileName)
    {
        var list = await GetOrCreateActiveListAsync(userId);
        var products = (await productRepository.GetProductsByListIdAsync(list.Id)).ToList();

        if (products.Count == 0)
            throw new InvalidOperationException("The shopping list is empty.");

        var productNames = products.Select(p => p.Name);
        var detectedName = await detectProductService.DetectProductAsync(imageStream, fileName, productNames);

        if (detectedName == "NO_MATCH")
            return null;

        var matched = products.FirstOrDefault(p =>
            p.Name.Equals(detectedName.Trim(), StringComparison.OrdinalIgnoreCase));

        if (matched is null)
            return null;

        matched.IsInCart = true;
        return await productRepository.UpdateProductAsync(matched);
    }

    public async Task<Buy> CompletePurchaseAsync(Guid userId, BuyDto buyDto)
    {
        var list = await listRepository.GetActiveListByUserIdAsync(userId);
        if (list is null)
            throw new InvalidOperationException("No active list found for this user.");

        list.IsActive = false;
        await listRepository.UpdateListAsync(list);

        var buy = new Buy
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ListId = list.Id,
            PurchaseCompletedAt = DateTime.UtcNow,
            SupermarketName = buyDto.SupermarketName
        };

        return await buyRepository.AddBuyAsync(buy);
    }

    private async Task<List> GetOrCreateActiveListAsync(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user is null)
            throw new KeyNotFoundException("User not found.");

        var list = await listRepository.GetActiveListByUserIdAsync(userId);
        if (list is not null)
            return list;

        var newList = new List
        {
            Id = Guid.NewGuid(),
            UserId = userId
        };

        return await listRepository.AddListAsync(newList);
    }
}
