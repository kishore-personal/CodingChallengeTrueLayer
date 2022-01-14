using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TLPokemon.Domain.Entities
{
     public class Pokemon
     {
        public string Name { get; set; } 
        [JsonPropertyName("flavor_text_entries")]
        public List<Flavor> Flavors { get; set; }
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
        [JsonPropertyName("habitat")]
        public Habitat Habitat { get; set; }
        public string Description { get; set; }

    }
}
