using System.Collections.Immutable;
using FundDataApi.Data;
using HtmlAgilityPack;
using MediatR;

namespace FundDataApi.Services.HistoricalData;

public class Sp500ConstituentsQueryHandler() : IRequestHandler<Sp500ConstituentsQuery, IImmutableList<string>>
{
    public async Task<IImmutableList<string>> Handle(Sp500ConstituentsQuery query, CancellationToken cancellationToken)
    {
        var url = "https://en.wikipedia.org/wiki/List_of_S%26P_500_companies#S&P_500_component_stocks";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

        var web = new HtmlWeb { UserAgent = userAgent };
        var page = await web.LoadFromWebAsync(url);

        var table = page.DocumentNode.SelectSingleNode("//table[@class='wikitable sortable' and @id='constituents']");

        if (table == null)
        {
            throw new Exception("Could not load SP 500 Table");
        }

        return table.SelectNodes(".//tr")
                    .Skip(1) // header
                    .Select(x => x.SelectNodes("td | th").First().InnerText.Trim())
                    .ToImmutableList();
    }
}
