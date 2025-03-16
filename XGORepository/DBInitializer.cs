using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XGOModels;
using XGORepository.Models;

namespace XGORepository
{
    public static class DBInitializer
    {
        public static async Task SeedDataAsync(XGODbContext xGODbContext)
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
                    new Product
                    {
                        Name = "iPhone 15",
                        IsProximity = false,
                        IsHeavy = false,
                        IsBulky = false,
                        Pictures = new List<Picture>
                        {
                            new Picture { Description = "Front view of iPhone 15", Infos = "High resolution image" }
                        }
                    }
                }
            },
            new SubCategory
            {
                Name = "Laptops",
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "MacBook Pro",
                        IsProximity = false,
                        IsHeavy = true,
                        IsBulky = false,
                        Pictures = new List<Picture>
                        {
                            new Picture { Description = "Side view of MacBook Pro", Infos = "Slim design" }
                        }
                    }
                }
            }
        }
    },
    new Category { Name = "Furniture", SubCategories = new List<SubCategory> { new SubCategory { Name = "Sofas", Products = new List<Product> { new Product { Name = "Leather Sofa", IsProximity = false, IsHeavy = true, IsBulky = true, Pictures = new List<Picture> { new Picture { Description = "Leather texture close-up", Infos = "High-quality material" } } } } } } },
    new Category { Name = "Clothing", SubCategories = new List<SubCategory> { new SubCategory { Name = "T-Shirts", Products = new List<Product> { new Product { Name = "Cotton T-Shirt", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "T-Shirt with brand logo", Infos = "Comfort fit" } } } } } } },
    new Category { Name = "Books", SubCategories = new List<SubCategory> { new SubCategory { Name = "Fiction", Products = new List<Product> { new Product { Name = "The Great Gatsby", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Book cover of The Great Gatsby", Infos = "Classic literature" } } } } } } },
    new Category { Name = "Sports Equipment", SubCategories = new List<SubCategory> { new SubCategory { Name = "Fitness Gear", Products = new List<Product> { new Product { Name = "Dumbbells Set", IsProximity = false, IsHeavy = true, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Set of adjustable dumbbells", Infos = "Gym essentials" } } } } } } },
    new Category { Name = "Toys & Games", SubCategories = new List<SubCategory> { new SubCategory { Name = "Board Games", Products = new List<Product> { new Product { Name = "Chess Set", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Wooden chess set", Infos = "Strategy game" } } } } } } },
    new Category { Name = "Beauty & Personal Care", SubCategories = new List<SubCategory> { new SubCategory { Name = "Skincare", Products = new List<Product> { new Product { Name = "Face Moisturizer", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Hydrating face cream", Infos = "For all skin types" } } } } } } },
    new Category { Name = "Automotive", SubCategories = new List<SubCategory> { new SubCategory { Name = "Car Accessories", Products = new List<Product> { new Product { Name = "Car Phone Holder", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Magnetic car phone mount", Infos = "Easy installation" } } } } } } },
    new Category { Name = "Home Appliances", SubCategories = new List<SubCategory> { new SubCategory { Name = "Vacuum Cleaners", Products = new List<Product> { new Product { Name = "Robotic Vacuum", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Smart robotic vacuum", Infos = "Auto-cleaning mode" } } } } } } },
    new Category { Name = "Music Instruments", SubCategories = new List<SubCategory> { new SubCategory { Name = "Guitars", Products = new List<Product> { new Product { Name = "Acoustic Guitar", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Wooden acoustic guitar", Infos = "Classic sound" } } } } } } },
    new Category { Name = "Pet Supplies", SubCategories = new List<SubCategory> { new SubCategory { Name = "Dog Accessories", Products = new List<Product> { new Product { Name = "Dog Collar", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Adjustable dog collar", Infos = "Durable and comfortable" } } } } } } },
    new Category { Name = "Office Supplies", SubCategories = new List<SubCategory> { new SubCategory { Name = "Stationery", Products = new List<Product> { new Product { Name = "Notebook", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Lined notebook", Infos = "Great for note-taking" } } } } } } },
    new Category { Name = "Garden & Outdoor", SubCategories = new List<SubCategory> { new SubCategory { Name = "Gardening Tools", Products = new List<Product> { new Product { Name = "Garden Rake", IsProximity = false, IsHeavy = true, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Metal garden rake", Infos = "Essential for gardening" } } } } } } },
    new Category { Name = "Health & Wellness", SubCategories = new List<SubCategory> { new SubCategory { Name = "Supplements", Products = new List<Product> { new Product { Name = "Multivitamins", IsProximity = false, IsHeavy = false, IsBulky = false, Pictures = new List<Picture> { new Picture { Description = "Bottle of multivitamins", Infos = "Essential daily nutrition" } } } } } } }
};





            if (await xGODbContext.Categories.AnyAsync()) return;

            await xGODbContext.Categories.AddRangeAsync(categories);
            await xGODbContext.SaveChangesAsync();


        }

    }
}
