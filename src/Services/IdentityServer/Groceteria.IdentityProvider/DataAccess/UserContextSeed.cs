﻿using Groceteria.Identity.Shared.Entities;
using Groceteria.Identity.Shared.Models;
using Groceteria.IdentityProvider.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Groceteria.IdentityProvider.DataAccess
{
    public class UserContextSeed
    {
        public static async Task SeedDefaultAdminAndRoleAsync(ILogger logger,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<AppRole>
                    {
                        new AppRole { Name = "Customer" },
                        new AppRole { Name = "StoreManager" },
                        new AppRole { Name = "BAU" },
                        new AppRole { Name = "SystemAdmin" },
                        new AppRole { Name = "SuperAdmin" }
                    };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                logger.Here().Information("Default roles seeded");
            }

            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    UserName = "Admin",
                    Email = "admin@groceteria.com",
                    Firstname = "Admin",
                    LastName = "N/A",
                    IsActive = true,
                    CreatedBy = "System Default",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedBy = null,
                    UpdatedAt = null,
                    LastLogin = null
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd");
                if (!result.Succeeded)
                {
                    logger.Here().Information("User seed failure");
                    return;
                }

                var createdUser = await userManager.FindByEmailAsync(user.Email);

                var selectedRoles = new List<string>{
                    UserRoles.SuperAdmin.ToString(),
                    UserRoles.SystemAdmin.ToString(),
                    UserRoles.StoreManager.ToString()
                };

                var roleResult = await userManager.AddToRolesAsync(createdUser, selectedRoles);
                if (roleResult.Succeeded)
                {
                    logger.Here().Information("Admin role add to user failure");
                }

                var claims = new List<Claim>
                {
                    new Claim("email", createdUser.Email),
                    new Claim("roles", JsonConvert.SerializeObject(selectedRoles)),
                    new Claim(JwtClaimTypes.PreferredUserName, createdUser.UserName),
                    new Claim(JwtClaimTypes.GivenName, createdUser.Firstname),
                    new Claim(JwtClaimTypes.FamilyName, createdUser.LastName),
                };

                await userManager.AddClaimsAsync(createdUser, claims);
            }
        }
    }
}
