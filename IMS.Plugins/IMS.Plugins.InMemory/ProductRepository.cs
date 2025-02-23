using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product() { ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150},
                new Product() { ProductId = 2, ProductName = "Car", Quantity = 10, Price = 25000},
            };
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _products.Max(x => x.ProductId);

            product.ProductId = maxId + 1;

            _products.Add(product);

            return Task.CompletedTask;
        }

        public Task DeleteProductByIdAsync(int productId)
        {
            var product = _products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                _products.Remove(product);
            }

            return Task.CompletedTask;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var prod = _products.FirstOrDefault(x => x.ProductId == productId);

            var newProd = new Product();

            if (prod != null)
            {
                newProd.ProductId = productId;
                newProd.ProductName = prod.ProductName;
                newProd.Quantity = prod.Quantity;
                newProd.Price = prod.Price;
                // Need to iterate through the list
                newProd.ProductsInventories = new List<ProductInventory>();
                
                // This approach is done to maintain the In-memory data-store consistency
                if (prod.ProductsInventories != null && prod.ProductsInventories.Count > 0)
                {
                    foreach(var prodInv in prod.ProductsInventories)
                    {
                        var newProdInv = new ProductInventory
                        { 
                            InventoryId = prodInv.InventoryId,
                            ProductId = prodInv.ProductId,
                            Product = prod,
                            Inventory = new Inventory(),
                            InventoryQuantity = prodInv.InventoryQuantity,
                        };
                        
                        if (prodInv.Inventory != null)
                        {
                            newProdInv.Inventory.InventoryId = prodInv.Inventory.InventoryId;
                            newProdInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
                            newProdInv.Inventory.Price = prodInv.Inventory.Price;
                            newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                        }

                        newProd.ProductsInventories.Add(newProdInv);
                    }

                }
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);

            return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateProductAsync(Product product)
        {
            // Basic Checks:
            // Checking if name is different and Product ID is different
            if (_products.Any(x => x.ProductId != product.ProductId &&
                x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            { return Task.CompletedTask; }

            var proToUpdate = _products.FirstOrDefault(x => x.ProductId == product.ProductId);

            if (proToUpdate != null)
            {
                proToUpdate.ProductName = product.ProductName;
                proToUpdate.Quantity = product.Quantity;
                proToUpdate.Price = product.Price;
            }

            return Task.CompletedTask;
        }
    }
}
