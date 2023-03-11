using Autofac;
using Autofac.Extensions.DependencyInjection;
using BusinessObject.MakeConnection;
using BusinessObject.Repository;
using BusinessObject.UnitOfWork;
using DataAccess.Helpers;
using DataAccess.Services;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.OpenApi.Models;
using System.Text;
using TourBookingApi.Mapper;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

builder.WebHost.UseIISIntegration();

builder.Host.ConfigureContainer<ContainerBuilder>(x => x.RegisterModule(new AutofacModule()));

// Add services to the container

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var service = builder.Services;

service.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

service.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder =>
        {
            builder
            //.WithOrigins(GetDomain())
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
service.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

service.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eCloset API",
        Version = "v1"
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                        securitySchema,
                    new string[] { "Bearer" }
        }
                });
});
service.ConnectToConnectionString(builder.Configuration);


#region JWT

var appSettingsSection = builder.Configuration.GetSection("AppSettings");

service.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

var key = Encoding.ASCII.GetBytes(appSettings.Secret);
service.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
});
var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "firebase.json");
//FirebaseApp.Create(new AppOptions
//{
//    Credential = GoogleCredential.FromFile(pathToKey)
//});

#endregion JWT

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tour Booking V1");
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
