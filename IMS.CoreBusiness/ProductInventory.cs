using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    // A single product has many inventories, and one inventory can belong in many products.
    // Therefore, this is a Many-to-Many relationship. Thus, it's better for us to have a
    // in-between table.
    public class ProductInventory
    {
        // Foreign Key - Associative property linking to Product
        public int ProductId { get; set; }

        // Navigation Property - References the related Product entity
        // We ignore this during serialization to avoid cyclical reference issues
        [JsonIgnore]
        public Product? Product { get; set; }

        // Foreign Key - Associative property linking to Inventory
        public int InventoryId { get; set; }

        // Navigation Property - References the related Inventory entity
        // We ignore this during serialization to avoid cyclical reference issues
        [JsonIgnore]
        public Inventory? Inventory { get; set; }

        public int InventoryQuantity { get; set; }
    }
}
