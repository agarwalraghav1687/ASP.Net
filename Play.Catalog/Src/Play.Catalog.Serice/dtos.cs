using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Serice.dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreateDate);

    public record CreateItemDTO([Required] string name, string description, [Range(0, 1000)] decimal price);

    public record UpdateItemDTO([Required] string name, string description, [Range(0, 1000)] decimal price);
}