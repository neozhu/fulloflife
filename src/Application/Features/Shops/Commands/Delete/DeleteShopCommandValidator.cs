// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Delete;

public class DeleteShopCommandValidator : AbstractValidator<DeleteShopCommand>
{
    public DeleteShopCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().GreaterThan(0);

    }
}
public class DeleteCheckedShopsCommandValidator : AbstractValidator<DeleteCheckedShopsCommand>
{
    public DeleteCheckedShopsCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().NotEmpty();

    }
}

