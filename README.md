# Purpose

As part of a larger system, this subsystem is a minimal API responsible for reporting data. For demonstrative purposes, the data is mocked from medical bloodwork and comes in through the app configuration via a json file.

The API is a minimal API approach -- no controllers, just straight up querying through the minimal API setup to a few different HTTP GETs.

# How to Run

Just check out the git repository, open the solution in Visual Studio, and run.  A browser window will appear with Swagger:

![Swagger](img/Swagger.png)

# How to Call

1. Ensure the api's url is passed in to the calling app.  For debug, it's easy enough to add to launchSettings:

```"LABWORK_API_URL": "https://localhost:7105"```

1. 1. Follow instructions online to add the OpenApi to the app to call the api
    1. After running the API in Swagger, copy the swagger.json to the calling app
    2. In the Solution Explorer (I'm sure there's a command-line, too), through Connected Services, add the file from #1
2. Then, after NSwag has generated the necessary code from the OpenApi/Swagger json, add code similar to the following:

```
var app = builder.Build();
.
:
var apiUrl = builder.Configuration["LABWORK_API_URL"];
var httpClient = new HttpClient();
var client = new LabClient(apiUrl, httpClient);
var labwork = await client.LabRecordsAsync();
```


# The System

Eventually, the overall system will consist of
1. This API, providing information from database (instead of the current mocked data from json configuration file)
    1. Will include stand-alone repo, ci-cd, testing up to the level of the api as a whole, and a docker container running the API
    1. This will include ci-cd all the way through deployment
1. A front-end UI interactively charting the data from the API, probably in Razor
    1. Will include stand-along repo, ci-cd, testing up to the level of the subystem (maybe using Playwright?), and a docker container running the UI
    1. This will include ci-cd all the way through deployment
1. An integration project solution that will orchestrate the api and UI in docker containers, with the ability to run ecosystem tests

- Each subsystem's project solution will have the ability to be run locally with or without its docker container, from the IDE or the command-line
- The integration project solution will be blocked from production

I chose to separate the api from the ui, instead of the usual minimal API approach of putting everything into the web front-end app -- sse *Separation of Concerns*

## Tech Used

- .NET 7
- OpenApi
- Minimal API

(not a TDD project)

Note this API started off with the basic scaffolding/templates via dotnet/Visual Studio:

*Basic Template for ASP.NET Core Web Api without Docker Support (Yet)*

![Original Template](img/InitialDotNetTemplate.png)

*Minimal Dependencies But with OpenApi*

![Minimal Dependences](img/MinimalDependencies.png)

## Details

### OpenAPI

Note that the swagger.json is currently manually copied to the UI project when the API changest. 

There are [ways](https://techcommunity.microsoft.com/t5/healthcare-and-life-sciences/auto-regenerating-api-client-for-your-open-api-project/ba-p/3302390) to automate this.

### Design

<mark>The design covers not only the actual apps -- the ui and api -- **BUT ALSO** the structure of the **repo**sitory, the **building**, the **testing**, the **publishing**, the **monitoring**.</mark>

See "The System" above.

## TESTING

Currently, only "integration" tests exist for the API, and no tests exist for the bare-bones UI.  

Some possible goals for testing

1. Setup project-level and solution-level stages to run locally, debug and production
1. Add Playwright or something similar to automate UI testing
1. Integration tests that spin up the app and ui together, driving UI through Playwright or something similar

### CI-CD

1. Investigate GitHub for support of ci-cd stages
1. Develop a strategy for ci-cd stages locally and deployed

### DEPLOYMENT

Currently, I've been running these only in debug, locally.  I do have a free Azure account that I may be able to deploy to.

### MONITORING

Explore Azure possibilities?

### Separation of Concerns

#### No to API+UI in the Same App

Combining api calls, even minimal api calls, with the ui is not a favored approach of mine under most circumstances. If a really simple case, sure, but in most cases the api and the ui are going to have to scale and are going to be live for a length of time long enough to consider maintainability in the design.

If this was the real world, a better choice would be to have the API and UI sitting in different solutions and deployed separately.

