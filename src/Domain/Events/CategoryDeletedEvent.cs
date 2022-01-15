// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Events;

public class CategoryDeletedEvent : DomainEvent
{
    public CategoryDeletedEvent(Category item)
    {
        Item = item;
    }

    public Category Item { get; }
}

