// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Commands.Delete;

public class DeleteAdvertisingCommandValidator : AbstractValidator<DeleteAdvertisingCommand>
{
    public DeleteAdvertisingCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().GreaterThan(0);

    }
}
public class DeleteCheckedAdvertisingsCommandValidator : AbstractValidator<DeleteCheckedAdvertisingsCommand>
{
    public DeleteCheckedAdvertisingsCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().NotEmpty();

    }
}

