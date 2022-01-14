using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TLPokemon.Domain.Entities
{
    public class ShakespeareApiResponse
    {
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; }
    }
}
