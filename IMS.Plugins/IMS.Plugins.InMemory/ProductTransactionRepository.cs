using IMS.CoreBusiness;
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
        private readonly IProductRepository productRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;

        public ProductTransactionRepository(IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository)
        {
            this.productRepository = productRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string createdBy)
        {
            // decrease the inventory
            var prod = await this.productRepository.GetProductByIdAsync(product.ProductId);
            if (prod != null) 
            {
                foreach(var pi in prod.ProductsInventories)
                {

                    if (pi.Inventory != null)
                    {
                        this.inventoryTransactionRepository.ProduceAsync(productionNumber,
                            pi.Inventory,
                            pi.InventoryQuantity * quantity,
                            createdBy,
                             -1);
                    }
                }
            }

            // add inventory transaction record

            // add the product transaction

        }

        public Task ProduceAsync(Product product, int quantity, string createdBy)
        {
            throw new NotImplementedException();
        }
    }
}
