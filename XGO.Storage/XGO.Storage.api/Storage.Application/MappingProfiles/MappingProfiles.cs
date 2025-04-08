using AutoMapper;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Storage.Application.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<StorageLocation, StorageLocationDto>().
                ForMember(destination => destination.HasChildren, opt => opt.MapFrom(x => x.StoredItems.Count != 0));

            CreateMap<StorageLocationDto, StorageLocation>();
        }
    }
}
