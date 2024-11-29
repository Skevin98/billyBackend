using api.Event.Impl;
using api.Event.Repositories;
using api.Event.Services;
using api.Event.Services.Impl;
using api.Schemas;
using api.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("BillyDatabase"));

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();
// .AddErrorFilter<GraphQLErrorFilter>()

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventRepository, EventRepositoryImpl>();
builder.Services.AddScoped<IEventService, EventServiceImpl>();

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