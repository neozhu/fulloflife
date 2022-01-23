// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Commands.AddEdit;

public class AddEditAdvertisingCommandValidator : AbstractValidator<AddEditAdvertisingCommand>
{
    public AddEditAdvertisingCommandValidator()
    {
        RuleFor(v => v.Category)
              .MaximumLength(256)
              .NotEmpty();
        RuleFor(v => v.Content)
              .MaximumLength(256)
              .NotEmpty();
        RuleFor(v => v.Picture)
               .NotEmpty();
        RuleFor(v => v.ExpiredDate)
            .NotNull()
            .Must(x => x > DateTime.Now);
    }
}

