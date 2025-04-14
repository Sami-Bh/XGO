using BuildingBlocks.Controllers;
using BuildingBlocks.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Queries;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Application.Models;
using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Controllers
{
    [Route("api/[controller]")]
    public class StoredItemController : GenericController<StoredItem, StoredItemDto>
    {
        public async override Task<ActionResult> Get()
        {
            return await Task.Run(NoContent);
        }

        [HttpGet("GetFilteredStoredItems")]
        public async Task<ActionResult> GetFilteredStoredItem([FromQuery] ProductsFilter productsFilter)
        {
            return Ok(await Mediator.Send(new GetFilteredStorageItems.Query { ProductsFilter = productsFilter }));
        }
    }
}
