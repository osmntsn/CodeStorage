namespace CodeStorage.Domain.Code;

public interface ICodeRepository
{
    Task<string> InsertCodeSnipped(CodeSnippedEntity codeSnipped);
    Task<CodeSnippedEntity?> GetCodeSnipped(string id);

}