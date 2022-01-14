using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
//using TLPokemon.Application.Behaviours;
using TLPokemon.Application.Interfaces;

namespace TLPokemon.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//            services.AddMediatR(Assembly.GetExecutingAssembly());
            
        }
    }
}
