﻿using System;
using System.Linq;
using System.Reflection;
using Erazer.DAL.ReadModel.Base;
using Erazer.DAL.ReadModel.ClassMaps;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel.Seeding;
using Microsoft.Extensions.DependencyInjection;
using Erazer.Services.Queries.Repositories;

namespace Erazer.Web.ReadAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMongoDbClassMaps(this IApplicationBuilder app)
        {
            // This will only work if every class map is in the same assembly!
            var assembly = typeof(TicketClassMap).GetTypeInfo().Assembly;

            // Base class type
            var type = typeof(MongoDbClassMap<>);

            // Get all types that have MongoDbClassMap as their base class
            var classMaps = assembly.GetTypes()
                .Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface && t.GetTypeInfo().BaseType != null && t.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType)
                .Where(t => t.GetTypeInfo().BaseType.GetGenericTypeDefinition() == type);

            // Create new instance of every class that has the 'MongoDbClass' as base
            foreach (var classMap in classMaps)
                Activator.CreateInstance(classMap);
        }

        public static void Seed(this IApplicationBuilder app)
        {
            Task.WaitAll(
                StatusSeeder.Seed(app.ApplicationServices.GetRequiredService<IStatusQueryRepository>()),
                PrioritySeeder.Seed(app.ApplicationServices.GetRequiredService<IPriorityQueryRepository>())
            );
        }
    }
}
