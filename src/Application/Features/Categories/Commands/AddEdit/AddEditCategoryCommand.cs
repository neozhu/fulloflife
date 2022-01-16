// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.AddEdit;

public class AddEditCategoryCommand : CategoryDto, IRequest<Result<int>>, IMapFrom<Category>, ICacheInvalidator
{
    public string CacheKey => nameof(AddEditCategoryCommand);

    public CancellationTokenSource ResetCacheToken => CategoryCacheKey.ResetCacheToken;
}

public class AddEditCategoryCommandHandler : IRequestHandler<AddEditCategoryCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditCategoryCommandHandler> _localizer;
    public AddEditCategoryCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditCategoryCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
            item = _mapper.Map(request, item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Category>(request);
            _context.Categories.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

