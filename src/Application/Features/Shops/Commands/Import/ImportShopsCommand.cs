// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Import;

public class ImportShopsCommand : IRequest<Result>
{
    public ImportShopsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
    public string FileName { get; set; }
    public byte[] Data { get; set; }
}
public class CreateShopsTemplateCommand : IRequest<byte[]>
{
    public IEnumerable<string> Fields { get; set; }
    public string SheetName { get; set; }
}

public class ImportShopsCommandHandler :
             IRequestHandler<CreateShopsTemplateCommand, byte[]>,
             IRequestHandler<ImportShopsCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ImportShopsCommandHandler> _localizer;
    private readonly IExcelService _excelService;

    public ImportShopsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportShopsCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ImportShopsCommand request, CancellationToken cancellationToken)
    {
        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ShopDto, object>>
        {
            { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },
            { _localizer["Contact Name"], (row,item) => item.ContactName = row[_localizer["Contact Name"]]?.ToString() },
            { _localizer["Contact Phone"], (row,item) => item.ContactPhone = row[_localizer["Contact Phone"]]?.ToString() },
            { _localizer["Address"], (row,item) => item.Address = row[_localizer["Address"]]?.ToString() },
            { _localizer["Delivery Distance"], (row,item) => item.DeliveryDistance =Convert.ToDecimal(row[_localizer["Delivery Distance"]]??3000m) },
            { _localizer["Min Charge"], (row,item) => item.MinCharge =Convert.ToDecimal(row[_localizer["Min Charge"]]??15m) },
            { _localizer["Icon"], (row,item) => item.Icon = row[_localizer["Icon"]]?.ToString() },
            { _localizer["Latitude"], (row,item) => item.Latitude =Convert.ToDecimal(row[_localizer["Latitude"]]??0) },
            { _localizer["Longitude"], (row,item) => item.Longitude = Convert.ToDecimal(row[_localizer["Longitude"]]??0) },

        }, _localizer["Shops"]);
        if (result.Succeeded)
        {
            foreach (var dto in result.Data)
            {
                var item = _mapper.Map<Shop>(dto);
                await _context.Shops.AddAsync(item, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        else
        {
            return Result.Failure(result.Errors);
        }
    }
    public async Task<byte[]> Handle(CreateShopsTemplateCommand request, CancellationToken cancellationToken)
    {

        var fields = new string[] {
                   _localizer["Name"],
                   _localizer["Contact Name"],
                   _localizer["Contact Phone"],
                   _localizer["Address"],
                   _localizer["Delivery Distance"],
                   _localizer["Min Charge"],
                   _localizer["Icon"],
                   _localizer["Latitude"],
                   _localizer["Longitude"]
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer["Shops"]);
        return result;
    }
}

