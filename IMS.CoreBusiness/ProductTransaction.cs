using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class ProductTransaction
    {

        public int ProductTransactionId { get; set; }
        
        public string SONumber { get; set; } = string.Empty;

        public string ProductionNumber { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public ProductTransactionType? ActivityType { get; set; }
        
        public int QuantityAfter { get; set; }
        
        public double? UnitPrice { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        // Navigation Property
        public Product? Product { get; set; }
        
    }
}
