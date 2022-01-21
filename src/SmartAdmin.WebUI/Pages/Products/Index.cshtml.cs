// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Products.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Products.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.Products.Queries.Export;
using CleanArchitecture.Razor.Application.Features.Products.Queries.Pagination;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Products.Commands.Import;

namespace SmartAdmin.WebUI.Pages.Products;

[Authorize(policy: Permissions.Products.View)]
public class IndexModel : PageModel
{
    [BindProperty]
    public AddEditProductCommand Input { get; set; }
    [BindProperty]
    public IFormFile UploadedFile { get; set; }
    [BindProperty]
    public IFormFile[] ImagesUploadedFile { get; set; }

    private readonly ISender _mediator;
    private readonly IStringLocalizer<IndexModel> _localizer;
    private readonly IQiniuService _qiniuService;
    public SelectList Categories { get; set; }
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
    public async Task OnGetAsync()
    {
        var request = new GetAllCategoriesQuery();
        var catelist = await _mediator.Send(request);
        Categories = new SelectList(catelist, "Id", "Name");

    }
    public async Task<IActionResult> OnGetDataAsync([FromQuery] ProductsWithPaginationQuery command)
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
        var result1 = new List<string>();
        var result2 = new List<string>();
        foreach (var uploadfile in ImagesUploadedFile)
        {
            var filename = uploadfile.FileName;
            using (var largStream = new MemoryStream())
            using (var smallStream = new MemoryStream())
            using (var stream = new MemoryStream())
            {
                await uploadfile.CopyToAsync(stream);
                stream.Position = 0;
                using (var image = Image.Load(stream))
                {
                    var largclone = image.Clone(context => context
                               .Resize(new ResizeOptions
                               {
                                   Mode = ResizeMode.Crop,
                                   Size = new Size(600, 450)
                               }));
                    largclone.Save(largStream,
                        new PngEncoder
                        {
                            TransparentColorMode = PngTransparentColorMode.Preserve
                        });

                    var smallclone = image.Clone(context => context
                               .Resize(new ResizeOptions
                               {
                                   Mode = ResizeMode.Crop,
                                   Size = new Size(90, 68)
                               }));
                    smallclone.Save(smallStream,
                        new PngEncoder
                        {
                            TransparentColorMode = PngTransparentColorMode.Preserve
                        });
                }
                var data1 = smallStream.ToArray();
                var key1 = await _qiniuService.Upload(data1, $"product_small_{DateTime.UtcNow.Ticks}_{filename}");
                result1.Add(key1);
                var data2 = largStream.ToArray();
                var key2 = await _qiniuService.Upload(data2, $"product_{DateTime.UtcNow.Ticks}_{filename}");
                result2.Add(key2);
            }
        }
        return new JsonResult(new { smallImages = result1, Images = result2 });

    }

    public async Task<IActionResult> OnPostDeleteCheckedAsync([FromBody] DeleteCheckedProductsCommand command)
    {

        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<IActionResult> OnGetDeleteAsync([FromQuery] DeleteProductCommand command)
    {
        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<FileResult> OnPostExportAsync([FromBody] ExportProductsQuery command)
    {
        var result = await _mediator.Send(command);
        return File(result, "application/vnd.openxmlformats-officeCategory.spreadsheetml.sheet", _localizer["Products"] + ".xlsx");
    }
    public async Task<IActionResult> OnPostImportAsync()
    {
        var stream = new MemoryStream();
        await UploadedFile.CopyToAsync(stream);
        var command = new ImportProductsCommand()
        {
            FileName= UploadedFile.FileName,
            Data=stream.ToArray()
        };
        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }

}
