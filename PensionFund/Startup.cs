using Autofac;
using PensionFund.Domain.Constants;
using PensionFund.IoCContainer;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PensionFund
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            services.AddMvc();
            services.AddHealthChecks();
            services.AddSwaggerGen();
            services.AddAWSLambdaHosting(LambdaEventSource.ApplicationLoadBalancer);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.BuildContext(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            string SYSTEM_MANAGER_PATH = Environment.GetEnvironmentVariable("SYSTEM_MANAGER_PATH");

            var pathToContentRoot = AppDomain.CurrentDomain.BaseDirectory;
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(pathToContentRoot)
                .AddSystemsManager(SYSTEM_MANAGER_PATH, TimeSpan.FromSeconds(10))
                .AddEnvironmentVariables()
                .Build();

            Configuration.GetSection($"{ParameterStoreConstants.LOGGINGLEVEL}");
            var loggingLevel = Configuration.GetValue<string>(ParameterStoreConstants.LOGGINGLEVEL);

            var level = LogEventLevel.Error;
            if (LogEventLevel.Information.ToString().Equals(loggingLevel))
                level = LogEventLevel.Information;

            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = level;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
                .WriteTo.Console(level, @"[{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            if (env.IsProduction() || env.IsStaging())
                app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/check-status");
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
