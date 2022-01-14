using System;
using System.Collections.Generic;
using System.Text;

namespace TLPokemon.Application.DTOs.Health
{
   public class HealthCheckResponses
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheckResponse> HealthChecks { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}
