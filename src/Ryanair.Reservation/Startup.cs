using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ryanair.Reservation.Models.IModelRepository;
using Ryanair.Reservation.Models.ModelRepository;
using Ryanair.Reservation.ServiceLayer;

namespace Ryanair.Reservation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {          
            //services.AddControllers();
            services.AddControllers().AddXmlDataContractSerializerFormatters();
            services.AddSingleton<IReservationRepository,ReservationRepository>();
            services.AddSingleton<IFlightRepository, FlightRepository>();
            services.AddSingleton<IFlightService,FlightService>();
            services.AddSingleton<IReservationService,ReservationService>();
            services.AddSwaggerGen();//adding swagger generator
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swagger=>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json","Ryanair reservation test");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
