// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using static CleanArchitecture.Razor.Domain.Entities.Product;

namespace CleanArchitecture.Razor.Application.Features.Products.DTOs;


public class ProductDto : IMapFrom<Product>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
                 .ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name));

        profile.CreateMap<ProductDto, Product>()
                .ForMember(x => x.Category, y => y.Ignore());

    }
    public TrackingState TrackingState { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Remark { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
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
    public string[]? SmalllImages { get; set; }
}

