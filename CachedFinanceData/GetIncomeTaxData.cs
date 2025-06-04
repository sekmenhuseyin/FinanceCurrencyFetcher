using Newtonsoft.Json;

namespace CachedFinanceData;

public class GetIncomeTaxData
{
    public static async Task<string> GetData(IHttpClientFactory httpClientFactory, IFusionCache cache)
    {
        var data = await cache.GetOrSetAsync<string>(
            "GetIncomeTaxData",
            _ => Pull(),
            TimeSpan.FromDays(1)
        );
        return data;

        async Task<string> Pull()
        {
            var model = new
            {
                cmd = "request.get",
                url = "https://www.verginet.net/dtt/1/GelirVergisiTarifesi_3804.aspx",
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