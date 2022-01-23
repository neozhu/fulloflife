// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Entities;

public class HistorySearch : AuditableEntity
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int? RelevantProductId { get; set; }
    public virtual Product? RelevantProduct { get; set; }
}
