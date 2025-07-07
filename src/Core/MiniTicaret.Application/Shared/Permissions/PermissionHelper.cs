using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Shared.Permissions;

public static class PermissionHelper
{
    public static Dictionary<string, List<string>> GetAllPermissions()
    {
        var result = new Dictionary<string, List<string>>();

        var nestedTypes = typeof(Permissions)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var type in nestedTypes)
        {
            var allField = type.GetField("All", BindingFlags.Public | BindingFlags.Static);
            if (allField?.GetValue(null) is List<string> permissionList)
            {
                result.Add(type.Name, permissionList);
            }
        }

        return result;
    }

    public static List<string> GetAllPermissionList()
    {
        return GetAllPermissions()
            .SelectMany(kv => kv.Value)
            .Distinct() // təkrarlanmaların qarşısını alır
            .ToList();
    }
}
