using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepo<InventoryItem> itemsRepo;

        private readonly CatalogClient catalogClient;

        public ItemsController(IRepo<InventoryItem> itemsRepo, CatalogClient catalogClient)
        {
            this.itemsRepo = itemsRepo;
            this.catalogClient = catalogClient;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogItemAsync();
            var inventoryItemEntities = await itemsRepo.GetAllAsync(item => item.UserId == userId);

            var inventoryItemDto = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemDto);
        }

        [HttpPost]

        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var inventoryItem = await itemsRepo.GetAsync(item => item.UserId == grantItemDto.UserId && item.CatalogId == grantItemDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogId = grantItemDto.CatalogItemId,
                    UserId = grantItemDto.UserId,
                    Quantity = grantItemDto.Quantity,
                    AcquireDate = DateTimeOffset.UtcNow
                };

                await itemsRepo.CreateAsync(inventoryItem);
            }

            else
            {
                inventoryItem.Quantity += grantItemDto.Quantity;
                await itemsRepo.UpdateAync(inventoryItem);
            }

            return Ok();

        }
    }
}