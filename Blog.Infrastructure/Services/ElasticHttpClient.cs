using System.Text;

namespace Blog.Infrastructure.Services;

public class ElasticHttpClient(HttpClient httpClient)
{
    private const string BaseUrl = "http://localhost:9200";

    public async Task<HttpResponseMessage> GetAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{BaseUrl}/{endpoint}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, string jsonBody, CancellationToken cancellationToken)
    {
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{BaseUrl}/{endpoint}", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, string jsonBody, CancellationToken cancellationToken)
    {
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"{BaseUrl}/{endpoint}", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"{BaseUrl}/{endpoint}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return response;
    }
}