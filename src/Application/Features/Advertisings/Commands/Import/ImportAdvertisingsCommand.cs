// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Commands.Import;

public class ImportAdvertisingsCommand : IRequest<Result>
{
    public ImportAdvertisingsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
    public string FileName { get; set; }
    public byte[] Data { get; set; }
}


public class ImportAdvertisingsCommandHandler :
             IRequestHandler<ImportAdvertisingsCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ImportAdvertisingsCommandHandler> _localizer;
    private readonly IExcelService _excelService;

    public ImportAdvertisingsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportAdvertisingsCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ImportAdvertisingsCommand request, CancellationToken cancellationToken)
    {

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, AdvertisingDto, object>>
        {
             { _localizer["Category"], (row,item) => item.Category = row[_localizer["Category"]]?.ToString() },
             { _localizer["Title"], (row,item) => item.Title = row[_localizer["Title"]]?.ToString() },
             { _localizer["Content"], (row,item) => item.Content = row[_localizer["Content"]]?.ToString() },
             { _localizer["ExpiredDate"], (row,item) => item.ExpiredDate =Convert.ToDateTime(row[_localizer["ExpiredDate"]]?.ToString()) },
        }, _localizer["Advertisings"]);
        if (result.Succeeded)
        {
            foreach (var dto in result.Data)
            {
                var item = _mapper.Map<Advertising>(dto);
                if (!string.IsNullOrEmpty(dto.Title))
                {
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == dto.Title,cancellationToken);
                    item.RelevantProductId = product?.Id;
                }
                await _context.Advertisings.AddAsync(item, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        else
        {
            return Result.Failure(result.Errors);
        }
    }

}

