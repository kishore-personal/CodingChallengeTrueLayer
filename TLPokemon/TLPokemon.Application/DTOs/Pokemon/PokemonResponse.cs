using System;
using System.Collections.Generic;
using System.Text;

namespace TLPokemon.Application.DTOs.Pokemon
{
    public class PokemonResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }
}
