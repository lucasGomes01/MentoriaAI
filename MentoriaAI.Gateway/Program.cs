using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var supabaseUrl = builder.Configuration["Supabase:Url"]
    ?? Environment.GetEnvironmentVariable("SUPABASE_URL");

var jwksUrl = $"{supabaseUrl}/auth/v1/keys";

var supabaseJwtSecret = builder.Configuration["Supabase:JwtSecret"]
    ?? Environment.GetEnvironmentVariable("SUPABASE__JWT__SECRET");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseJwtSecret)),

            ValidateIssuer = true,
            ValidIssuer = $"{supabaseUrl}/auth/v1",

            ValidateAudience = true,
            ValidAudience = "authenticated",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            // Para Teste
            TryAllIssuerSigningKeys = true
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Token inválido: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AllowAnonymous", policy =>
//    {
//        policy.RequireAssertion(_ => true);
//    });
//});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("AllowAnonymous", policy =>
    {
        policy.RequireAssertion(_ => true);
    });

    options.AddPolicy("ReadOnlyOrAuthenticated", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;

            if (httpContext == null)
                return false;

            var method = httpContext.Request.Method;

            // Permite GET sem login
            //if (method == HttpMethods.Get)
            //{
            //    return true;
            //}
            return true;
            // Outros métodos exigem autenticação
            return context.User.Identity?.IsAuthenticated == true;
        });
    });
});

//builder.Services.AddAuthorization(options =>
//{
//    options.DefaultPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//
//    options.AddPolicy("AllowAnonymous", policy =>
//    {
//        policy.RequireAssertion(_ => true);
//    });
//});

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.StatusCode = 200;
        return;
    }

    await next();
});

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var userId = context.User.FindFirst("sub")?.Value;
        var email = context.User.FindFirst("email")?.Value;

        context.Request.Headers["X-User-Id"] = userId ?? "";
        context.Request.Headers["X-User-Email"] = email ?? "";
    }

    await next();
});

app.MapReverseProxy();

app.Run();
