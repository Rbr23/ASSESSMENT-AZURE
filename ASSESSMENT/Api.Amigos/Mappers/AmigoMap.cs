using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Amigos.Mappers
{
    public class AmigoMap : Profile
    {
        public AmigoMap()
        {
            CreateMap<Domain.Amigos, Domain.AmigoResponse>();
        }
}
}