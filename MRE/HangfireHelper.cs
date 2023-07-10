using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.SqlServer;
using MRE.Application.HangFire;
using MediatR;

namespace MRE
{
    public static class HangfireHelper
    {
        public static void AddHangFire(this WebApplicationBuilder builder,string connectionString)
        {
            var sp = builder.Services.BuildServiceProvider();

            // Add Hangfire services.
            builder.Services.AddHangfire(configuration =>
            {
                configuration.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                 {
                     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(60),
                     QueuePollInterval = TimeSpan.FromSeconds(5)
                 });
            });

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();

            //setup jobs
            var serviceProvider = builder.Services.BuildServiceProvider();
            JobStorage.Current = new SqlServerStorage(connectionString);
           
        }


        public static void UseHangFire(this WebApplication app)
        {
            //Hangfire
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions{
                               RequireSsl = false,
                                SslRedirect = false,
                                LoginCaseSensitive = true,
                                Users = new []
                                {
                                    new BasicAuthAuthorizationUser
                                    {
                                        Login =app.Configuration.GetValue<string>("HangFire:UserName"),
                                        PasswordClear=app.Configuration.GetValue<string>("HangFire:Password")
                                    }
                                }
                    })
                }
            });
        }
    }
}
