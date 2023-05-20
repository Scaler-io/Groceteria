using Groceteria.Shared.Core;
using Microsoft.AspNetCore.Mvc;
using Swagger.Configurations;
using Swagger.Examples.Errors;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Groceteria.Shared.Extensions;

namespace Groceteria.SaleseOrder.Api.Controllers.v1
{
    [ApiVersion("1")]
    public class HealthCheckController : BaseApiController
    {     
        public HealthCheckController(Serilog.ILogger logger) 
            : base(logger)
        {
        }

        [HttpGet]
        [Route("healthCheck")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetHealthCheckResult", Summary = "Performs api endpoint healthcheck")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(object))]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult GetHealthCheckResult()
        {
            Logger.Here().MethodEnterd();

            var healthCheckResult = new
            {
                Status = "Healthy",
                CheckPassed = 20
            };
            var result = Result<object>.Success(healthCheckResult);

            Logger.Here().Information("Result recieved. {@result}", result);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
