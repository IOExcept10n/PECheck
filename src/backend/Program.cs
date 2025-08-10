using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using backend.Auth;
using backend.Data;
using backend.Models;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Physical Education Attendance API", 
        Version = "v1",
        Description = "API for tracking student attendance in physical education sections. This API provides functionality for managing students, teachers, sections, attendance tracking, and grading.",
        Contact = new OpenApiContact
        {
            Name = "Admin",
            Email = "admin@example.com"
        }
    });

    // Define JWT Bearer authentication scheme for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer {token}' in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });

    // Set the comments path for the Swagger JSON and UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Configure swagger to use the full schema name to avoid naming conflicts
    c.CustomSchemaIds(type => type.FullName);

    // Group endpoints by controller
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((docName, apiDesc) => true);
});

// Configure SQLite database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "defaultkeythatneedstobechanged12345678");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        ClockSkew = TimeSpan.Zero
    };
});

// Register Services
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SectionService>();
builder.Services.AddScoped<StudentSectionService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<StatsService>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PE Check API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DefaultModelsExpandDepth(-1); // Hide schemas section by default
        c.DisplayRequestDuration();
    });
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed data will be added here
        await SeedData(userManager, roleManager, context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
{
    // Create roles if they don't exist
    string[] roleNames = { "Moderator", "Teacher", "Student" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create admin user
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Moderator");
        }
    }

    // Create a teacher
    var teacherUser = await userManager.FindByEmailAsync("teacher@example.com");
    if (teacherUser == null)
    {
        teacherUser = new ApplicationUser
        {
            UserName = "teacher@example.com",
            Email = "teacher@example.com",
            FirstName = "Teacher",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(teacherUser, "Teacher123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(teacherUser, "Teacher");
        }
    }

    // Create a student
    var studentUser = await userManager.FindByEmailAsync("student@example.com");
    if (studentUser == null)
    {
        studentUser = new ApplicationUser
        {
            UserName = "student@example.com",
            Email = "student@example.com",
            FirstName = "Student",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(studentUser, "Student123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(studentUser, "Student");
        }
    }

    // Create a semester if none exists
    if (!await context.Semesters.AnyAsync())
    {
        var semester = new Semester
        {
            Name = "Fall 2023",
            StartDate = new DateTime(2023, 9, 1),
            EndDate = new DateTime(2023, 12, 31),
            IsActive = true
        };

        context.Semesters.Add(semester);
        await context.SaveChangesAsync();
    }

    // Create a section if none exists
    if (!await context.Sections.AnyAsync() && teacherUser != null)
    {
        var section = new Section
        {
            Name = "Basketball",
            Description = "Basketball training for beginners",
            TeacherId = teacherUser.Id,
            MaxStudents = 20,
            MinAttendanceForGrade = 12,
            MaxAttendance = 20,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Sections.Add(section);
        await context.SaveChangesAsync();

        // Add a schedule for this section
        var schedule = new Schedule
        {
            SectionId = section.Id,
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeSpan(14, 0, 0), // 2:00 PM
            EndTime = new TimeSpan(15, 30, 0),  // 3:30 PM
            Location = "Main Gym",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Schedules.Add(schedule);
        await context.SaveChangesAsync();
    }
}