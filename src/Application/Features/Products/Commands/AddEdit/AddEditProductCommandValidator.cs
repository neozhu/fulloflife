// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Products.Commands.AddEdit;

public class AddEditProductCommandValidator : AbstractValidator<AddEditProductCommand>
{
    public AddEditProductCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(256)
                            .NotEmpty();
        RuleFor(v => v.Description).NotEmpty();

        RuleFor(v => v.CategoryId).GreaterThan(0);
        RuleFor(v => v.Images).NotEmpty();

        RuleFor(v => v.Price).NotNull();
    }
}

