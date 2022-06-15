using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracs;
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

        private readonly IPublishEndpoint publishEndPoint;
        public ItemConroller(IRepo<Item> itemsRepo, IPublishEndpoint publishEndPoint)
        {
            this.itemsrepo = itemsRepo;
            this.publishEndPoint = publishEndPoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await itemsrepo.GetAllAsync())
                        .Select(item => item.AsDto());

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

            await publishEndPoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

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

            await publishEndPoint.Publish(new CatalogItemUpdated(existingitem.Id, existingitem.Name, existingitem.Description));


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

            await publishEndPoint.Publish(new CatalogItemDeleted(id));

            await itemsrepo.DeleteAsync(item.Id);



            return NoContent();
        }

    }
}