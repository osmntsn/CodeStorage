using AutoMapper;
using CodeStorage.Application.Common;
using CodeStorage.Domain.Code;
using MediatR;

namespace CodeStorage.Application.CodeSnipped;



public record CreateCodeSnippedCommand : IRequest<CodeSnippedEntityDto>, IMapTo<CodeSnippedEntity>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();
}

public class CreateCodeSnippedCommandHandler : IRequestHandler<CreateCodeSnippedCommand, CodeSnippedEntityDto>
{
    private readonly ICodeRepository _codeRepository;
    private readonly IMapper _mapper;

    public CreateCodeSnippedCommandHandler(ICodeRepository codeRepository, IMapper mapper)
    {
        _codeRepository = codeRepository;
        _mapper = mapper;
    }

    public async Task<CodeSnippedEntityDto> Handle(CreateCodeSnippedCommand request, CancellationToken cancellationToken)
    {
        var codeSnipped = _mapper.Map<CodeSnippedEntity>(request);
        codeSnipped.Id = await _codeRepository.InsertCodeSnipped(codeSnipped);
        return _mapper.Map<CodeSnippedEntityDto>(codeSnipped);
    }
}
