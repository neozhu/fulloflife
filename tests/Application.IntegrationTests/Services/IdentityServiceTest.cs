// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity.DTOs;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity.WeiChat;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Services;
using static Testing;
public class IdentityServiceTest : TestBase
{
    [Test]
    public async Task RegisterFromWx()
    {

        var identityService = await GetRequiredService<IIdentityService>();
        var guid = Guid.NewGuid().ToString();
        var wxuser = new UserInfo("nickName", "https://test/user.jpg", guid);
        var result = await identityService.RegisterFromWx(wxuser);
        result.Should().BeTrue();
        var loginrequest = new TokenRequestDto()
        {
            UserName = guid,
            Password = $"nickName@{guid}.wx",
            RememberMe = false
        };
        var tokenreponse = await identityService.LoginAsync(loginrequest);

        tokenreponse.Succeeded.Should().BeTrue();
    }
}
