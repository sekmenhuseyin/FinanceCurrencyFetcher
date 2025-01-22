using Newtonsoft.Json;

namespace CachedFinanceData;

public class CarTax
{
    public static async Task<string> GetData(IHttpClientFactory httpClientFactory, IFusionCache cache)
    {
        var data = await cache.GetOrSetAsync<string>(
            "CarTaxData",
            _ => Pull(),
            TimeSpan.FromDays(1)
        );
        return data;

        async Task<string> Pull()
        {
            var model = new
            {
                cmd = "request.get",
                url = "https://www.hesapkurdu.com/tasit-kredisi/h/arac-otv-kdv-hesaplama",
                maxTimeout = 60000
            };
            var cancellationToken = new CancellationTokenSource().Token;
            var response = await httpClientFactory.CreateClient()
                .PostAsJsonAsync("https://cached-data-flaresolverr.tv3e2g.easypanel.host/v1", model, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var json = JsonConvert.DeserializeObject<JsonData>(content)!;
            return json.Solution.Response;

        }
    }
    public class JsonData
    {
        public Solution Solution { get; set; } = new();
    }

    public class Solution
    {
        public string Response { get; set; } = "";
    }
}