using CodeStorage.Domain.Code;
using Microsoft.Extensions.Configuration;

namespace CodeStorage.Infrastructure.Data;

public class CodeRepository : ElasticsearchRepositoryBase, ICodeRepository
{
    public CodeRepository(IConfiguration configuration) : base(configuration)
    {

    }

    public async Task<CodeSnippedEntity?> GetCodeSnipped(string id)
    {
        var result = await _elasticClient.GetAsync<CodeSnippedEntity>(id);
        ControlElasticsearchResult(result);
        return result.Source;
    }

    public async Task<string> InsertCodeSnipped(CodeSnippedEntity codeSnipped)
    {
        var result = await _elasticClient.IndexAsync(codeSnipped);
        ControlElasticsearchResult(result);
        return result.Id;
    }
}