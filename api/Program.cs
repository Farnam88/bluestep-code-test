using api.Application.DependencyInjections;
using api.Domain.DependencyInjections;
using api.ExternalServices.DependencyInjections;
using api.Presentation.DependencyInjections;
using Microsoft.OpenApi.Models;

var builder = EnvironmentDependencyInjections.RegisterEnvironments(args);
builder.RegisterDomainServices();
builder.RegisterApplicationServices();
builder.RegisterExternalServices();

builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
    builder.Services.AddSwaggerGen(c =>
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Enity bank assignment ", Version = "v1" }));

var app = builder.Build();


if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Enity bank assignment"));
}

app.Run();