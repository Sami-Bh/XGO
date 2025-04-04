using Microsoft.EntityFrameworkCore;
using XGO.Store.Domain;
using XGO.Store.Persistance.Models;

namespace XGO.Store.Persistance
{
    public static class DBInitializer
    {
        public static async Task SeedDataAsync(XGODbContext xGODbContext)
        {
            var categories = GenerateData();





            if (await xGODbContext.Categories.AnyAsync()) return;

            await xGODbContext.Categories.AddRangeAsync(categories);
            await xGODbContext.SaveChangesAsync();


        }

        private static List<Category> GenerateData()
        {
            var categories = new List<Category>
{
    new Category
    {
        Name = "Electronics",
        SubCategories = new List<SubCategory>
        {
            new SubCategory
            {
                Name = "Mobile Phones",
                Products = new List<Product>
                {
                    new Product { Name = "iPhone 15", IsProximity = false, IsHeavy = false, IsBulky = false },
                    new Product { Name = "Samsung Galaxy S23", IsProximity = false, IsHeavy = false, IsBulky = false },
                    new Product { Name = "Google Pixel 7", IsProximity = false, IsHeavy = false, IsBulky = false },
                    new Product { Name = "OnePlus 11", IsProximity = false, IsHeavy = false, IsBulky = false },
                    new Product { Name = "Xiaomi 13 Pro", IsProximity = false, IsHeavy = false, IsBulky = false }
                }
            }
        }
    },
    new Category
    {
        Name = "Furniture",
        SubCategories = new List<SubCategory>
        {
            new SubCategory
            {
                Name = "Sofas",
                Products = new List<Product>
                {
                    new Product { Name = "Leather Sofa", IsProximity = false, IsHeavy = true, IsBulky = true },
                    new Product { Name = "Fabric Sofa", IsProximity = false, IsHeavy = true, IsBulky = true },
                    new Product { Name = "Recliner Sofa", IsProximity = false, IsHeavy = true, IsBulky = true },
                    new Product { Name = "L-Shaped Sofa", IsProximity = false, IsHeavy = true, IsBulky = true },
                    new Product { Name = "Futon Sofa", IsProximity = false, IsHeavy = false, IsBulky = false }
                }
            }
        }
    },
    new Category { Name = "Toys", SubCategories = new List<SubCategory> { new SubCategory { Name = "Action Figures", Products = new List<Product> { new Product { Name = "Spider-Man Figure" }, new Product { Name = "Batman Figure" }, new Product { Name = "Superman Figure" }, new Product { Name = "Iron Man Figure" }, new Product { Name = "Captain America Figure" } } } } },
    new Category { Name = "Books", SubCategories = new List<SubCategory> { new SubCategory { Name = "Fiction", Products = new List<Product> { new Product { Name = "The Great Gatsby" }, new Product { Name = "1984" }, new Product { Name = "To Kill a Mockingbird" }, new Product { Name = "The Catcher in the Rye" }, new Product { Name = "Brave New World" } } } } },
    new Category { Name = "Appliances", SubCategories = new List<SubCategory> { new SubCategory { Name = "Kitchen Appliances", Products = new List<Product> { new Product { Name = "Microwave Oven" }, new Product { Name = "Refrigerator" }, new Product { Name = "Blender" }, new Product { Name = "Toaster" }, new Product { Name = "Dishwasher" } } } } },
    new Category { Name = "Automotive", SubCategories = new List<SubCategory> { new SubCategory { Name = "Car Accessories", Products = new List<Product> { new Product { Name = "Car Cover" }, new Product { Name = "Steering Wheel Cover" }, new Product { Name = "Car Floor Mats" }, new Product { Name = "Dashboard Camera" }, new Product { Name = "Car Air Freshener" } } } } },
    new Category { Name = "Clothing", SubCategories = new List<SubCategory> { new SubCategory { Name = "Men's Wear", Products = new List<Product> { new Product { Name = "T-Shirt" }, new Product { Name = "Jeans" }, new Product { Name = "Jacket" }, new Product { Name = "Sneakers" }, new Product { Name = "Watch" } } } } },
    new Category { Name = "Shoes", SubCategories = new List<SubCategory> { new SubCategory { Name = "Sports Shoes", Products = new List<Product> { new Product { Name = "Running Shoes" }, new Product { Name = "Basketball Shoes" }, new Product { Name = "Football Cleats" }, new Product { Name = "Tennis Shoes" }, new Product { Name = "Hiking Boots" } } } } },
    new Category { Name = "Beauty & Health", SubCategories = new List<SubCategory> { new SubCategory { Name = "Skincare", Products = new List<Product> { new Product { Name = "Moisturizer" }, new Product { Name = "Sunscreen" }, new Product { Name = "Face Wash" }, new Product { Name = "Serum" }, new Product { Name = "Eye Cream" } } } } },
    new Category { Name = "Sports", SubCategories = new List<SubCategory> { new SubCategory { Name = "Outdoor Equipment", Products = new List<Product> { new Product { Name = "Tent" }, new Product { Name = "Backpack" }, new Product { Name = "Sleeping Bag" }, new Product { Name = "Hiking Poles" }, new Product { Name = "Compass" } } } } },
    new Category { Name = "Home Decor", SubCategories = new List<SubCategory> { new SubCategory { Name = "Wall Art", Products = new List<Product> { new Product { Name = "Canvas Painting" }, new Product { Name = "Wall Clock" }, new Product { Name = "Sculpture" }, new Product { Name = "Mirror" }, new Product { Name = "Photo Frame" } } } } },
    new Category { Name = "Grocery", SubCategories = new List<SubCategory> { new SubCategory { Name = "Dairy", Products = new List<Product> { new Product { Name = "Milk" }, new Product { Name = "Cheese" }, new Product { Name = "Yogurt" }, new Product { Name = "Butter" }, new Product { Name = "Cream" } } } } },
    new Category { Name = "Pet Supplies", SubCategories = new List<SubCategory> { new SubCategory { Name = "Dog Food", Products = new List<Product> { new Product { Name = "Dry Kibble" }, new Product { Name = "Canned Food" }, new Product { Name = "Treats" }, new Product { Name = "Rawhide Bones" }, new Product { Name = "Dog Chews" } } } } },
    new Category { Name = "Music Instruments", SubCategories = new List<SubCategory> { new SubCategory { Name = "Guitars", Products = new List<Product> { new Product { Name = "Acoustic Guitar" }, new Product { Name = "Electric Guitar" }, new Product { Name = "Bass Guitar" }, new Product { Name = "Classical Guitar" }, new Product { Name = "Ukulele" } } } } }
};


            return categories;
        }
    }
}
