using AutoMapper;
using BELibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BELibrary.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMapFromEntitiesToViewModels();
            CreateMapFromViewModelsToEntites();
        }

        private void CreateMapFromViewModelsToEntites()
        {
        }

        private void CreateMapFromEntitiesToViewModels()
        {
        }
    }
}