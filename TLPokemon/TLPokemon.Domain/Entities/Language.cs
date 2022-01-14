using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TLPokemon.Domain.Entities
{
    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
