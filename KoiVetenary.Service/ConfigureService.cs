using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public static class ConfigureService
    {
        public static IServiceCollection ConfigureServiceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAnimalService, AnimalService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IVeterinarianService, VeterinarianService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            services.AddScoped<IAnimalTypeService, AnimalTypeService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    } 
}
