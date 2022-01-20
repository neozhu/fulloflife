// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Common.Interfaces.Identity.DTOs;

namespace CleanArchitecture.Razor.Application.Common.Interfaces.Identity;

public interface IIdentityService : IService
{
    Task<Result<TokenResponseDto>> LoginAsync(TokenRequestDto request);
    Task<Result<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<string> GetUserNameAsync(string userId);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
    Task<Result> DeleteUserAsync(string userId);
    Task<IDictionary<string, string>> FetchUsers(string roleName);
    Task<string> UpdateLiveStatus(string userId, bool isLive);

    Task<bool> RegisterUser(string nickName, string avatarUrl, int gender, string country, string province, string city, string language);
}
