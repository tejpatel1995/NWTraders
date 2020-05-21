using System.Data.Entity;

namespace NorthwindTraders.Models

{
    public class NWTradersContext : DbContext
    {
        public NWTradersContext() : base("name=NWTradersContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public void AddCategory(Category category)
        {
            this.Categories.Add(category);
            this.SaveChanges();
        }
        public void EditCategory(Category UpdatedCategory)
        {
            Category category = this.Categories.Find(UpdatedCategory.CategoryId);
            category.CategoryName = UpdatedCategory.CategoryName;
            category.Description = UpdatedCategory.Description;
            category.Products = UpdatedCategory.Products;
            this.SaveChanges();
        }
        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }
        public void EditProduct(Product UpdatedProduct)
        {
            Product product = this.Products.Find(UpdatedProduct.ProductID);
            product.ProductName = UpdatedProduct.ProductName;
            product.QuantityPerUnit = UpdatedProduct.QuantityPerUnit;
            product.ReorderLevel = UpdatedProduct.ReorderLevel;
            product.Supplier = UpdatedProduct.Supplier;
            product.SupplierID = UpdatedProduct.SupplierID;
            product.UnitPrice = UpdatedProduct.UnitPrice;
            product.UnitsOnOrder = UpdatedProduct.UnitsOnOrder;
            this.SaveChanges();
        }
    }
}
