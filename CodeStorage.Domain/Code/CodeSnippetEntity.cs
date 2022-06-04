using System;
namespace CodeStorage.Domain.Code
{
    public class CodeSnippedEntity
    {
        public string Id { get; set; } =  Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }
}
