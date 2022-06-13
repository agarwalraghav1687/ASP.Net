namespace Play.Inventory.Service.dtos
{
    public record GrantItemDto(Guid UserId, Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(Guid CatalogItemId, string Name, string Desciption, int Quantity, DateTimeOffset AcquiredDate);

    public record CatalogItemDto(Guid Id, string Name, string Description);
}