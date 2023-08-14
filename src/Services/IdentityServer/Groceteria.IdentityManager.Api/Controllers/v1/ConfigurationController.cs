using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.IdentityManager.Api.Controllers.v1
{
    [ApiVersion("1")]  
    public class ConfigurationController : BaseApiController
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public ConfigurationController(ILogger logger, ConfigurationDbContext configurationDbContext)
            : base(logger)
        {
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet("clients")]
        [Authorize]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _configurationDbContext.Clients.ToListAsync();
            //Logger.Information("clients fetched {@clients});
            return Ok(clients);
        }
    }
}
