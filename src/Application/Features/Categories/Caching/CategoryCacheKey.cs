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

    static CategoryCacheKey()
    {
        ResetCacheToken = new CancellationTokenSource();
    }
    public static CancellationTokenSource ResetCacheToken { get; private set; }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(ResetCacheToken.Token));
}

