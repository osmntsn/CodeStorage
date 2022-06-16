
using AutoMapper;
using CodeStorage.Domain.Code;
using MediatR;

namespace CodeStorage.Application.CodeSnipped;

public class GetCodeSnippedQuery : IRequest<CodeSnippedEntityDto>
{
    public string Id { get; set; } = string.Empty;
}

public class GetCodeSnippedQueryHandler : IRequestHandler<GetCodeSnippedQuery, CodeSnippedEntityDto>
{
    private readonly IMapper _mapper;
    private readonly ICodeRepository _codeRepository;

    public GetCodeSnippedQueryHandler(IMapper mapper, ICodeRepository codeRepository)
    {
        _mapper = mapper;
        _codeRepository = codeRepository;
    }

    public async Task<CodeSnippedEntityDto> Handle(GetCodeSnippedQuery request, CancellationToken cancellationToken)
    {
        var result = await _codeRepository.GetCodeSnipped(request.Id);
        return _mapper.Map<CodeSnippedEntityDto>(result); 
    }
}