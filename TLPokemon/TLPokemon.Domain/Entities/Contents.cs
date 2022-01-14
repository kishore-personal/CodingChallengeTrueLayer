using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TLPokemon.Domain.Entities
{
    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
    }
}
