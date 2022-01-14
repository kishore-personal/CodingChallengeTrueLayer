using AutoMapper;
using TLPokemon.Application.DTOs.Pokemon;
using TLPokemon.Domain.Entities;

namespace TLPokemon.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Pokemon, PokemonResponse>()
                .ForMember(d => d.Habitat, o => o.MapFrom(s => s.Habitat.HabitatName));
        }
    }
}
