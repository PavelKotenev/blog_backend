using System.Text;

namespace Blog.Infrastructure.Services;

public class ElasticHttpClient
{
    private const string EsHostPort = "http://localhost:9200";
    private readonly HttpClient _http = new();

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        var response = await _http.GetAsync($"{EsHostPort}/{endpoint}");
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, string jsonBody)
    {
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"{EsHostPort}/{endpoint}", content);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, string jsonBody)
    {
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _http.PutAsync($"{EsHostPort}/{endpoint}", content);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        var response = await _http.DeleteAsync($"{EsHostPort}/{endpoint}");
        response.EnsureSuccessStatusCode();
        return response;
    }
}