// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Queries.GetAll;

public class GetAllAdvertisingsQuery : IRequest<IEnumerable<AdvertisingDto>>
{

}

public class GetAllAdvertisingsQueryHandler :
     IRequestHandler<GetAllAdvertisingsQuery, IEnumerable<AdvertisingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllAdvertisingsQueryHandler> _localizer;

    public GetAllAdvertisingsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllAdvertisingsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<AdvertisingDto>> Handle(GetAllAdvertisingsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Advertisings
                     .ProjectTo<AdvertisingDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


