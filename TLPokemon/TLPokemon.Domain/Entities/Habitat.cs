using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TLPokemon.Domain.Entities
{
    public class Habitat
    {
        [JsonPropertyName("name")]
        public string HabitatName { get; set; }
    }
}
