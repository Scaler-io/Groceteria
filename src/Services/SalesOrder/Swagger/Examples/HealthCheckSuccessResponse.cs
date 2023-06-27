using Groceteria.Shared.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger.Examples
{
    public class HealthCheckSuccessResponse : IExamplesProvider<object>
    {
        public object GetExamples()
        {
            return new
            {
                Status = "Healthy",
                CheckPassed = 20
            };
        }
    }
}
