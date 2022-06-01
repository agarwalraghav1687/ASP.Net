using Play.Catalog.Serice.dtos;
using Play.Catalog.Serice.Entities;

namespace Play.Catalog.Serice
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreateDate);
        }
    }
}