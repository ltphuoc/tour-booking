using Autofac;
using Autofac.Extensions.DependencyInjection;
using BusinessObject.MakeConnection;
using DataAccess.Helpers;
using DataAccess.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;
using TourBookingApi.Mapper;
using TourBookingApi.NewFolder;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

builder.WebHost.UseIISIntegration();

builder.Host.ConfigureContainer<ContainerBuilder>(x => x.RegisterModule(new AutofacModule()));

// Add services to the container

builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory()); //add this...
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        //NamingStrategy = new SnakeCaseNamingStrategy()
        NamingStrategy = new CamelCaseNamingStrategy()
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmailService, EmailService>();

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
    c.OperationFilter<SnakecasingParameOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tour Booking API",
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
// Configure FirebaseApp with service account
var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "firebase.json");
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(pathToKey)
});

service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

service.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "1"));
    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim("Role", "2"));
});

#endregion JWT

service.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
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

