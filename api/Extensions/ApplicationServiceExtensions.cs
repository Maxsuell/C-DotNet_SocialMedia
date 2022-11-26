using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Services;
using api.SignalR;
using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {   
            services.AddCors();
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();            
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
            // essas duas funcoes são padroes na api, elas são as responsaveis por inicias o modelo de api em C#
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSignalR();
            

            return services;
        }
    }
}