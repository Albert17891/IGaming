﻿using IGaming.Core.Interfaces;
using IGaming.Core.Services;
using IGaming.Domain.Options;
using Market.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IGaming.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration)
    {      
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
