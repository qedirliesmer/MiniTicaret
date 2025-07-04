using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Shared.Permissions;

public class Permissions
{
    public static class Role
    {
        public const string Create = "Role.Create";
        public const string Delete = "Role.Delete";
        public const string View = "Role.View";
        public const string Update = "Role.Update";

        public static List<string> All = new()
        {
            Create, Delete, View, Update
        };
    }

    public static class Product
    {
        public const string Create = "Product.Create";
        public const string Delete = "Product.Delete";
        public const string View = "Product.View";
        public const string Update = "Product.Update";

        public static List<string> All = new()
        {
            Create, Delete, View, Update
        };
    }

    public static List<string> GetPermissionsByRole(string role)
    {
        return role switch
        {
            "Admin" => Role.All.Concat(Product.All).ToList(),
            "Seller" => new List<string> { Product.Create, Product.Update },
            "Buyer" => new List<string>(),
            _ => new List<string>()
        };
    }
}