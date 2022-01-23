// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using static CleanArchitecture.Razor.Domain.Entities.Product;

namespace CleanArchitecture.Razor.Application.Features.Products.DTOs;


public class ProductDto : IMapFrom<Product>
{
    public void Mapping(Profile profile)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        profile.CreateMap<Product, ProductDto>()
                 .ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name))
                 .ForMember(x=>x.Labels, y => y.MapFrom(z=>JsonSerializer.Serialize(z.Labels, serializeOptions)))
                 .ForMember(x=>x.Images, y => y.MapFrom(z=>JsonSerializer.Serialize(z.Images, serializeOptions)))
                 .ForMember(x => x.SmallImages, y => y.MapFrom(z => JsonSerializer.Serialize(z.SmallImages, serializeOptions)))
                 .ForMember(x => x.Options, y => y.MapFrom(z => JsonSerializer.Serialize(z.Options, serializeOptions)))
                 ;

        profile.CreateMap<ProductDto, Product>()
                .ForMember(x => x.Category, y => y.Ignore())
                .ForMember(x => x.Labels, y => y.MapFrom(z => JsonSerializer.Deserialize<string[]?>(z.Labels, serializeOptions)))
                .ForMember(x => x.Images, y => y.MapFrom(z => JsonSerializer.Deserialize<string[]?>(z.Images, serializeOptions)))
                .ForMember(x => x.SmallImages, y => y.MapFrom(z => JsonSerializer.Deserialize<string[]?>(z.SmallImages, serializeOptions)))
                .ForMember(x => x.Options, y => y.MapFrom(z =>JsonSerializer.Deserialize<Dictionary<string,IList<SKU>>?>(z.Options, serializeOptions)))
                ;

    }
    public TrackingState TrackingState { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Remark { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Unit { get; set; }
    public int Sort { get; set; }
    public bool IsNew { get; set; } = true;
    public bool IsEnable { get; set; } = true;
    public bool HasRecommend { get; set; }
    public bool IsSingle { get; set; }
    public string? Options { get; set; }
    public string? Labels { get; set; }
    public decimal? Price { get; set; }
    public decimal? Cost { get; set; }
    public int StockQty { get; set; }
    public int SalesQty { get; set; }
    public string? Images { get; set; }
    public string? SmallImages { get; set; }
}

