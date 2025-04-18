﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {

        private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();

        private readonly IProductRepository productRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;
        private readonly IInventoryRepository inventoryRepository;

        public ProductTransactionRepository(
            IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            this.productRepository = productRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, double? price, string createdBy)
        {
            var prod = await this.productRepository.GetProductByIdAsync(product.ProductId);


            if (prod != null) 
            {
                foreach(var pi in prod.ProductsInventories)
                {

                    if (pi.Inventory != null)
                    {
                        // add product transaction
                        this.inventoryTransactionRepository.ProduceAsync(productionNumber,
                            pi.Inventory,
                            pi.InventoryQuantity * quantity,
                            createdBy,
                             -1);

                        // decrease the inventory
                        var inv = await this.inventoryRepository.GetInventoriesByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;
                        await this.inventoryRepository.UpdateInventoryAsync(inv);
                    }
                }
            }

            // add the product transaction
            this._productTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.Now,
                CreatedBy = createdBy,
            });
        }

        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            this._productTransactions.Add(new ProductTransaction
                {
                    ActivityType = ProductTransactionType.SellProduct,
                    SONumber = salesOrderNumber,
                    ProductId = product.ProductId,
                    QuantityBefore = product.Quantity,
                    QuantityAfter = product.Quantity - quantity,
                    TransactionDate = DateTime.Now,
                    CreatedBy = doneBy,
                    UnitPrice = unitPrice,
                });

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            // this would definitely cause a performance issue with the software, however, we are only calling in-memory data
            var products = (await productRepository.GetProductsByNameAsync(string.Empty)).ToList();

            // now we use a LinQ query because we are querying the in-memory db
            var query = from it in this._productTransactions
                        join product in products on it.ProductId equals product.ProductId
                        where
                            (string.IsNullOrEmpty(productName) || product.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                            &&
                            (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date)
                            &&
                            (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date)
                            &&
                            (!transactionType.HasValue || it.ActivityType == transactionType)
                        select new ProductTransaction
                        {
                            Product = product,
                            ProductTransactionId = it.ProductTransactionId,
                            SONumber = it.SONumber,
                            ProductionNumber = it.ProductionNumber,
                            ProductId = it.ProductId,
                            QuantityBefore = it.QuantityBefore,
                            ActivityType = it.ActivityType,
                            QuantityAfter = it.QuantityAfter,
                            TransactionDate = it.TransactionDate,
                            CreatedBy = it.CreatedBy,
                            UnitPrice = it.UnitPrice
                        };

            return query;
        }
    }
}
