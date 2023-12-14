using Microsoft.Extensions.DependencyInjection;
using Cursed.GraphQL;

namespace Cursed;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello");

        var servicesCollection = new ServiceCollection();
        servicesCollection.AddStratzClient()
            .ConfigureHttpClient(
                client =>
                {
                    client.BaseAddress = new Uri("https://api.stratz.com/graphql");
                    client.DefaultRequestHeaders.Add(
                        "Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJodHRwczovL3N0ZWFtY29tbXVuaXR5LmNvbS9vcGVuaWQvaWQvNzY1NjExOTg5ODUxODc3ODkiLCJ1bmlxdWVfbmFtZSI6IkwnZW5mZXIgYydlc3QgbGVzIGF1dHJlcyIsIlN1YmplY3QiOiJiYjhjMDVlNS01YjVlLTQxMmUtYmQxNy1iYTIwZjg5NTdmYmUiLCJTdGVhbUlkIjoiMTAyNDkyMjA2MSIsIm5iZiI6MTY4MjU3NzIxNCwiZXhwIjoxNzE0MTEzMjE0LCJpYXQiOjE2ODI1NzcyMTQsImlzcyI6Imh0dHBzOi8vYXBpLnN0cmF0ei5jb20ifQ.C28zroRLVnRixxggNgSCbTwqqyAvFJ5yiMaZlUiP8YY"
                        );
                }
            );

        IServiceProvider services = servicesCollection.BuildServiceProvider();
        IStratzClient client = services.GetRequiredService<IStratzClient>();
        var result = await client.GetHeroStats.ExecuteAsync();
        if (result.Data == null)
        {
            return;
        }
        foreach (var vs in result.Data.HeroStats.HeroVsHeroMatchup.Advantage.SelectMany(x => x.Vs))
        {
            Console.WriteLine($"{vs.HeroId1} VS {vs.HeroId2} AVERAGE {vs.WinsAverage}");
        }
    }
}

// services.AddStratz