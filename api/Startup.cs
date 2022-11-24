using api.Extensions;
using api.Middleware;
using api.SignalR;

namespace api.LotusSocialMedia
{
    public class Startup
    {
        //Injeção de dependencias, a função Startup inicia a variavel _config com o valor passado por parametro do tipo IConfiguration
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
            
        }


        // This method gets called by the runtime. Use this method to add services to the container. Esse metodo inicia serviço, ou seja, metodos que não são construidos ao decorredor do algoritmo, assim eles podem ser usado em todo o sistema sem a necessidade de implementação nos construtores
        public void ConfigureServices(IServiceCollection services)
        {
            // Arquivo externo contendo os servicos do sistema
            services.AddApplicationServices(_config);
            // essas duas funcoes são padroes na api, elas são as responsaveis por inicias o modelo de api em C#
            services.AddControllers();
            services.AddCors();
            services.AddIdentityServices(_config);
            services.AddSignalR();      
        }

        // Esse metodo da inicio ao sistema, esse é o metodo principal e por ele os outros metodos são chamado e construidos
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            
            //Chamada de metodos padroes de conexão com uma pagina http e https
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins("https://localhost:4200"));

            // Metodo de iniciação de de funcoes que controlam a autenticação e a autorização
            app.UseAuthentication();
            app.UseAuthorization();

            //Endpoint função que inicia os controladores
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/message");
            });
        }
    }
}
