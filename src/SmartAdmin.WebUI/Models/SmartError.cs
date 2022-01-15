// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SmartAdmin.WebUI.Models;






public class SmartError
{
    public string[][] Errors { get; set; } = Array.Empty<string[]>();

    public static SmartError Failed(params string[] errors) => new SmartError { Errors = new[] { errors } };
}
