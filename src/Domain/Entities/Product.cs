// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Entities;

public class Product : AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Remark { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    public string? Unit { get; set; }
    public int Sort { get; set; }
    public bool IsNew { get; set; } = true;
    public bool IsEnable { get; set; } = true;
    public bool IsSingle { get; set; }
    public Dictionary<string, IList<SKU>>? Options { get; set; }
    public string[]? Labels { get; set; }
    public decimal? Price { get; set; }
    public decimal? Cost { get; set; }
    public int StockQty { get; set; }
    public int SalesQty { get; set; }
    public string[]? Images { get; set; }
    public string[]? SmallImages { get; set; }

    public class SKU
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public bool IsSelected { get; set; }
        public decimal? Price { get; set; }
    }

}
