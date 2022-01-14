// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Entities;

public class VendorAddress : AuditableEntity
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? PhoneNumber { get; set; }
}
