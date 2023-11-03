using NLog.Web;
using NLog;
using Microsoft.AspNetCore.Mvc.Razor;
using VizGuideBackend.Data;
using VizGuideBackend.Repository.ModelsIRepository;
using VizGuideBackend.Repository.ModelsRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init main");

try
{
    var ReadOnlyAllowSpecificOrigins = "_readOnlyAllowSpecificOrigins";
    var AdminAllowSpecificOrigins = "_adminAllowSpecificOrigins";

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: AdminAllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins(
                            "http://89.111.140.52:8081",
                            "http://89.111.140.52:8080"
                            )
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                        });
        options.AddPolicy(name: ReadOnlyAllowSpecificOrigins,
                          policy =>
                          {
							  policy.WithOrigins(
							  "https://www.viz-guide.online:8081",
							  "https://www.viz-guide.online:8082",
							  "https://viz-guide.online:8081",
							  "https://viz-guide.online:8082"
							  )
                                    .AllowAnyHeader()
                                    .WithMethods("GET");
                          });
    });

    //

    builder.Services.AddRazorPages();

    builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt =>
			opt.UseNpgsql(builder.Configuration.GetConnectionString("SampleDbConnection")));

	builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
	builder.Services.AddTransient<IScriptsRepository, ScriptsRepository>();
	builder.Services.AddTransient<IBaseTypesRepository, BaseTypesRepository>();
	builder.Services.AddTransient<IMemberProceduresRepository, MemberProceduresRepository>();
	builder.Services.AddTransient<IPropertiesRepository, PropertiesRepository>();

    var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		//app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

    app.UseDefaultFiles();
	app.UseStaticFiles();

    app.MapRazorPages();

    app.MapControllerRoute(
		name: "default",
		pattern: "{controller}/{action=Index}/{id?}");

    app.MapFallbackToFile("index.html"); 

    app.UseHttpsRedirection();

	app.UseRouting();

    app.UseCors(AdminAllowSpecificOrigins);
    app.UseCors(ReadOnlyAllowSpecificOrigins);
    //app.UseCors();

    app.UseAuthorization();
    
    app.Run();
}
catch (Exception exception)
{
	// NLog: catch setup errors
	logger.Error(exception, "Stopped program because of exception");
	throw;
}
finally
{
	// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
	LogManager.Shutdown();
}
