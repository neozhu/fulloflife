// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using CleanArchitecture.Razor.Application.Features.KeyValues.Commands.Delete;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using CleanArchitecture.Razor.Application.Features.KeyValues.Commands.AddEdit;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Application.IntegrationTests.KeyValues.Commands;

using static Testing;

public class DeleteKeyValueTests : TestBase
{
    [Test]
    public void ShouldRequireValidKeyValueId()
    {
        var command = new DeleteKeyValueCommand { Id = 99 };

        FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteKeyValue()
    {
        var addcommand = new AddEditKeyValueCommand()
        {
            Name = "Word",
            Text = "Word",
            Value = "Word",
            Description = "For Test"
        };
        var result = await SendAsync(addcommand);

        await SendAsync(new DeleteKeyValueCommand
        {
            Id = result.Data
        });

        var item = await FindAsync<Document>(result.Data);

        item.Should().BeNull();
    }

}
