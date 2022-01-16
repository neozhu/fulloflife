// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommand : CategoryDto, IRequest<Result>, IMapFrom<Category>, ICacheInvalidator
{
    public string CacheKey => nameof(UpdateCategoryCommand);

    public CancellationTokenSource ResetCacheToken => CategoryCacheKey.ResetCacheToken;
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<UpdateCategoryCommandHandler> _localizer;
    public UpdateCategoryCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<UpdateCategoryCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item != null)
        {
            item = _mapper.Map(request, item);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}

