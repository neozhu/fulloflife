// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Domain.Events;


public class CategoryUpdatedEvent : DomainEvent
{
    public CategoryUpdatedEvent(Category item)
    {
        Item = item;
    }

    public Category Item { get; }
}

