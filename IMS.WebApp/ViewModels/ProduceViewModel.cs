﻿using IMS.CoreBusiness;
using IMS.WebApp.ViewModelsValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class ProduceViewModel
    {
        [Required]
        public string ProductionNumber { get; set; } = string.Empty;

        [Required]
        [Range(minimum: 1, maximum:int.MaxValue, ErrorMessage = "You have to select a product.")]
        public int ProductId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1.")]
        [Product_EnsureEnoughInventoryQuantity]
        public int QuantityToProduce { get; set; }

        // acts like a navigation property
        public Product? Product { get; set; } = null;
    }
}
