using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Serice.dtos;
using Play.Catalog.Serice.Entities;
using Play.Common;

namespace Play.Catalog.Serice.Controllers
{

    [ApiController]
    [Route("Items")]
    public class ItemConroller : ControllerBase
    {
        private readonly IRepo<Item> itemsrepo;

        private static int requestCounter = 0;

        public ItemConroller(IRepo<Item> itemsRepo)
        {
            this.itemsrepo = itemsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {

            requestCounter++;
            Console.WriteLine($"Request {requestCounter} starting...");

            if (requestCounter <= 2)
            {
                Console.WriteLine($"Request {requestCounter} delaying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            if (requestCounter <= 4)
            {
                Console.WriteLine($"Request {requestCounter} 500 (internal Server Error)...");
                return StatusCode(500);
            }


            var items = (await itemsrepo.GetAllAsync())
                        .Select(item => item.AsDto());
            Console.WriteLine($"Request {requestCounter}: 200(OK)");

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsrepo.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDTO createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.name,
                Description = createItemDto.description,
                Price = createItemDto.price,
                CreateDate = DateTimeOffset.UtcNow
            };

            await itemsrepo.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDTO updateItemDto)
        {
            var existingitem = await itemsrepo.GetAsync(id);

            if (existingitem == null)
            {
                return NotFound();
            }

            existingitem.Name = updateItemDto.name;
            existingitem.Description = updateItemDto.description;
            existingitem.Price = updateItemDto.price;

            await itemsrepo.UpdateAync(existingitem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsrepo.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await itemsrepo.DeleteAsync(item.Id);

            return NoContent();
        }

    }
}