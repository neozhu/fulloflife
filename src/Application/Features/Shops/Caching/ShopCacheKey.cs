// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Shops.Caching;

public static class ShopCacheKey
{
    public const string GetAllCacheKey = "all-Shops";
    public static string GetPagtionCacheKey(string parameters) {
        return "ShopsWithPaginationQuery,{parameters}";
    }
        static ShopCacheKey()
    {
        ResetCacheToken = new CancellationTokenSource();
    }
    public static CancellationTokenSource ResetCacheToken { get; private set; }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(ResetCacheToken.Token));
}

