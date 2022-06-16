using CodeStorage.Application.CodeSnipped;
using Microsoft.AspNetCore.Mvc;

namespace CodeStorage.API.Controllers;

public class CodeSnippedsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<CodeSnippedEntityDto> Get([FromRoute]string id)
    {
        var result = await Mediator.Send(new GetCodeSnippedQuery{ Id = id});
        return result;
    }

    [HttpPost]
    public async Task<CodeSnippedEntityDto> Create([FromBody]CreateCodeSnippedCommand command)
    {
        var result = await Mediator.Send(command);
        return result;
    }
}