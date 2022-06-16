using CodeStorage.Domain.Code;
using Microsoft.Extensions.Configuration;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elastic.Transport.Products.Elasticsearch;

namespace CodeStorage.Infrastructure.Data;

public abstract class ElasticsearchRepositoryBase
{
    private readonly ElasticsearchConfiguration _elasticsearchConfiguration;

    protected readonly ElasticsearchClient _elasticClient;

    protected ElasticsearchRepositoryBase(IConfiguration configuration)
    {
        _elasticsearchConfiguration = new ElasticsearchConfiguration
        {
            UserName = configuration.GetSection("Elasticsearch:UserName").Value,
            Url = configuration.GetSection("Elasticsearch:Url").Value,
            Password = configuration.GetSection("Elasticsearch:Password").Value,
        };

        var connectionConf = new ElasticsearchClientSettings(new Uri(_elasticsearchConfiguration.Url))
        .CertificateFingerprint("F5:05:61:90:8B:91:F5:B3:0C:41:F6:E9:3A:BB:63:FF:8E:59:BB:1D:22:D7:BB:F0:B5:20:7B:8D:88:FB:6E:A0")
        //.BasicAuthentication(_elasticsearchConfiguration.UserName, _elasticsearchConfiguration.Password)
        .ServerCertificateValidationCallback((a, b, c, d) => { return true; })
        .DefaultMappingFor<CodeSnippedEntity>(x => x.IndexName("codestorage"))
        .Authentication(new BasicAuthentication(_elasticsearchConfiguration.UserName, _elasticsearchConfiguration.Password)); ;

        _elasticClient = new ElasticsearchClient(connectionConf);
    }

    protected void ControlElasticsearchResult(ElasticsearchResponseBase response)
    {
        if (response.OriginalException != null)
            throw response.OriginalException;
    }


    internal class ElasticsearchConfiguration
    {
        public string Url { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}