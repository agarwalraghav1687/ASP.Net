using Play.Inventory.Service.dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extension
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string name, string desciption)
        {
            return new InventoryItemDto(item.CatalogId, name, desciption, item.Quantity, item.AcquireDate);
        }
    }
}