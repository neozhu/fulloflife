// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using FluentAssertions;
using NUnit.Framework;
namespace CleanArchitecture.Application.IntegrationTests.Services;
using static Testing;
public class QiniuTest : TestBase
{

    [Test]
    public async Task UploadFileToQiniu()
    {
        var path = @"D:\github\fulloflife\tests\Application.IntegrationTests\img\1.png";
        var buffer = File.ReadAllBytes(path);
        var qiniu = await GetRequiredService<IQiniuService>();
        var result =await qiniu.Upload(buffer, "test_1.png");
        result.Should().Contain("test_1.png");
    }
    [Test]
    public async Task DeleteFileFromQiniu()
    {
        var path = @"D:\github\fulloflife\tests\Application.IntegrationTests\img\1.png";
        var buffer = File.ReadAllBytes(path);
        var qiniu = await GetRequiredService<IQiniuService>();
        await qiniu.Upload(buffer, "test_1.png");
        var result =await qiniu.Delete("test_1.png");
        result.Should().Be(200);
    }
}
