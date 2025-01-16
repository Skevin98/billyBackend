using api.Event.Services;
using api.Event.Services.Impl;
using api.Events.Repositories;
using api.Events.Repositories.Impl;
using api.Events.Services;
using api.Schemas;
using api.Shared;
using api.Tickets.Repositories;
using api.Tickets.Repositories.Impl;
using api.Tickets.Services;
using api.Tickets.Services.Impl;
using api.Users.Repositories;
using api.Users.Repositories.Impl;
using api.Users.Services;
using api.Users.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("BillyDatabase"));

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering();
// .AddErrorFilter<GraphQLErrorFilter>()

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventRepository, EventRepositoryImpl>();
builder.Services.AddScoped<IEventService, EventServiceImpl>();

builder.Services.AddScoped<ITicketTypeRepository, TicketTypeRepositoryImpl>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeServiceImpl>();

builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();

builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            policy => policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        );
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();
app.MapGraphQL();

app.UseHttpsRedirection();


app.Run();