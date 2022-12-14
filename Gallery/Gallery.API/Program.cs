using Gallery.API.Extensions;
using Gallery.DAL;
using Gallery.DAL.Configurations;
using Gallery.DAL.Configurations.Interfaces;
using Gallery.DAL.Domain;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<GalleryDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
    {
        opt.EnableRetryOnFailure();
    }).EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsFactory>();
builder.Services.AddAutoMapperExtensions();
builder.Services.AddExceptionExtensions();
builder.Services.AddIdentityExtensions(configuration);

builder.Services.AddAuthenticationExtensions(configuration);

builder.Services.AddServiceExtensions();
builder.Services.AddConfigurations(configuration);
builder.Services.AddSwaggerExtension(configuration);

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", true, true);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GalleryDbContext>();
    dbContext.Database.Migrate();
}

app.UseProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", configuration.GetValue<string>("IdentityServer:Name"));
        options.OAuthClientId(configuration.GetValue<string>("IdentityServer:ClientId"));
        options.OAuthClientSecret(configuration.GetValue<string>("IdentityServer:ClientSecret"));
        options.OAuthAppName(configuration.GetValue<string>("IdentityServer:Name"));
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

var configService = app.Services.GetRequiredService<IGalleryConfigurationService>(); 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(configService.GetStaticFilePhysicalPath()),
    RequestPath = $"/{configService.GetStaticFileRequestPath()}"
});

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Services.AddRoleSeedExtensionExtensions(configuration);

app.Run();
