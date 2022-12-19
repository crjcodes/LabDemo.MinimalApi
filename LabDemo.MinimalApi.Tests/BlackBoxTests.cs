using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Shouldly;
using System.Net.Http.Json;
using LabDemo.MinimalApi;
using LabDemo.MinimalApi.Models;

namespace LabDemo.MinimalApi.Tests
{
    internal class LabApiAppForTesting : WebApplicationFactory<Program>
    {
        // TODO: override as necessary for testing any configuration of the builder or the app here
    }

    // CONSIDER: this hits the api with a different mechanism than the actual app.  How effective/needed
    // is this approach vs another?
    //
    // Under the hood, of course, it's the same http GET

    // CONSIDER: how extensive should the integration testing be for this small API?
    // Hint #1: it depends on how the API will be used (critical path, heavy load, security-conscious environment)
    // Hint #2: it depends on the software development approach, e.g. TDD

    // CONSIDER: what is the "unit" under test?  Here, in the integration testing, the "unit" is the API app

    public class BlackBoxTests
    { 
        [Fact]
        public async Task GetLabwork_ShouldRespondOk()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLabwork_ShouldReturnRecords()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords");
            var response = await client.SendAsync(request);

            var listLabwork = await response.Content.ReadFromJsonAsync<List<FlattenedLabRecord>>();

            listLabwork.ShouldNotBeEmpty<FlattenedLabRecord>();
        }

        [Fact]
        public async Task GetLabNames_ShouldRespondOk()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabNames");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLabwork_ShouldRespondNotFound_WhenUnknownPath()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/Foo");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetLabName_ShouldRespondOk_WhenLabNameExists()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/Search?LabName=RBC");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLabName_ShouldReturnList_WhenLabNameExists()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/Search?LabName=RBC");
            var response = await client.SendAsync(request);

            var listLabwork = await response.Content.ReadFromJsonAsync<List<FlattenedLabRecord>>();

            listLabwork.ShouldNotBeEmpty<FlattenedLabRecord>();
        }

        [Fact]
        public async Task GetLabName_ShouldReturnEmptyList_WhenLabNameDoesNotExist()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/Search?LabName=FOO");
            var response = await client.SendAsync(request);

            var listLabwork = await response.Content.ReadFromJsonAsync<List<FlattenedLabRecord>>();

            listLabwork.ShouldBeEmpty<FlattenedLabRecord>();
        }

        [Fact]
        public async Task GetLabName_ShouldRespondNotFound_WhenWrongFormat()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/LabName");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}