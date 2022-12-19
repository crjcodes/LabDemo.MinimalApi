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
    }

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
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/LabName?LabName=RBC");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLabName_ShouldReturnList_WhenLabNameExists()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/LabName?LabName=RBC");
            var response = await client.SendAsync(request);

            var listLabwork = await response.Content.ReadFromJsonAsync<List<FlattenedLabRecord>>();

            listLabwork.ShouldNotBeEmpty<FlattenedLabRecord>();
        }

        [Fact]
        public async Task GetLabName_ShouldReturnEmptyList_WhenLabNameDoesNotExist()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/LabName?LabName=FOO");
            var response = await client.SendAsync(request);

            var listLabwork = await response.Content.ReadFromJsonAsync<List<FlattenedLabRecord>>();

            listLabwork.ShouldBeEmpty<FlattenedLabRecord>();
        }

        [Fact]
        public async Task GetLabName_ShouldRespondBadRequest_WhenNoLabNameGiven()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords/LabName");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetLabName_ShouldRespondOk_WhenDirectLabNameExists()
        {
            var app = new LabApiAppForTesting();
            var client = app.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/LabRecords?LabName=RBC");
            var response = await client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}