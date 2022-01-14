// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Entities;

public class Announcement : AuditableEntity
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime DateDue { get; set; }
}
