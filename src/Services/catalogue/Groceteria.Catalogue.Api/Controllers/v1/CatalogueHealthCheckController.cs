using Groceteria.Catalogue.Api.Swagger;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Groceteria.Catalogue.Api.Controllers.v1
{
    [ApiVersion("1")]
    public class CatalogueHealthCheckController : BaseApi
    {
        public CatalogueHealthCheckController(Serilog.ILogger logger) 
            : base(logger)
        {
        }

        [HttpGet]
        [Route("healthCheck")]
        [SwaggerHeader("CorrelationId", "string", "", true)]
        [SwaggerOperation(OperationId = "GetHealthCheckResult", Summary = "Performs api endpoint healthcheck")]
        public async Task<IActionResult> GetHealthCheckResult()
        {
            Logger.Here().MethodEnterd();

            var healthCheckResult = new
            {
                Status = "Healthy",
                CheckPassed = 20
            };

            var result = Result<object>.Success(healthCheckResult);

            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
