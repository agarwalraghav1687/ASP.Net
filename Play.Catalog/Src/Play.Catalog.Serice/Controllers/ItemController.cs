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
        public ActionResult<ItemDto> Get(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDTO createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.name, createItemDto.description, createItemDto.price, DateTimeOffset.UtcNow);
            
            items.Add(item);

            return CreatedAtAction(nameof(Get), new {id = item.Id}, item);
        }

        [HttpPut("{id}")]

        public IActionResult Put(Guid id, UpdateItemDTO updateItemDto)
        {
            var existingitem = items.Where (item => item.Id == id).SingleOrDefault();

            if(existingitem == null)
            {
                return NotFound();
            }

            var UpdatedItem = existingitem with {
                Name = updateItemDto.name,
                Description = updateItemDto.description,
                Price = updateItemDto.price
            };

            var index = items.FindIndex(existingitem => existingitem.Id == id);
            items[index] = UpdatedItem;
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(existingitem => existingitem.Id == id);

            if(index < 0)
            {
                return NotFound();
            }
            items.RemoveAt(index);

            return NoContent();
        }

    }
}