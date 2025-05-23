﻿using BuildingBlocks.DTOs;

namespace XGO.Storage.Api.Storage.Application.DTOs
{
    public class StorageLocationDto : BaseDto
    {
        public required string Name { get; set; }
        public bool HasChildren { get; set; }
    }
}
