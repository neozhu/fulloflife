// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Import;

public class ImportCategoriesCommand : IRequest<Result>, ICacheInvalidator
{
    public ImportCategoriesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => nameof(ImportCategoriesCommand);

    public CancellationTokenSource ResetCacheToken => CategoryCacheTokenSource.ResetCacheToken;
}
public class CreateCategoriesTemplateCommand : IRequest<byte[]>
{
    public CreateCategoriesTemplateCommand(IEnumerable<string> fields, string sheetName = nameof(Category))
    {
        Fields = fields;
        SheetName = sheetName;
    }
    public IEnumerable<string> Fields { get; set; }
    public string SheetName { get; set; }

}

public class ImportCategoriesCommandHandler :
             IRequestHandler<CreateCategoriesTemplateCommand, byte[]>,
             IRequestHandler<ImportCategoriesCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ImportCategoriesCommandHandler> _localizer;
    private readonly IExcelService _excelService;

    public ImportCategoriesCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportCategoriesCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ImportCategoriesCommand request, CancellationToken cancellationToken)
    {
        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, CategoryDto, object>>
        {
            { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },
            { _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]]?.ToString() },
            { _localizer["Icon"], (row,item) => item.Icon = row[_localizer["Icon"]]?.ToString() },
            { _localizer["Sequence"], (row,item) => item.Sequence =Convert.ToInt32(row[_localizer["Sequence"]]?.ToString()) },

        }, _localizer[nameof(Category)]);
        throw new System.NotImplementedException();
    }
    public async Task<byte[]> Handle(CreateCategoriesTemplateCommand request, CancellationToken cancellationToken)
    {

        var fields = new string[] {
                     _localizer["Sequence"],
                     _localizer["Name"],
                     _localizer["Description"],
                     _localizer["Icon"]
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[nameof(Category)]);
        return result;
    }
}

