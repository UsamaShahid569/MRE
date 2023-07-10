using DinkToPdf;
using DinkToPdf.Contracts;
using MRE.Presistence.InternalClasses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Concrete;

namespace MRE.Presistence.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseDinkToPdf(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //var context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "LibwkHtml", "libwkhtmltox.dll"));

            return services;
        }
        public static IServiceCollection AddProjectConfig(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRpository>();
            return services;
        }
    }
}
