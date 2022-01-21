// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Queries.Export;

public class ExportShopsQuery : IRequest<byte[]>
{
    public string? FilterRules { get; set; }
    public string Sort { get; set; } = "Id";
    public string Order { get; set; } = "desc";
}

public class ExportShopsQueryHandler :
     IRequestHandler<ExportShopsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportShopsQueryHandler> _localizer;

    public ExportShopsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IExcelService excelService,
        IStringLocalizer<ExportShopsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _excelService = excelService;
        _localizer = localizer;
    }

    public async Task<byte[]> Handle(ExportShopsQuery request, CancellationToken cancellationToken)
    {
        var filters = PredicateBuilder.FromFilter<Shop>(request.FilterRules);
        var data = await _context.Shops.Where(filters)
                   .OrderBy($"{request.Sort} {request.Order}")
                   .ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<ShopDto, object>>()
            {
               { _localizer["Name"], item => item.Name },
               { _localizer["Contact Name"], item => item.ContactName },
               { _localizer["Contact Phone"], item => item.ContactPhone },
               { _localizer["Address"], item => item.Address },
               { _localizer["Delivery Distance"], item => item.DeliveryDistance },
               { _localizer["Min Charge"], item => item.MinCharge },
               { _localizer["Icon"], item => item.Icon },
               { _localizer["Latitude"], item => item.Latitude },
               { _localizer["Longitude"], item => item.Longitude },
            }
            , _localizer["Shops"]);
        return result;
    }
}
