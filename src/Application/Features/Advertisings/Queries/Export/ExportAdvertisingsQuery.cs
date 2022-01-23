// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Queries.Export;

public class ExportAdvertisingsQuery : IRequest<byte[]>
{
    public string FilterRules { get; set; }
    public string Sort { get; set; } = "Id";
    public string Order { get; set; } = "desc";
}

public class ExportAdvertisingsQueryHandler :
     IRequestHandler<ExportAdvertisingsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportAdvertisingsQueryHandler> _localizer;

    public ExportAdvertisingsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IExcelService excelService,
        IStringLocalizer<ExportAdvertisingsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _excelService = excelService;
        _localizer = localizer;
    }

    public async Task<byte[]> Handle(ExportAdvertisingsQuery request, CancellationToken cancellationToken)
    {

        var filters = PredicateBuilder.FromFilter<Advertising>(request.FilterRules);
        var data = await _context.Advertisings.Where(filters)
                   .OrderBy($"{request.Sort} {request.Order}")
                   .ProjectTo<AdvertisingDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<AdvertisingDto, object>>()
            {
                { _localizer["Category"], item => item.Category },
                { _localizer["Content"], item => item.Content },
                { _localizer["Picture"], item => item.Picture },
                { _localizer["Title"], item => item.Title },
                { _localizer["Relevant Product Id"], item => item.RelevantProductId },
                { _localizer["ExpiredDate"], item => item.ExpiredDate },
            }
            , _localizer["Advertisings"]);
        return result;
    }
}
