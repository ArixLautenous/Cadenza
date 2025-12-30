using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RX_Server.Data;
using RX_Server.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Ket noi SQL
//Lay chuoi ket noi tu file appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//THÊM CORS - QUAN TRỌNG CHO CLIENT
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()      // Cho phép mọi origin (localhost, 127.0.0.1...)
              .AllowAnyMethod()      // Cho phép GET, POST, PUT, DELETE...
              .AllowAnyHeader();     // Cho phép mọi header
    });
});

//Dang ki Services
//AddScoped: Tao moi khi co request (thuong dung cho Service co logic DB)
builder.Services.AddScoped<IPaymentService, PaymentService>();

//AddSingleton: Tao 1 lan va dung mai (neu khong dinh den DB state) hoac AddScoped neu muon an toan. O day se dung Scoped.
builder.Services.AddScoped<IStreamingService, StreamingService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAudioProcessingService, AudioProcessingService>();

//Cau hinh JWT Authentication
//JWT Key can duoc luu trong appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super-secret-key-1234567890-min-length-32-chars"; 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Trong moi truong Dev co the bo qua
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        
        // Cho phep lay Token tu Query String (cho chuc nang Stream nhac)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // Nếu request đến đường dẫn "/api/songs/.../stream"
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/api/songs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

//Data seeding
//Tu dong chay MMigration khi khoi dong ung dung
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbInitializer.Seed(context); //Goi phuong thuc Seed de them du lieu mau
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Đã xảy ra lõi khi tạo Database.");
    }
}

//HTTP Request Pipeline
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".wav"] = "audio/wav"; //Dang MIME cho WAV
provider.Mappings[".flac"] = "audio/flac"; //Dang MIME cho FLAC
provider.Mappings[".m4a"] = "audio/mp4"; //Dang MIME cho AAC
provider.Mappings[".mp3"] = "audio/mpeg"; //Dang MIME cho MP3
provider.Mappings[".aac"] = "audio/aac";
provider.Mappings[".wma"] = "audio/x-ms-wma";
provider.Mappings[".ogg"] = "audio/ogg";
provider.Mappings[".opus"] = "audio/opus";
provider.Mappings[".webp"] = "image/webp";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
    //Bao mat cho folder audio khong cho truy cap truc tiep qua link
    //Can them OnPrepareResponse de kiem tra quyen (nang cao)
    //O day se de public de Client de stream
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// SỬ DỤNG CORS - ĐẶT TRƯỚC UseAuthentication
app.UseCors("AllowAll");

// Thu tu quan trong: Authen -> Author -> MapControllers
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();