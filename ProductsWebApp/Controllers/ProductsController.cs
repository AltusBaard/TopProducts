using Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Order;
using Newtonsoft.Json;

namespace ProductsWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost(Name = "ProcessOrder")]
        public async Task<IActionResult> ProcessOrder([FromBody] OrderItem[] request)
        {
            _logger.LogInformation("Request received to process order: {body}", JsonConvert.SerializeObject(request));

            var result = await _productService.ProcessOrder(request).ConfigureAwait(false);

            return result.Match(
                res =>
                {
                    _logger.LogInformation("Processed successfully: {body}", JsonConvert.SerializeObject(res));
                    return (IActionResult)Ok(res);
                },
                () =>
                {
                    _logger.LogError("Unknown error occured during processing");
                    return NotFound();
                });
        }
    }
}
