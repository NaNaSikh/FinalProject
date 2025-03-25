using System.Text;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
using EmployeeBonusManagementSystem.Application.Mapping;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Application;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
	    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

	    // Enable JWT Authentication in Swagger UI
	    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	    {
		    Description = "Enter 'Bearer {token}' (without quotes) in the text box below.",
		    Name = "Authorization",
		    In = ParameterLocation.Header,
		    Type = SecuritySchemeType.Http,
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
			    new string[] {}
		    }
	    });
    });

    builder.Services.AddLogging();


	builder.Services.AddAutoMapper(typeof(EmployeeProfile));
	builder.Services.AddAutoMapper(typeof(BonusProfile));

	builder.Services.AddHttpContextAccessor();

	builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddEmployeeCommand).Assembly));


	builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	    .AddJwtBearer(options =>
	    {
		    options.TokenValidationParameters = new TokenValidationParameters
		    {
			    ValidateIssuer = true,
			    ValidateAudience = true,
			    ValidateLifetime = true,
			    ValidateIssuerSigningKey = true,
			    ValidIssuer = builder.Configuration["Jwt:Issuer"],
			    ValidAudience = builder.Configuration["Jwt:Audience"],
			    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		    };
	    });

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("User", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("User"); // Both "User" and "Admin" can access
	});

	options.AddPolicy("Admin", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("Admin"); // Only "Admin" can access
	});
});



	

// Configure the HTTP request pipeline.
var app = builder.Build();

    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}

    app.UseSwagger();

    app.UseSwaggerUI();

    app.UseHttpsRedirection();


	app.MapControllers();

    app.Run();


