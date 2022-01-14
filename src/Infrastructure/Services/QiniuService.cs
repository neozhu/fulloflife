// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qiniu.Storage;
using Qiniu.Util;

namespace CleanArchitecture.Razor.Infrastructure.Services;

public class QiniuService : IQiniuService
{
    private readonly QiniuSettings _qiniuSetting;
    private readonly ILogger<QiniuService> _logger;

    public QiniuService(
        IOptions<QiniuSettings> qiniuSetting, ILogger<QiniuService> logger)
    {
        _qiniuSetting = qiniuSetting.Value;
        _logger = logger;
    }
    public Task<string> Upload(byte[] data, string fileName)
    {
        var mac = new Mac(_qiniuSetting.AccessKey, _qiniuSetting.SecretKey);
        // 上传文件名
        string key = fileName;

        // 存储空间名
        string Bucket = _qiniuSetting.Bucket;
        // 设置上传策略
        var putPolicy = new PutPolicy();
        // 设置要上传的目标空间
        putPolicy.Scope = Bucket;
        // 上传策略的过期时间(单位:秒)
        putPolicy.SetExpires(3600);
        // 文件上传完毕后，在多少天后自动被删除
        putPolicy.DeleteAfterDays = 0;
        // 生成上传token
        string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        Config config = new Config();
        // 设置上传区域
        config.Zone = Zone.ZONE_CN_East;
        // 设置 http 或者 https 上传
        config.UseHttps = true;
        config.UseCdnDomains = true;
        config.ChunkSize = ChunkUnit.U512K;
        // 表单上传
        FormUploader target = new FormUploader(config);
        var httpresult = target.UploadData(data, key, token, null);
        var result = JsonSerializer.Deserialize<result>(httpresult.Text);
        Console.WriteLine("form upload result: " + httpresult.ToString());

        return Task.FromResult($"{_qiniuSetting.Domain}{result.key}");
    }

    record result
    {
        public string key { get; set; }
        public string hash { get; set; }
    }
}
