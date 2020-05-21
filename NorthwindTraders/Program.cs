using System;
using NorthwindTraders.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLog;
using NLog.Config;
using System.Security.Cryptography;
using System.Data.Entity;

namespace NorthwindTraders
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string answer;
                do
                {

                    Console.WriteLine("Select action: \n1. Display Categories\n2. Add Category\n3. Edit Category\n4. Add Product\n5. Edit Product\n6. Display all Products\n7. Display a Specific Product\n8. Display Category with Products\n9. Display all Catgeories with all Products\nPress any other key to Quit");
                    answer = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"{answer} chosen");
                    if (answer == "1")
                    {
                        var db = new NWTradersContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine($"Categories:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}: {item.Description}");
                        }
                    }
                    if (answer == "2")
                    {
                        var db = new NWTradersContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);
                        Console.Write("Enter a name for a new Category: ");
                        var name = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        var descprtion = Console.ReadLine();
                        bool valid = true;
                        foreach (var item in query)
                        {
                            if (name.Equals(item.CategoryName))
                            {
                                valid = false;
                            }
                        }
                        if (name.Equals("") || valid == false)
                        {
                            logger.Error($"INVALID: {name} is already category name or is null.");
                            Console.WriteLine($"INVALID: {name} is an invalid category name or is null.");
                        }
                        else
                        {
                            var category = new Category { CategoryName = name, Description = descprtion };
                            db.AddCategory(category);
                            logger.Info($"Category added - {name}");
                        }
                    }
                    if (answer == "3")
                    {
                        Console.WriteLine("Choose the cateogry to edit:");
                        var db = new NWTradersContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine($"Categories:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}. {item.CategoryName}: {item.Description}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int CategoryID))
                        {
                            Category category = db.Categories.FirstOrDefault(p => p.CategoryId == CategoryID);
                            if (category != null)
                            {
                                Console.WriteLine("Enter new Category Name: ");
                                var name = Console.ReadLine();
                                Console.WriteLine("Enter new Category Description: ");
                                var description = Console.ReadLine();
                                var UpdatedCategory = new Category { CategoryId = CategoryID, CategoryName = name, Description = description };
                                db.EditCategory(UpdatedCategory);
                            }
                            else
                            {
                                logger.Error("Invalid Category Id");
                                continue;
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Category Id");
                            continue;
                        }
                    }
                    if (answer == "4")
                    {
                        Product product = new Product();
                        Console.WriteLine("Product Name: ");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Quantity Per Unit: ");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Unit Price (exclude dollar sign): ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal UnitPrice))
                        {
                            product.UnitPrice = UnitPrice;
                        }
                        else
                        {
                            logger.Error("Invalid Unit Price");
                            continue;
                        }
                        Console.WriteLine("Units In Stock: ");
                        if (short.TryParse(Console.ReadLine(), out short UnitsInStock))
                        {
                            product.UnitsInStock = UnitsInStock;
                        }
                        else
                        {
                            logger.Error("Invalid UnitsInStock Entry");
                            continue;
                        }
                        Console.WriteLine("Reorder Level: ");
                        if (short.TryParse(Console.ReadLine(), out short ReorderLevel))
                        {
                            product.ReorderLevel = ReorderLevel;
                        }
                        else
                        {
                            logger.Error("Invalid ReorderLevel Entry");
                            continue;
                        }
                        Console.WriteLine("Discontinued? (y/n): ");
                        string disc = Console.ReadLine();
                        if (disc.ToLower() == "y")
                        {
                            product.Discontinued = true;
                        }
                        else if (disc.ToLower() == "n")
                        {
                            product.Discontinued = false;
                        }
                        else
                        {
                            logger.Error("Invalid Discontinued? Entry");
                            continue;
                        }
                        var db = new NWTradersContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine($"Categories:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}. {item.CategoryName}: {item.Description}");
                        }
                        Console.WriteLine("Category ID: ");
                        if (int.TryParse(Console.ReadLine(), out int CategoryID))
                        {
                            Category category = db.Categories.FirstOrDefault(p => p.CategoryId == CategoryID);
                            if (category != null)
                            {
                                product.CategoryID = CategoryID;
                            }
                            else
                            {
                                logger.Error("Invalid Category Id");
                                continue;
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Category Id");
                            continue;
                        }

                        var query2 = db.Suppliers.OrderBy(p => p.SupplierID);

                        Console.WriteLine($"Suppliers:");
                        foreach (var item in query2)
                        {
                            Console.WriteLine($"{item.SupplierID}. {item.CompanyName}");
                        }
                        Console.WriteLine("Supplier ID: ");
                        if (int.TryParse(Console.ReadLine(), out int SupplierID))
                        {
                            Supplier supplier = db.Suppliers.FirstOrDefault(p => p.SupplierID == SupplierID);
                            if (supplier != null)
                            {
                                product.SupplierID = SupplierID;
                                product.Supplier = supplier;
                            }
                            else
                            {
                                logger.Error("Invalid Supplier Id");
                                continue;
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Supplier Id");
                            continue;
                        }
                        db.AddProduct(product);
                    }
                    if (answer == "5")
                    {
                        var db = new NWTradersContext();
                        Console.WriteLine("Which category is your product in?");
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine($"Categories:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}. {item.CategoryName}: {item.Description}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int CategoryID))
                        {
                            Category category = query.FirstOrDefault(p => p.CategoryId == CategoryID);
                            if (category != null)
                            {
                                var query2 = db.Products.OrderBy(p => p.ProductName).Where(p => p.CategoryID == CategoryID);
                                foreach (var item in query2)
                                {
                                    Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                                }

                                Console.WriteLine("Product ID: ");
                                if (int.TryParse(Console.ReadLine(), out int ProductID))
                                {
                                    Product product = db.Products.FirstOrDefault(p => p.ProductID == ProductID);
                                    if (product != null)
                                    {
                                        Console.WriteLine("Product Name: ");
                                        product.ProductName = Console.ReadLine();
                                        Console.WriteLine("Quantity Per Unit: ");
                                        product.QuantityPerUnit = Console.ReadLine();
                                        Console.WriteLine("Unit Price (exclude dollar sign): ");
                                        if (decimal.TryParse(Console.ReadLine(), out decimal UnitPrice))
                                        {
                                            product.UnitPrice = UnitPrice;
                                        }
                                        else
                                        {
                                            logger.Error("Invalid Unit Price");
                                            continue;
                                        }
                                        Console.WriteLine("Units In Stock: ");
                                        if (short.TryParse(Console.ReadLine(), out short UnitsInStock))
                                        {
                                            product.UnitsInStock = UnitsInStock;
                                        }
                                        else
                                        {
                                            logger.Error("Invalid UnitsInStock Entry");
                                            continue;
                                        }
                                        Console.WriteLine("Reorder Level: ");
                                        if (short.TryParse(Console.ReadLine(), out short ReorderLevel))
                                        {
                                            product.ReorderLevel = ReorderLevel;
                                        }
                                        else
                                        {
                                            logger.Error("Invalid ReorderLevel Entry");
                                            continue;
                                        }
                                        Console.WriteLine("Discontinued? (y/n): ");
                                        string disc = Console.ReadLine();
                                        if (disc.ToLower() == "y")
                                        {
                                            product.Discontinued = true;
                                        }
                                        else if (disc.ToLower() == "n")
                                        {
                                            product.Discontinued = false;
                                        }
                                        else
                                        {
                                            logger.Error("Invalid Discontinued? Entry");
                                            continue;
                                        }
                                        var query3 = db.Categories.OrderBy(p => p.CategoryId);

                                        Console.WriteLine($"Categories:");
                                        foreach (var item in query3)
                                        {
                                            Console.WriteLine($"{item.CategoryId}. {item.CategoryName}: {item.Description}");
                                        }
                                        Console.WriteLine("Category ID: ");
                                        if (int.TryParse(Console.ReadLine(), out int CategoryID2))
                                        {
                                            Category category2 = db.Categories.FirstOrDefault(p => p.CategoryId == CategoryID2);
                                            if (category2 != null)
                                            {
                                                product.CategoryID = CategoryID2;
                                            }
                                            else
                                            {
                                                logger.Error("Invalid Category Id");
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            logger.Error("Invalid Category Id");
                                            continue;
                                        }

                                        var query4 = db.Suppliers.OrderBy(p => p.SupplierID);

                                        Console.WriteLine($"Suppliers:");
                                        foreach (var item in query4)
                                        {
                                            Console.WriteLine($"{item.SupplierID}. {item.CompanyName}");
                                        }
                                        Console.WriteLine("Supplier ID: ");
                                        if (int.TryParse(Console.ReadLine(), out int SupplierID))
                                        {
                                            Supplier supplier = db.Suppliers.FirstOrDefault(p => p.SupplierID == SupplierID);
                                            if (supplier != null)
                                            {
                                                product.SupplierID = SupplierID;
                                                product.Supplier = supplier;
                                            }
                                            else
                                            {
                                                logger.Error("Invalid Supplier Id");
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            logger.Error("Invalid Supplier Id");
                                            continue;
                                        }
                                        db.EditProduct(product);
                                    }
                                }
                            }
                            else
                            {
                                logger.Error("Invalid Category Id");
                                continue;
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Category Id");
                            continue;
                        }
                    }
                    if (answer == "6")
                    {
                        var db = new NWTradersContext();
                        Console.WriteLine("What would you like to see?\n1. All Products\n2. Discontinued Products\n3. Active Products");
                        string productType = Console.ReadLine();
                        if (productType == "1")
                        {
                            var query = db.Products.OrderBy(p => p.ProductID);
                            Console.WriteLine("All Products: ");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                            }
                        }
                        else if (productType == "2")
                        {
                            var query = db.Products.Where(p => p.Discontinued == true).OrderBy(p => p.ProductID);
                            Console.WriteLine("Discontinued Products: ");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                            }
                        }
                        else if (productType == "3")
                        {
                            Console.WriteLine("Active Products: ");
                            var query = db.Products.Where(p => p.Discontinued == false).OrderBy(p => p.ProductID);
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Product Type Entry");
                        }

                    }
                    if (answer == "7")
                    {
                        var db = new NWTradersContext();
                        var query = db.Products.OrderBy(p => p.ProductID);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                        }
                        Console.WriteLine("Which Product would you like to view?");
                        if (int.TryParse(Console.ReadLine(), out int ProductID))
                        {
                            var query2 = db.Products.FirstOrDefault(p => p.ProductID == ProductID);
                            Console.WriteLine($"Product ID: {query2.ProductID}");
                            Console.WriteLine($"Product Name: {query2.ProductName}");
                            Console.WriteLine($"Quantity Per Unit: {query2.QuantityPerUnit}");
                            Console.WriteLine($"Reorder Level: {query2.ReorderLevel}");
                            var query3 = db.Suppliers.FirstOrDefault(s => s.SupplierID == query2.SupplierID);
                            Console.WriteLine($"Supplier: {query3.CompanyName}");
                            Console.WriteLine($"Supplier ID: {query2.SupplierID}");
                            Console.WriteLine($"Unit Price: ${query2.UnitPrice}");
                            Console.WriteLine($"Units In Stock: {query2.UnitsInStock}");
                            Console.WriteLine($"Units On Order: {query2.UnitsOnOrder}");
                        }
                        else
                        {
                            logger.Error("Invalid Product Id");
                        }
                    }
                    if (answer == "8")
                    {
                        var db = new NWTradersContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId).Include(p => p.Products);

                        Console.WriteLine("Which category is your product in?");
                        Console.WriteLine($"Categories:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}. {item.CategoryName}: {item.Description}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int CategoryID))
                        {
                            Console.WriteLine(db.Categories.FirstOrDefault(p => p.CategoryId == CategoryID).CategoryName);
                            var query2 = db.Products.Where(p => p.CategoryID == CategoryID);
                            foreach (var item2 in query2)
                            {
                                Console.WriteLine($"{item2.ProductID}. {item2.ProductName}");
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Category Id");
                            continue;
                        }
                    }
                    if (answer == "9")
                    {
                        var db = new NWTradersContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId).Include(p => p.Products);

                        Console.WriteLine($"Categories:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}: {item.Description}");
                            foreach (var item2 in item.Products)
                            {
                                Console.WriteLine($"{item2.ProductID}. {item2.ProductName}");
                            }
                            Console.WriteLine("");
                        }
                    }
                    Console.WriteLine("Press Any Key to Continue.");
                    Console.ReadLine();
                    Console.Clear();
                } while (answer == "1" || answer == "2" || answer == "3" || answer == "4" || answer == "5" || answer == "6" || answer == "7" || answer == "8" || answer == "9");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
