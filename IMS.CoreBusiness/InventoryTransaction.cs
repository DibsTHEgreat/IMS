using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class InventoryTransaction
    {

        public int InventoryTransactionId { get; set; }
        
        public string PONumber { get; set; } = string.Empty;

        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public InventoryTransactionType? ActivityType { get; set; }
        
        public int QuantityAfter { get; set; }
        
        public double UnitPrice { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        // Navigation Property
        public Inventory? Inventory { get; set; }
        
    }
}
