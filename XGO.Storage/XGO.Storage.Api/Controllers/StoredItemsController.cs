using BuildingBlocks.Controllers;
using BuildingBlocks.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Commands;
using XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Queries;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Application.Models;
using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Controllers
{
    [Route("api/[controller]")]
    public class StoredItemsController : GenericController<StoredItem, StoredItemDto>
    {
        public async override Task<ActionResult> Get()
        {
            return await Task.Run(NoContent);
        }

        [HttpGet("GetExpiringItems")]
        public async Task<ActionResult> GetExpiringItems([FromQuery] ExpiringProductsFilter expiringProductsFilter)
        {
            return Ok(await Mediator.Send(new GetExpiringProducts.Query { Filter = expiringProductsFilter }));
        }

        [HttpGet("GetFilteredStoredItems")]
        public async Task<ActionResult> GetFilteredStoredItem([FromQuery] ProductsFilter productsFilter)
        {
            return Ok(await Mediator.Send(new GetFilteredStorageItems.Query { ProductsFilter = productsFilter }));
        }

        [HttpGet("GetStoredItemsNames")]
        public async Task<ActionResult> GetStoredItemsNames([FromQuery] string searchText)
        {
            return Ok(await Mediator.Send(new GetProductNames.Query { SearchText = searchText }));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<StoredItemDto>> Create([FromBody] StoredItemDto storedItem)
        {
            return Ok(await Mediator.Send(new Create.Command { StoredItem = storedItem }));
        }

        [HttpPut("UpdateBatch")]
        public async Task<ActionResult<List<StoredItemDto>>> UpdateBatch([FromBody] List<StoredItemDto> storedItems)
        {
            return Ok(await Mediator.Send(new UpdateBatch.Command { StoredItems = storedItems }));
        }
    }
}
