// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Common.Interfaces;

public interface IQiniuService
{
    Task<string> Upload(byte[] data,string fileName);
    Task<int> Delete(string key);
}
