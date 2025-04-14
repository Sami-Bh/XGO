using BuildingBlocks.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Controllers
{
    [Route("api/[controller]")]
    public class StorageLocationController : GenericController<StorageLocation,StorageLocationDto>
    {
    }
}
