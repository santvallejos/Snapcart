namespace Snapcart.Application.Interfaces;

public interface IDetectProductService
{
    Task<string> DetectProductAsync(Stream imageStream, string fileName);
}
