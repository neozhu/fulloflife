// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Shops.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.Shops.Queries.Export;
using CleanArchitecture.Razor.Application.Features.Shops.Queries.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using CleanArchitecture.Razor.Application.Constants.Permission;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using CleanArchitecture.Razor.Application.Features.Shops.Commands.Import;

namespace SmartAdmin.WebUI.Pages.Shops;

[Authorize(policy: Permissions.Shops.View)]
public class IndexModel : PageModel
{
    [BindProperty]
    public AddEditShopCommand Input { get; set; }
    [BindProperty]
    public IFormFile UploadedFile { get; set; }
    [BindProperty]
    public IFormFile IconUploadedFile { get; set; }

    private readonly ISender _mediator;
    private readonly IStringLocalizer<IndexModel> _localizer;
    private readonly IQiniuService _qiniuService;

    public IndexModel(
            ISender mediator,
            IStringLocalizer<IndexModel> localizer,
            IQiniuService qiniuService
        )
    {
        _mediator = mediator;
        _localizer = localizer;
        _qiniuService = qiniuService;
    }
    public Task OnGetAsync()
    {

        return Task.CompletedTask;
    }
    public async Task<IActionResult> OnGetDataAsync([FromQuery] ShopsWithPaginationQuery command)
    {
        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var result = await _mediator.Send(Input);
        return new JsonResult(result);

    }
    public async Task<IActionResult> OnPostQiniu()
    {
        var filename = IconUploadedFile.FileName;
        using (var smallStream = new MemoryStream())
        using (var stream = new MemoryStream())
        {
            await UploadedFile.CopyToAsync(stream);
            stream.Position = 0;
            using (var image = Image.Load(stream))
            {
                var clone = image.Clone(context => context
                           .Resize(new ResizeOptions
                           {
                               Mode = ResizeMode.Crop,
                               Size = new Size(68, 68)
                           }));
                clone.Save(smallStream,
                    new PngEncoder
                    {
                        TransparentColorMode = PngTransparentColorMode.Preserve
                    });
            }
            var data = smallStream.ToArray();
            var result = await _qiniuService.Upload(data, $"shop_icon_{DateTime.UtcNow.Ticks}_{filename}");
            return new JsonResult(result);
        }
    }

    public async Task<IActionResult> OnPostDeleteCheckedAsync([FromBody] DeleteCheckedShopsCommand command)
    {

        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<IActionResult> OnGetDeleteAsync([FromQuery] DeleteShopCommand command)
    {
        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<FileResult> OnPostExportAsync([FromBody] ExportShopsQuery command)
    {
        var result = await _mediator.Send(command);
        return File(result, "application/vnd.openxmlformats-officeShop.spreadsheetml.sheet", _localizer["Shops"] + ".xlsx");
    }
    public async Task<IActionResult> OnPostImportAsync()
    {
        using (var stream = new MemoryStream())
        {
            await UploadedFile.CopyToAsync(stream);
            var command = new ImportShopsCommand(UploadedFile.FileName, stream.ToArray());
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
    }

}
