// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Categories.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Categories.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.Export;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using CleanArchitecture.Razor.Application.Constants.Permission;
using Microsoft.AspNetCore.Mvc.Rendering;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace SmartAdmin.WebUI.Pages.Categories;

[Authorize(policy: Permissions.Categories.View)]
public class IndexModel : PageModel
{
    [BindProperty]
    public AddEditCategoryCommand Input { get; set; }
    [BindProperty]
    public IFormFile UploadedFile { get; set; }


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
    public async Task<IActionResult> OnGetDataAsync([FromQuery] CategoriesWithPaginationQuery command)
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
        var filename = UploadedFile.FileName;
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
            var result = await _qiniuService.Upload(data, $"product_catalog_{DateTime.UtcNow.Ticks}_{filename}");
            return new JsonResult(result);
        }
          
        
    }

    public async Task<IActionResult> OnPostDeleteCheckedAsync([FromBody] DeleteCheckedCategoriesCommand command)
    {

        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<IActionResult> OnGetDeleteAsync([FromQuery] DeleteCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return new JsonResult(result);
    }
    public async Task<FileResult> OnPostExportAsync([FromBody] ExportCategoriesQuery command)
    {
        var result = await _mediator.Send(command);
        return File(result, "application/vnd.openxmlformats-officeCategory.spreadsheetml.sheet", _localizer["Categorys"] + ".xlsx");
    }


}
