using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Serice.dtos;

namespace Play.Catalog.Serice.Controllers
{

    [ApiController]
    [Route("Items")]
    public class ItemConroller : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Heals by 50", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Removes Posion", 40, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Sword", "Increade damage by 20", 90, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ItemDto Get(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return item;
        }
    }
}