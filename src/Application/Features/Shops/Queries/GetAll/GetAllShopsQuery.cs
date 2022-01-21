// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Queries.GetAll;

public class GetAllShopsQuery : IRequest<IEnumerable<ShopDto>>
{

}

public class GetAllShopsQueryHandler :
     IRequestHandler<GetAllShopsQuery, IEnumerable<ShopDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllShopsQueryHandler> _localizer;

    public GetAllShopsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllShopsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<ShopDto>> Handle(GetAllShopsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Shops
                     .ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


