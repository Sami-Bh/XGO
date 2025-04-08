using BuildingBlocks.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XGO.Storage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : AuthorizedControllerBase
    {
    }
}
