// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Entities;

public class Product : AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public string? Unit { get; set; }
    public int Sequence { get; set; }
    public bool IsNew { get; set; } = true;
    public bool IsPublish { get; set; }
    public Dictionary<string, decimal?>? Options { get; set; }
    public decimal? Price { get; set; }
    public decimal? Cost { get; set; }
    public int StockQty { get; set; }
    public int SalesQty { get; set; }
    public string[]? Pictures { get; set; }

}
