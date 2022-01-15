// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using SmartAdmin.WebUI.Pages.Shared.Components.ImportExcel;

namespace SmartAdmin.WebUI.ViewComponents;

public class ImportExcelViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string importUri, string getTemplateUri, string onImportedSucceeded)
    {
        return View(new DefaultModel()
        {
            ImportUri = importUri,
            GetTemplateUri = getTemplateUri,
            OnImportedSucceeded = onImportedSucceeded
        });
    }
}
