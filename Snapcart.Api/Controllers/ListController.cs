using Microsoft.AspNetCore.Mvc;
using Snapcart.Application.Dtos;
using Snapcart.Application.Interfaces;

namespace Snapcart.Api.Controllers;

[ApiController]
[Route("api/user/{userId:guid}/list")]
public class ListController(IProductService productService) : ControllerBase
{
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(Guid userId)
    {
        try
        {
            var products = await productService.GetProductsByUserAsync(userId);
            return Ok(products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Brand,
                p.Quantity,
                p.Price,
                p.Category,
                p.IsInCart
            }));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpGet("products/{productId:guid}")]
    public async Task<IActionResult> GetProduct(Guid userId, Guid productId)
    {
        var product = await productService.GetProductByIdAsync(productId);
        if (product is null)
            return NotFound(new { Error = "Product not found." });

        return Ok(new
        {
            product.Id,
            product.Name,
            product.Brand,
            product.Quantity,
            product.Price,
            product.Category,
            product.IsInCart
        });
    }

    [HttpPost("products")]
    public async Task<IActionResult> AddProduct(Guid userId, [FromBody] ProductDto productDto)
    {
        try
        {
            var product = await productService.AddProductAsync(userId, productDto);
            return CreatedAtAction(nameof(GetProduct), new { userId, productId = product.Id }, new
            {
                product.Id,
                product.Name,
                product.Brand,
                product.IsInCart
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpPut("products/{productId:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid userId, Guid productId, [FromBody] ProductDto productDto)
    {
        try
        {
            var product = await productService.UpdateProductAsync(productId, productDto);
            return Ok(new
            {
                product.Id,
                product.Name,
                product.Brand,
                product.Quantity,
                product.Price,
                product.Category,
                product.IsInCart
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpDelete("products/{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid userId, Guid productId)
    {
        try
        {
            await productService.DeleteProductAsync(productId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpPost("detect")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> DetectProduct(Guid userId, IFormFile image)
    {
        if (image is null || image.Length == 0)
            return BadRequest(new { Error = "No image provided." });

        try
        {
            using var stream = image.OpenReadStream();
            var product = await productService.DetectAndMarkProductAsync(userId, stream, image.FileName);

            if (product is null)
                return Ok(new { Matched = false, Message = "No matching product found in the list." });

            return Ok(new
            {
                Matched = true,
                Product = new
                {
                    product.Id,
                    product.Name,
                    product.Brand,
                    product.IsInCart
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompletePurchase(Guid userId, [FromBody] BuyDto buyDto)
    {
        try
        {
            var buy = await productService.CompletePurchaseAsync(userId, buyDto);
            return Ok(new
            {
                buy.Id,
                buy.UserId,
                buy.ListId,
                buy.PurchaseCompletedAt,
                buy.SupermarketName
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
