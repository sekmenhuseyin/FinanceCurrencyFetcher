namespace CachedFinanceData;

public class Finance
{
    public static async Task<string> GetData(IHttpClientFactory httpClientFactory, IFusionCache cache)
    {
        var data = await cache.GetOrSetAsync<string>(
            "FinanceCurrencyData",
            _ => Pull(),
            TimeSpan.FromHours(1)
        );
        return data;

        async Task<string> Pull()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var response = await httpClientFactory.CreateClient()
                .GetAsync("https://" + "finans.truncgil.com/devextreme-datasource.php", cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content;

        }
    }
}