using System.IO;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.KeyValues.Queries.ByName;
using FluentAssertions;
using NUnit.Framework;
namespace CleanArchitecture.Application.IntegrationTests.Services;
using static Testing;
public class QiniuTest:TestBase
{
    private readonly IQiniuService _qiniuService;

    public QiniuTest(
        IQiniuService qiniuService)
    {
        _qiniuService = qiniuService;
    }
    [Test]
    public async Task UploadFileToQiniu()
    {
        var path = @"D:\github\fulloflife\tests\Application.IntegrationTests\img\1.png";
        var buffer = File.ReadAllBytes(path);

       var result =await  _qiniuService.Upload(buffer, "1.png");
    }
}
