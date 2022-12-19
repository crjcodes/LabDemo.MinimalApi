using LabDemo.MinimalApi.Models;


//========================================================================
// BUILDER ACTIONS

var builder = WebApplication.CreateBuilder(args);

// Load into the configuration the mock data

// For this simple example, this approach works
// In the real world, the data would be more voluminous and coming from
// a database read, most likely

builder.Configuration.AddJsonFile("mockdata.json");

//========================================================================
// BUILDER ACTIONS - SERVICE SETUP

// if we were to move to a service, we would need to add logic here to inject
// the mock data configuration using a pattern like Options/Configure<List<FlattendLab...
// In the real world, patterns commonly used are a dbContext or a repo or even a data access service

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//========================================================================
// APP SETUP
//
// Instead of this more traditional controllers approach use the minimal APIs instead

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// FUTURE: determine how to setup redirection properly for testing to pass in the CI pipeline
// (see the github action runners)
//app.UseHttpsRedirection();


// In the real world, this would be coming from a service -- data access, dbContext, or repo, etc

var records = builder.Configuration.GetSection("LabRecords").Get<List<FlattenedLabRecord>>();

// API ROUTING

//app.MapGet("/", () => { return "Hello"; });
app.MapGet("/", () => { return records; });

app.MapGet("/LabRecords", () => { return records; })
//    .WithName("LabRecordss")
    .WithOpenApi();

app.MapGet("/LabNames", () => { return records?.Select(r => r.Name).Distinct(); })
//    .WithName("LabNames")
    .WithOpenApi();

app.MapGet("/LabRecords/Search", (string LabName) =>
{
    return records?.Where(r => r.Name.ToUpper() == LabName.ToUpper());
})
//    .WithName("SearchLabNames")
    .WithOpenApi();

// REFERENCE
// If a need exists to bind complex parameters, see https://code-maze.com/aspnetcore-query-string-parameters-minimal-apis/

app.Run();


