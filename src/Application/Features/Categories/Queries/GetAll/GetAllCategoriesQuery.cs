// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;

public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>, ICacheable
{
    public string CacheKey => $"{nameof(GetAllCategoriesQuery)}";

    public MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions()
        .AddExpirationToken(new CancellationChangeToken(CategoryCacheTokenSource.ResetCacheToken.Token));

}

public class GetAllCategoriesQueryHandler :
     IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllCategoriesQueryHandler> _localizer;

    public GetAllCategoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllCategoriesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Categories
                     .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


