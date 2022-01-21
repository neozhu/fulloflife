// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Common.Interfaces.Identity.WeiChat;

public class UserInfo
{
    public UserInfo(string nickName, string avatarUrl, string openpid)
    {
        this.nickName = nickName;
        this.avatarUrl = avatarUrl;
        this.openpid = openpid;
    }
    public string openpid { get; set; }
    public string nickName { get; set; }
    public string avatarUrl { get; set; }
    public int gender { get; set; }
    public string? country { get; set; }
    public string? province { get; set; }
    public string? city { get; set; }
    public string? language { get; set; }
}
