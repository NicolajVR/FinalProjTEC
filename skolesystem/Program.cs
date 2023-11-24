
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using skolesystem.Authorization;
using skolesystem.Data;
using skolesystem.Repository;
using skolesystem.Repository.AssignmentRepository;
using skolesystem.Repository.ClasseRepository;
using skolesystem.Repository.EnrollmentRepository;
using skolesystem.Repository.EnrollmentsRepository;
using skolesystem.Repository.SubjectRepository;
using skolesystem.Repository.UserSubmissionRepository;
using skolesystem.Service;
using skolesystem.Service.AssignmentService;
using skolesystem.Service.ClasseService;
using skolesystem.Service.EnrollmentService;
using skolesystem.Service.SubjectService;
using skolesystem.Service.UserSubmissionService;
using System.Text.Json;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
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




/*
    Konfiguration af Swagger:

    - builder.Services.AddEndpointsApiExplorer();
      - Registrerer ApiExplorer-tjenesten, der bruges til at udforske og få oplysninger om API-ruter.

    - builder.Services.AddSwaggerGen(c =>
      - Konfigurerer Swagger ved at tilføje generering af Swagger-dokumentation.

    - c.SwaggerDoc("v1", new OpenApiInfo { Title = "skolesystem", Version = "v1" });
      - Specificerer Swagger-dokumentets information, herunder titel og version.

    - c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
      - Tilføjer definitionen af sikkerhedsordningen "Bearer" for JWT-autentifikation.
      - Dette inkluderer oplysninger som navn, type, format og beskrivelse.

    - c.AddSecurityRequirement(new OpenApiSecurityRequirement
      - Specificerer sikkerhedskravet for at anvende JWT-autentifikation.
      - Kravet er defineret som en reference til sikkerhedsordningen "Bearer".

    - Swagger bruges til at dokumentere API'er og gøre dem lette at forstå og interagere med.
    - Denne konfiguration inkluderer også sikkerhedsdefinitionen og -kravet for JWT-autentifikation.
*/
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




/*
    Konfiguration af MySQL-databaseforbindelse:

    - Hver blok konfigurerer en specifik databasekontekst for en entitet i applikationen ved at tilføje DbContext til tjenestekontaineren.

    - builder.Services.AddDbContext<TDbContext>(
        o => o.UseMySql(builder.Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 35))));
      - Tilføjer en DbContext af typen TDbContext til tjenestekontaineren.
      - Konfigurerer forbindelsesstrengen ("MySQL") ved at hente den fra konfigurationsfilerne.
      - Specificerer MySQL-databasetypen og -versionen ved at bruge UseMySql-metoden med en MySqlServerVersion.

    - Dette er en opsætning af DbContext for forskellige entiteter, hvor hver entitet har sin egen databasekontekst.
    - Forbindelsesstrengen ("MySQL") skal være konfigureret i appens konfigurationsfiler.
    - MySqlServerVersion bruges til at angive MySQL-databasetypen og -versionen.
*/
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




/*
    Registrering af JwtUtils-service:

    - builder.Services.AddScoped<IJwtUtils, JwtUtils>();
      - Registrerer IJwtUtils-interface og dets implementering JwtUtils i tjenestekontaineren.
      - Scoped-levetidsområdet angiver, at en ny instans af JwtUtils oprettes og bruges inden for omfanget af en enkelt HTTP-anmodning.

    - Dette er en måde at registrere en JwtUtils-service på, der håndterer generering og validering af JSON Web Tokens (JWT).
    - JwtUtils kan bruges til at implementere sikkerhedsforanstaltninger som f.eks. autentifikation og autorisation i en webapplikation.
*/
builder.Services.AddScoped<IJwtUtils, JwtUtils>();




/*
    Registrering af repository-interface og -implementeringer:

    - Hver blok registrerer et repository-interface og dets tilsvarende implementering i tjenestekontaineren.
    - Scoped-levetidsområdet angiver, at en ny instans af repository'en oprettes og bruges inden for omfanget af en enkelt HTTP-anmodning.

    - builder.Services.AddScoped<IRepositoryInterface, RepositoryImplementation>();
      - Registrerer et repository-interface og dets implementering i tjenestekontaineren.
      - Scoped-levetidsområdet sikrer, at der oprettes en ny instans af repository'en pr. anmodning.

    - Dette er en måde at registrere repositories på i en afhængighedsinjektionscontainer.
    - Repository'er er ansvarlige for at interagere med databasen og udføre CRUD-operationer for de tilsvarende entiteter.

*/
builder.Services.AddScoped<IUser_informationRepository, User_informationRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAbsenceRepository, AbsenceRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IClasseRepository, ClasseRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IUserSubmissionRepository, UserSubmissionRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentsRepository>();



/*
    Registrering af service-interface og -implementeringer:

    - Hver blok registrerer et service-interface og dets tilsvarende implementering i tjenestekontaineren.
    - Scoped-levetidsområdet angiver, at en ny instans af service'en oprettes og bruges inden for omfanget af en enkelt HTTP-anmodning.

    - builder.Services.AddScoped<IServiceInterface, ServiceImplementation>();
      - Registrerer et service-interface og dets implementering i tjenestekontaineren.
      - Scoped-levetidsområdet sikrer, at der oprettes en ny instans af service'en pr. anmodning.

    - Dette er en måde at registrere services på i en afhængighedsinjektionscontainer.
    - Services indeholder forretningslogik og fungerer som et mellemled mellem controllers og repositories.
*/
builder.Services.AddScoped<IUser_informationService, User_informationService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IClasseService, ClasseService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IUserSubmissionService, UserSubmissionService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

/*
    Konfiguration af AutoMapper:

    - builder.Services.AddAutoMapper(typeof(MappingProfiles));
      - Registrerer AutoMapper ved at angive den klasse (MappingProfiles), der indeholder profildefinitionerne for kortlægning af entiteter til DTO'er og omvendt.

    - AutoMapper bruges til at automatisere kortlægningen mellem objekter af forskellige typer.
    - MappingProfiles er en klasse, der indeholder AutoMapper-profiler, hvor kortlægningslogikken konfigureres.
    - Denne tjenesteregistrering gør det muligt for applikationen at bruge AutoMapper til at udføre kortlægning i tjenester og kontrollere.
*/
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