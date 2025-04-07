using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IInventoryRepository inventoryRepository;
        public List<InventoryTransaction> _inventoryTransactions = new List<InventoryTransaction>();

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository) 
        {
            this.inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            // this would definitely cause a performance issue with the software, however, we are only calling in-memory data
            var inventories = (await inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

            // now we use a LinQ query because we are querying the in-memory db
            var query = from it in this._inventoryTransactions
                        join inv in inventories on it.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrEmpty(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                            &&
                            (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) 
                            &&
                            (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date)
                            &&
                            (!transactionType.HasValue || it.ActivityType == transactionType)
                         select new InventoryTransaction
                         {
                             Inventory = inv,
                             InventoryTransactionId = it.InventoryTransactionId,
                             PONumber = it.PONumber,
                             InventoryId = it.InventoryId,
                             QuantityBefore = it.QuantityBefore,
                             ActivityType = it.ActivityType,
                             QuantityAfter = it.QuantityAfter,
                             TransactionDate = it.TransactionDate,
                             CreatedBy = it.CreatedBy,
                             UnitPrice = it.UnitPrice
                         };

            // Very similar to SQL
            /* 
                select *
                    from inventoryTransaction it
                    join inventories inv on it.InventoryId = inv.inventoryId
             */

            return query;
        }

        public void ProduceAsync(string productionNumber, Inventory inventory, int quantityConsumed, string createdBy, double price)
        {
            this._inventoryTransactions.Add(new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                Inventory = inventory,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.ProduceProducts,
                QuantityAfter = inventory.Quantity - quantityConsumed,
                TransactionDate = DateTime.Now,
                CreatedBy = createdBy,
                UnitPrice = price
            });
        }

        public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string createdBy, double price)
        {
            this._inventoryTransactions.Add(new InventoryTransaction
            {
                PONumber = poNumber,
                Inventory = inventory,
                InventoryId = inventory.InventoryId,
                QuantityBefore = quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.Now,
                CreatedBy = createdBy,
                UnitPrice = price
            });
        }
    }
}
