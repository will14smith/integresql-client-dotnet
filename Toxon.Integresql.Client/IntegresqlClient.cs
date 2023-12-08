using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Toxon.Integresql.Client.Exceptions;
using Toxon.Integresql.Client.Models;
using Toxon.Integresql.Client.RequestModels;

namespace Toxon.Integresql.Client;

public class IntegresqlClient : IIntegresqlTemplateClient, IIntegresqlDatabaseClient
{
    private readonly HttpClient _httpClient;
    private readonly Uri _baseUri;

    public IntegresqlClient(Uri baseUri, HttpMessageHandler? httpMessageHandler = null)
    {
        _baseUri = baseUri;
        _httpClient = new HttpClient(httpMessageHandler);
        _httpClient.BaseAddress = _baseUri;
    }
    
    #region Templates
    public async Task<TemplateDatabase> InitializeTemplate(string hash)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUri.AbsoluteUri}/templates");
        request.Content = SerializeRequest(new InitializeTemplateRequest
        {
            Hash = hash
        }, SourceGenerationContext.Default.InitializeTemplateRequest);
        
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var responseModel = await DeserializeResponse(responseContent, SourceGenerationContext.Default.InitializeTemplateResponse).ConfigureAwait(false);
                return responseModel.ToModel();
            
            // case HttpStatusCode.Locked:
            case (HttpStatusCode) 423:
                throw new TemplateAlreadyInitializedException();
            
            case HttpStatusCode.ServiceUnavailable:
                throw new InternalErrorException("manager not ready");
            
            default:
                throw new InternalErrorException($"unexpected http status code {response.StatusCode}");
        }
    }
    
    public async Task FinalizeTemplate(TemplateDatabase database)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUri.AbsoluteUri}/templates/{database.Hash}");
     
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return;

            case HttpStatusCode.NotFound:
                throw new TemplateNotFoundException();
            
            case HttpStatusCode.ServiceUnavailable:
                throw new InternalErrorException("manager not ready");
            
            default:
                throw new InternalErrorException($"unexpected http status code {response.StatusCode}");
        }    }

    public async Task DiscardTemplate(TemplateDatabase database)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUri.AbsoluteUri}/templates/{database.Hash}");
     
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return;

            case HttpStatusCode.NotFound:
                throw new TemplateNotFoundException();
            
            case HttpStatusCode.ServiceUnavailable:
                throw new InternalErrorException("manager not ready");
            
            default:
                throw new InternalErrorException($"unexpected http status code {response.StatusCode}");
        }
    }
    #endregion

    #region Databases
    public async Task<TestDatabase> GetDatabase(string hash)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUri.AbsoluteUri}/templates/{hash}/tests");
        
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var responseModel = await DeserializeResponse(responseContent, SourceGenerationContext.Default.GetDatabaseResponse).ConfigureAwait(false);
                return responseModel.ToModel();
            
            case HttpStatusCode.NotFound:
                throw new TemplateNotFoundException();

            case HttpStatusCode.Gone:
                throw new DatabaseDiscardedException();
            
            case HttpStatusCode.ServiceUnavailable:
                throw new InternalErrorException("manager not ready");
            
            default:
                throw new InternalErrorException($"unexpected http status code {response.StatusCode}");
        }
    }

    public async Task ReturnDatabase(TestDatabase database)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUri.AbsoluteUri}/templates/{database.Hash}/tests/{database.Id}");
        
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return;
            
            case HttpStatusCode.NotFound:
                throw new TemplateNotFoundException();
            
            case HttpStatusCode.ServiceUnavailable:
                throw new InternalErrorException("manager not ready");
            
            default:
                throw new InternalErrorException($"unexpected http status code {response.StatusCode}");
        }    }
    #endregion
    
    private static StringContent SerializeRequest<TRequest>(TRequest request, JsonTypeInfo<TRequest> jsonTypeInfo) =>
        new(JsonSerializer.Serialize(request, jsonTypeInfo), Encoding.UTF8, "application/json");
    
    private async Task<TModel> DeserializeResponse<TModel>(Stream responseStream, JsonTypeInfo<TModel> jsonTypeInfo) => await JsonSerializer.DeserializeAsync(responseStream, jsonTypeInfo).ConfigureAwait(false) ?? throw new InternalErrorException("failed to deserialize response");
}