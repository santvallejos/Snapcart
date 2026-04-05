using Microsoft.AspNetCore.Mvc;
using Snapcart.Application.Interfaces;

namespace Snapcart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DetectProductController(IDetectProductService detectProductService) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> DetectProduct(IFormFile image)
    {
        if (image is null || image.Length == 0)
            return BadRequest(new { Error = "No image provided." });

        using var stream = image.OpenReadStream();
        var product = await detectProductService.DetectProductAsync(stream, image.FileName);

        return Ok(new { Product = product });
    }
}
