// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities.Log;

namespace CleanArchitecture.Razor.Application.Features.Logs.DTOs;

public class LogDto : IMapFrom<Logger>
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string MessageTemplate { get; set; }
    public string Level { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Exception { get; set; }
    public string UserName { get; set; }
    public string Properties { get; set; }

}
