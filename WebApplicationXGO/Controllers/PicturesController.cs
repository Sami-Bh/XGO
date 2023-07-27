using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Pictures}")]
    public class PicturesController : GenericController<Picture>
    {
        public PicturesController(IPictureRepository picturesRepository)
        {
            _repositoryService = picturesRepository;
        }
    }
}
