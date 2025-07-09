using Microsoft.AspNetCore.Identity;
using MiniTicaret.Application.Shared.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static MiniTicaret.Application.Shared.Permissions.Permissions;

namespace MiniTicaret.Persistence.Services;

public class DbInitializer
{
    public static async Task SeedPermissionsAsync(RoleManager<IdentityRole> roleManager)
    {
        // Rolların siyahısı
        var roles = new[] { "Admin", "Seller", "Buyer", "Moderator" };

        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var existingClaims = await roleManager.GetClaimsAsync(role);
            var permissionList = Permissions.GetPermissionsByRole(roleName);

            foreach (var permission in permissionList)
            {
                if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }

}
