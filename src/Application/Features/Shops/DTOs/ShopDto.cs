// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Shops.DTOs;


public class ShopDto : IMapFrom<Shop>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Shop, ShopDto>().ReverseMap();
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public decimal MinCharge { get; set; } = 30m;
    public decimal DeliveryDistance { get; set; } = 3000m;
    public string? Icon { get; set; }
}

