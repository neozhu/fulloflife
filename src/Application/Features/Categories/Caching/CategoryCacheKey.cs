// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace CleanArchitecture.Razor.Application.Features.Categories.Caching;

public static class CategoryCacheKey
{
    public const string GetAllCacheKey = "all-Categories";
    public static string GetPagtionCacheKey(string parameters)
    {
        return "CategoriesWithPaginationQuery,{parameters}";
    }
}

