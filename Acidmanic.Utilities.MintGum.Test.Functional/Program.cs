using Acidmanic.Utilities.MintGum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ** 1) - Create static server configurator
var frontEndApplication = new StaticServerConfigurator()
    .ServeForAngular();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ** 2) - This line should be called before app.UseRouting();
frontEndApplication.ConfigurePreRouting(app, app.Environment);

app.UseRouting();

// ** 3) - line Should be called before mapping controllers
frontEndApplication.ConfigureMappings(app, app.Environment);


app.Run();
