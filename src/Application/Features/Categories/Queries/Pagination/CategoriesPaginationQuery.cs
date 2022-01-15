// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CleanArchitecture.Razor.Application.Features.Categories.Queries.Pagination;

public class CategoriesWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<CategoryDto>>, ICacheable
{
    public string CacheKey => $"{nameof(CategoriesWithPaginationQuery)},{this.ToString()}";

    public MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions()
        .AddExpirationToken(new CancellationChangeToken(CategoryCacheTokenSource.ResetCacheToken.Token));

}

public class CategoriesWithPaginationQueryHandler :
         IRequestHandler<CategoriesWithPaginationQuery, PaginatedData<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CategoriesWithPaginationQueryHandler> _localizer;

    public CategoriesWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<CategoriesWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<CategoryDto>> Handle(CategoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var filters = PredicateBuilder.FromFilter<Category>(request.FilterRules);
        var data = await _context.Categories.Where(filters)
             .OrderBy($"{request.Sort} {request.Order}")
             .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.Page, request.Rows);
        return data;
    }
}
