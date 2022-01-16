// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommand : CategoryDto, IRequest<Result<int>>, IMapFrom<Category>, ICacheInvalidator
{
    public string CacheKey => nameof(CreateCategoryCommand);

    public CancellationTokenSource ResetCacheToken => CategoryCacheKey.ResetCacheToken;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CreateCategoryCommand> _localizer;
    public CreateCategoryCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<CreateCategoryCommand> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Category>(request);
        _context.Categories.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(item.Id);
    }
}

