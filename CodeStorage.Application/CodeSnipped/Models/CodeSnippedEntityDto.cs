using CodeStorage.Application.Common;
using CodeStorage.Domain.Code;

namespace CodeStorage.Application.CodeSnipped;

public class CodeSnippedEntityDto : IMapFrom<CodeSnippedEntity>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();
    public string UserId { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
    public DateTime InsertDate { get; set; }
    public string LastUpdateDate { get; set; } = string.Empty;
}