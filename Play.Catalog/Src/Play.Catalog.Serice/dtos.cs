using System;

namespace Play.Catalog.Serice.dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreateDate);

    public record CreateItemDTO(Guid Id, string name, string description, decimal price);

    public record UpdateItemDTO(Guid Id, string name, string description, decimal price);
}