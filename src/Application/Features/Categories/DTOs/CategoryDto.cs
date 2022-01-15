// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Categories.DTOs;


public class CategoryDto : IMapFrom<Category>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryDto>().ReverseMap();

    }

    public TrackingState TrackingState { get; set; }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
}

