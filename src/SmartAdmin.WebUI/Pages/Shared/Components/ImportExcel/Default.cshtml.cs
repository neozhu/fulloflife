// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SmartAdmin.WebUI.Pages.Shared.Components.ImportExcel;

public class DefaultModel
{
    [BindProperty, Display(Name = "File")]
    public IFormFile UploadedFile { get; set; }
    public string ImportUri { get; set; }
    public string GetTemplateUri { get; set; }
    public string AntiForgeryToken { get; set; }
    public string OnImportedSucceeded { get; set; }
}
