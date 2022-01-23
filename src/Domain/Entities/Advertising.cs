// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Entities;

public  class Advertising : AuditableEntity
{
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Picture { get; set; }
    public string? Content { get; set; }
    public int? RelevantProductId { get; set; }
    public virtual Product? RelevantProduct { get; set; }
    public DateTime ExpiredDate { get; set; }
}
