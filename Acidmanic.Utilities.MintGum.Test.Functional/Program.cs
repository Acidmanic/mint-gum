using Acidmanic.Utilities.MintGum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Create static server configurator
var frontEndApplication = new StaticServerConfigurator()
    .ServeForAngular();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This line should be called before app.UseRouting();
frontEndApplication.ConfigurePreRouting(app, app.Environment);

app.UseRouting();

// This line Should be called after routing to configure maps 
frontEndApplication.ConfigureMappings(app, app.Environment);


app.Run();
