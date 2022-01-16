// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using CleanArchitecture.Razor.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CleanArchitecture.Razor.Domain.Entities.Product;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        builder.Property(e => e.Options)
           .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                 v => JsonSerializer.Deserialize<Dictionary<string, IList<SKU>>>(v, (JsonSerializerOptions)null)
                );

        builder.Property(u => u.Images)
            .HasConversion(
                d => JsonSerializer.Serialize(d, (JsonSerializerOptions)null),
                s => JsonSerializer.Deserialize<string[]>(s, (JsonSerializerOptions)null)
            );
        builder.Property(u => u.SmallImages)
            .HasConversion(
                d => JsonSerializer.Serialize(d, (JsonSerializerOptions)null),
                s => JsonSerializer.Deserialize<string[]>(s, (JsonSerializerOptions)null)
            );
        builder.Property(u => u.Labels)
            .HasConversion(
                d => JsonSerializer.Serialize(d, (JsonSerializerOptions)null),
                s => JsonSerializer.Deserialize<string[]>(s, (JsonSerializerOptions)null)
            );

    }
}
