
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using skolesystem.Service;
using skolesystem.Repository;
using skolesystem.Authorization;
using Microsoft.OpenApi.Models;
using skolesystem.Repository.SubjectRepository;
using skolesystem.Repository.ClasseRepository;
using skolesystem.Repository.AssignmentRepository;
using skolesystem.Repository.UserSubmissionRepository;
using skolesystem.Service.SubjectService;
using skolesystem.Service.ClasseService;
using skolesystem.Service.AssignmentService;
using skolesystem.Service.UserSubmissionService;
using skolesystem.Repository.EnrollmentRepository;
using skolesystem.Repository.EnrollmentsRepository;
using skolesystem.Service.EnrollmentService;



var builder = WebApplication.CreateBuilder(args);
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()  // Allow any headers
                                .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

// used when injecting appSettings.Secret into jwtUtils
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "skolesystem", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure MySQL database connection
builder.Services.AddDbContext<User_informationDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<ScheduleDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<UsersDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<AbsenceDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<SubjectsDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<ClasseDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<AssignmentDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<UserSubmissionDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
builder.Services.AddDbContext<EnrollmentDbContext>(
    o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));

// Register JwtUtils

builder.Services.AddScoped<IJwtUtils, JwtUtils>();

// Register your repository
builder.Services.AddScoped<IUser_informationRepository, User_informationRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAbsenceRepository, AbsenceRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IClasseRepository, ClasseRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IUserSubmissionRepository, UserSubmissionRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentsRepository>();


// Register Service

builder.Services.AddScoped<IUser_informationService, User_informationService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IClasseService, ClasseService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IUserSubmissionService, UserSubmissionService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// Add AutoMapper configuration
builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

// JWT middleware setup, use this instead of default Authorization
app.UseMiddleware<JwtMiddleware>();
//app.UseAuthorization();

app.MapControllers();

//app.MapIdentityApi<IdentityUser>();
app.Run();
