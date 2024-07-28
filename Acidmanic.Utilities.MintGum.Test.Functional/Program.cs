using Acidmanic.Utilities.MintGum.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// *** MintGum 1) Add MintGum services
builder.Services.AddMintGum(cb => cb.ServeAngularSpa());


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// *** MintGum 2) configure MintGum providers before app.UseRouting();
app.ConfigureMintGumProvider(app.Environment);


app.UseRouting();

// *** MintGum 3) add MintGum maps before adding other mappings 
app.MapMintGum(app.Environment);


app.Run();
