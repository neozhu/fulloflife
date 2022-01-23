// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Caching;

public static class AdvertisingCacheKey
{
    public const string GetAllCacheKey = "all-Advertisings";
    public static string GetPagtionCacheKey(string parameters)
    {
        return "AdvertisingsWithPaginationQuery,{parameters}";
    }
    static AdvertisingCacheKey()
    {
        ResetCacheToken = new CancellationTokenSource();
    }
    public static CancellationTokenSource ResetCacheToken { get; private set; }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(ResetCacheToken.Token));
}

