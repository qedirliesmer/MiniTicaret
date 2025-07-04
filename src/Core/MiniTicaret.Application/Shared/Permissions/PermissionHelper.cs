using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Shared.Permissions;

public class PermissionHelper
{
    public static Dictionary<string, List<string>> GetAllPermissions()
    {
        var result = new Dictionary<string, List<string>>();

        Type[] nestedTypes = typeof(MiniTicaret.Application.Shared.Permissions.Permissions)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var moduleType in nestedTypes)
        {
            var allField = moduleType.GetField("All", BindingFlags.Public | BindingFlags.Static);

            if (allField is not null)
            {
                var permissions = allField.GetValue(null) as List<string>;

                if (permissions is not null)

                    result.Add(moduleType.Name, permissions);
            }
        }
        return result;
    }

    public static List<string> GetAllPermissionList()
    {
        return GetAllPermissions().SelectMany(x => x.Value).ToList();

    }
}
