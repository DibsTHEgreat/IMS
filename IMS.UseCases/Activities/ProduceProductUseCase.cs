using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductTransactionRepository productTransactionRepository;
        private readonly IProductRepository productRepository;

        public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository,
            IProductRepository productRepository)
        {
            this.productTransactionRepository = productTransactionRepository;
            this.productRepository = productRepository;
        }

        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string createdBy)
        {
            // add transaction record
            await this.productTransactionRepository.ProduceAsync(product, quantity, createdBy);

            // decrease the quantity of inventories


            // update the quantity of product
            product.Quantity += quantity;
            await this.productRepository.UpdateProductAsync(product);
        }
    }
}
