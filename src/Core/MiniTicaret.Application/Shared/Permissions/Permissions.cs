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
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
        public const string GetAllPermission = "Role.GetAllPermission";

        public static List<string> All = new()
        {
            Create,
            GetAllPermission,
            Update,
            Delete
        };
    }

    public static class Category
    {
        public const string MainCreate = "Category.MainCreate";
        public const string SubCreate = "Category.SubCreate";
        public const string GetAll = "Category.GetAll";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";

        public static List<string> All = new()
        {
            MainCreate,
            SubCreate,
            Update,
            Delete,
            GetAll
        };
    }
    public static class Product
    {
        public const string Create = "Product.Create";
        public const string Update = "Product.Update";
        public const string Delete = "Product.Delete";
        public const string GetMy = "Product.GetMy";
        public const string GetAll = "Product.GetAll";
        public const string DeleteProductImage = "Product.DeleteProductImage";
        public const string AddProductImage = "Product.AddProductImage";
        public const string AddProductFavourite = "Product.AddFavourite";
        public const string GetAllFavourite = "Product.GetAllFavourite";
        public const string DeleteFavourite = "Product.DeleteFavourite";

        public static List<string> All = new()
    {
        Create,
        Update,
        Delete,
        GetMy,
        DeleteProductImage,
        AddProductImage,
        AddProductFavourite,
        GetAllFavourite,
        DeleteFavourite,
        GetAll

    };
    }

    public static class Order
    {
        public const string Create = "Order.Create";
        public const string GetAll = "Order.GetAll";
        public const string Update = "Order.Update";
        public const string Delete = "Order.Delete";
        public const string GetMy = "Order.GetMy";
        public const string GetDetail = "Order.GetDetail";
        public const string GetMySales = "Order.GetMySales";

        public static List<string> All = new()
        {
            Create,
            GetAll,
            Update,
            Delete,
            GetMy,
            GetMySales,
            GetDetail
        };
    }

    public static class User
    {
        public const string GetAll = "User.GetAll";
        public const string GetById = "User.GetById";
        public static List<string> All = new()
    {
        GetAll,
        GetById
    };
    }
    public static class Review
    {
        public const string Create = "Review.Create";
        public const string Delete = "Review.Delete";
        public const string Update = "Review.Update";
        public const string Get = "Review.Get";

        public static List<string> All = new()
        {
            Create,
            Delete,
            Update,
            Get
        };
    }

   
    public static List<string> GetPermissionsByRole(string role)
    {
        return role switch
        {
            "Admin" => new List<string>
            {
                Role.Create,
                Role.Update,
                Role.Delete,
                Role.GetAllPermission,
                Category.MainCreate,
                Category.SubCreate,
                Category.GetAll,
                Category.Update,
                Category.Delete,
                Product.Create,
                Product.Update,
                Product.Delete,
                Product.GetMy,
                Product.GetAll,
                Product.DeleteProductImage,
                Product.AddProductImage,
                Product.AddProductFavourite,
                Product.GetAllFavourite,
                Product.DeleteFavourite,
                Order.Create,
                Order.GetAll,
                Order.Update,
                Order.Delete,
                Order.GetMy,
                Order.GetDetail,
                Order.GetMySales,
                User.GetAll,
                User.GetById,
                Review.Create,
                Review.Delete,
                Review.Update,
                Review.Get,
                Account.AddRole,
                Account.Create,
                Account.UpdateProfile,
                Account.ChangePassword
            },

            "Seller" => new List<string>
            {
                Product.Create,
                Product.Update,
                Product.Delete,
                Product.GetMy,
                Product.AddProductImage,
                Product.DeleteProductImage,
                Product.AddProductFavourite,
                Order.Create,
                Order.GetMySales,
                Order.Update,
                Order.Delete,
                Order.GetDetail,
                Order.GetAll,
                Order.GetMy,
                Review.Create,
                Review.Delete,
                Account.UpdateProfile,
                Account.ChangePassword
            },



            "Buyer" => new List<string>
            {
                 Order.Create,
                 Order.GetMy,
                 Order.GetAll,
                 Order.GetDetail,
                 Product.AddProductFavourite,
                 Product.DeleteFavourite,
                 Product.GetAllFavourite,
                 Review.Create,
                 Review.Delete,
                 Account.UpdateProfile,
                 Account.ChangePassword

            },

            "Moderator" => new List<string>
            {
                Category.MainCreate,
                Category.SubCreate,
                Category.GetAll,
                Category.Update,
                Category.Delete,
                Product.Create,
                Product.Update,
                Product.Delete,
                Product.GetMy,
                Product.DeleteProductImage,
                Product.AddProductImage,
                Product.AddProductFavourite,
                Order.Create,
                Order.GetMySales,
                Order.Update,
                Order.Delete,
                Order.GetDetail,
                Order.GetMy,
                Review.Create,
                Review.Delete,
                User.GetAll,
                User.GetById,
                Account.UpdateProfile,
                Account.ChangePassword
            },

            _ => new List<string>()
        };
  
       
    }
    public static class Account
    {
        public const string AddRole = "Account.AddRole";
        public const string Create = "Account.Create";
        public const string UpdateProfile = "Account.UpdateProfile";
        public const string ChangePassword = "Account.ChangePassword";

        public static List<string> All = new()
    {
        UpdateProfile,
        ChangePassword,
        AddRole,
        Create

    };
    }

}