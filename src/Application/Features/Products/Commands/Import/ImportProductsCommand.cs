// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Text.Json;
using CleanArchitecture.Razor.Application.Features.Products.DTOs;
using static CleanArchitecture.Razor.Domain.Entities.Product;

namespace CleanArchitecture.Razor.Application.Features.Products.Commands.Import;

public class ImportProductsCommand : IRequest<Result>
{
    public string? FileName { get; set; }
    public byte[] Data { get; set; }
}
public class CreateProductsTemplateCommand : IRequest<byte[]>
{
    public IEnumerable<string> Fields { get; set; } = Array.Empty<string>();
    public string SheetName { get; set; } = "Sheet1";
}

public class ImportProductsCommandHandler :
             IRequestHandler<CreateProductsTemplateCommand, byte[]>,
             IRequestHandler<ImportProductsCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ImportProductsCommandHandler> _localizer;
    private readonly IExcelService _excelService;

    public ImportProductsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportProductsCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ImportProductsCommand request, CancellationToken cancellationToken)
    {
        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ProductDto, object>>
        {
            { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },
            { _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]]?.ToString() },
            { _localizer["Remark"], (row,item) => item.Remark = row[_localizer["Remark"]]?.ToString() },
            { _localizer["Category Name"], (row,item) => item.CategoryName = row[_localizer["Category Name"]]?.ToString() },
            { _localizer["Unit"], (row,item) => item.Unit = row[_localizer["Unit"]]?.ToString() },
            { _localizer["Sort"], (row,item) => item.Sort =Convert.ToInt32(row[_localizer["Sort"]]?.ToString()) },
            { _localizer["Is New"], (row,item) => item.IsNew =Convert.ToBoolean(row[_localizer["Is New"]]?.ToString()) },
            { _localizer["Is Enable"], (row,item) => item.IsEnable =Convert.ToBoolean(row[_localizer["Is Enable"]]?.ToString()) },
            { _localizer["Is Single"], (row,item) => item.IsSingle =Convert.ToBoolean(row[_localizer["Is Single"]]?.ToString()) },
            { _localizer["Labels"], (row,item) => item.Labels =row[_localizer["Labels"]]?.ToString() },
            { _localizer["Price"], (row,item) => item.Price =Convert.ToDecimal(row[_localizer["Price"]]?.ToString()) },
            { _localizer["Cost"], (row,item) => item.Cost =Convert.ToDecimal(row[_localizer["Name"]]?.ToString()) },
            { _localizer["StockQty"], (row,item) => item.StockQty =Convert.ToInt32(row[_localizer["Name"]]?.ToString()) },
            { _localizer["SalesQty"], (row,item) => item.SalesQty =Convert.ToInt32(row[_localizer["Name"]]?.ToString()) },
            { _localizer["Images"], (row,item) => item.Images =row[_localizer["Images"]]?.ToString() },
            { _localizer["Small Images"], (row,item) => item.SmallImages =row[_localizer["Small Images"]]?.ToString() },
            { _localizer["Options"], (row,item) => item.Options = JsonSerializer.Deserialize<Dictionary<string,IList<SKU>?>>(row[_localizer["Options"]]?.ToString()) },

        }, _localizer[nameof(Product)]);
        return Result.Success();
    }
    public async Task<byte[]> Handle(CreateProductsTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                  _localizer["Name"],
                  _localizer["Description"],
                  _localizer["Remark"],
                  _localizer["Category Name"],
                  _localizer["Unit"],
                  _localizer["Sort"],
                  _localizer["Is New"],
                  _localizer["Is Enable"],
                  _localizer["Is Single"],
                  _localizer["Options"],
                  _localizer["Labels"],
                  _localizer["Price"],
                  _localizer["Cost"],
                  _localizer["Stock Qty"],
                  _localizer["Sales Qty"],
                  _localizer["Images"],
                  _localizer["Small Images"],
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[nameof(Product)]);
        return result;
    }
}

