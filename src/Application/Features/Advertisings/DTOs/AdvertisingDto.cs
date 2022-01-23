// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;


public class AdvertisingDto:IMapFrom<Advertising>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Advertising, AdvertisingDto>()
            .ForMember(x => x.Title, y => y.MapFrom(z => z.RelevantProduct.Name));
        profile.CreateMap<AdvertisingDto, Advertising>()
                .ForMember(x => x.RelevantProduct, y => y.Ignore());

    }
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Picture { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int? RelevantProductId { get; set; }
    public virtual Product? RelevantProduct { get; set; }
    public DateTime ExpiredDate { get; set; }
}

