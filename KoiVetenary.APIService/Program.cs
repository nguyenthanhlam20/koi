using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Writers;

using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using System.Text.Json.Serialization;

namespace KoiVetenary.APIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var modelBuilder = new ODataConventionModelBuilder();
            //modelBuilder.EntitySet<Data.Models.Service>("Services");
            //
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureServiceService(builder.Configuration);
            builder.Services.AddControllers();
            //builder.Services.AddControllers().AddNewtonsoftJson(x =>
            //                 x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddControllers().AddOData(opt =>
            //opt.AddRouteComponents("odata", modelBuilder.GetEdmModel())
            //   .Select()
            //   .Filter()
            //   .OrderBy()
            //   .Expand()
            //   .SetMaxTop(100)
            //   .Count());

            var app = builder.Build();


            ///////////
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
