// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Update;

public class UpdateShopCommandValidator : AbstractValidator<UpdateShopCommand>
{
    public UpdateShopCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(256)
            .NotEmpty();
        RuleFor(v => v.ContactPhone)
                  .MaximumLength(256)
                  .NotEmpty();
        RuleFor(v => v.Address)
                 .MaximumLength(256)
                 .NotEmpty();
    }
}

