using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string createdBy, double price);
    
        void ProduceAsync(string productionNumber, Inventory inventory, int quantityConsumed, string createdBy, double price);
    }
}