using AutoMapper;
using CodeStorage.Application.Common;
using CodeStorage.Domain.Code;
using CodeStorage.Domain.User;
using MediatR;

namespace CodeStorage.Application.CodeSnipped;



public record CreateCodeSnippedCommand : IRequest<CodeSnippedEntityDto>, IMapTo<CodeSnippedEntity>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();
    public string UserId { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
    public DateTime InsertDate { get; set; }
    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}

public class CreateCodeSnippedCommandHandler : IRequestHandler<CreateCodeSnippedCommand, CodeSnippedEntityDto>
{
    private readonly ICodeRepository _codeRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public CreateCodeSnippedCommandHandler(ICodeRepository codeRepository, IMapper mapper, IUserService userService)
    {
        _codeRepository = codeRepository;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<CodeSnippedEntityDto> Handle(CreateCodeSnippedCommand request, CancellationToken cancellationToken)
    {
        var codeSnipped = _mapper.Map<CodeSnippedEntity>(request);
        codeSnipped.UserId = _userService.GetUserId();
        codeSnipped.LastUpdateDate = DateTime.Now;
        codeSnipped.Id = await _codeRepository.InsertCodeSnipped(codeSnipped);
        codeSnipped.InsertDate = DateTime.Now;
        return _mapper.Map<CodeSnippedEntityDto>(codeSnipped);
    }
}
