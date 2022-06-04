namespace CodeStorage.Domain.Code;

public interface ICodeRepository
{
    string InsertCodeSnipped(CodeSnippedEntity codeSnipped);
    CodeSnippedEntity GetCodeSnipped(string id);

}