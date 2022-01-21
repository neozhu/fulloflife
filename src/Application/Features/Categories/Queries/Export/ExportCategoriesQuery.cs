// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.Features.Categories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Categories.Queries.Export;

public class ExportCategoriesQuery : IRequest<byte[]>
{
    public string FilterRules { get; set; }
    public string Sort { get; set; } = "Id";
    public string Order { get; set; } = "desc";
}

public class ExportCategoriesQueryHandler :
     IRequestHandler<ExportCategoriesQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportCategoriesQueryHandler> _localizer;

    public ExportCategoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IExcelService excelService,
        IStringLocalizer<ExportCategoriesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _excelService = excelService;
        _localizer = localizer;
    }

    public async Task<byte[]> Handle(ExportCategoriesQuery request, CancellationToken cancellationToken)
    {

        var filters = PredicateBuilder.FromFilter<Category>(request.FilterRules);
        var data = await _context.Categories.Where(filters)
                   .OrderBy($"{request.Sort} {request.Order}")
                   .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<CategoryDto, object>>()
            {
                 { _localizer["Name"], item => item.Name },
                 { _localizer["Description"], item => item.Description },
                 { _localizer["Sort"], item => item.Sequence },
                 { _localizer["Icon"], item => item.Icon },
            }
            , _localizer["Categories"]);
        return result;
    }
}
