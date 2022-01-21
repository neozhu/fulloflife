// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Queries.Pagination;

public class ShopsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ShopDto>>
{

}

public class ShopsWithPaginationQueryHandler :
     IRequestHandler<ShopsWithPaginationQuery, PaginatedData<ShopDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ShopsWithPaginationQueryHandler> _localizer;

    public ShopsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<ShopsWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<ShopDto>> Handle(ShopsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var filters = PredicateBuilder.FromFilter<Shop>(request.FilterRules);
        var data = await _context.Shops.Where(filters)
             .OrderBy($"{request.Sort} {request.Order}")
             .ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.Page, request.Rows);
        return data;
    }
}
