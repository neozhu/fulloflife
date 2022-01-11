// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using CleanArchitecture.Razor.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
     
        builder.Property(e => e.Options)
           .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                 v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions)null)
                );

        builder.Property(u => u.Pictures)
            .HasConversion(
                d => JsonSerializer.Serialize(d, (JsonSerializerOptions)null),
                s => JsonSerializer.Deserialize<string[]>(s, (JsonSerializerOptions)null)
            );
       
    }
}
