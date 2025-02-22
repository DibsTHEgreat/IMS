using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        // These validations are called DataAnnotations (DA) which is independent of ASP.NET Core
        // Whether you are developing Blazor, or Razor pages, MVC, or WebAPI you can use DA.
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater or equal to 0.")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater or equal to 0.")]
        public double Price { get; set; }

        [Product_EnsurePriceIsGreaterThanInvetoriesCost]
        public List<ProductInventory> ProductsInventories { get; set; } = new List<ProductInventory>();

        public void AddInventory(Inventory inventory)
        {
            if (!this.ProductsInventories.Any(
                x => x.Inventory is not null && 
                x.Inventory.InventoryName.Equals(inventory.InventoryName)))
            {
                this.ProductsInventories.Add(new ProductInventory
                {
                    InventoryId = inventory.InventoryId,
                    Inventory = inventory,
                    InventoryQuantity = 1,
                    ProductId = this.ProductId,
                    Product = this
                });
            }
        }

        public void RemoveInventory(ProductInventory productInventory)
        {
            this.ProductsInventories?.Remove(productInventory);
        }
    }
}
