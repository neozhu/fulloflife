// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Queries.Pagination;

public class AdvertisingsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<AdvertisingDto>>
{

}

public class AdvertisingsWithPaginationQueryHandler :
     IRequestHandler<AdvertisingsWithPaginationQuery, PaginatedData<AdvertisingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AdvertisingsWithPaginationQueryHandler> _localizer;

    public AdvertisingsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<AdvertisingsWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<AdvertisingDto>> Handle(AdvertisingsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var filters = PredicateBuilder.FromFilter<Advertising>(request.FilterRules);
        var data = await _context.Advertisings.Where(filters)
             .OrderBy($"{request.Sort} {request.Order}")
             .ProjectTo<AdvertisingDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.Page, request.Rows);
        return data;
    }
}
