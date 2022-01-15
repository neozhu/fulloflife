// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.AddEdit;

public class AddEditCategoryCommandValidator : AbstractValidator<AddEditCategoryCommand>
{
    public AddEditCategoryCommandValidator()
    {

        RuleFor(v => v.Name)
              .MaximumLength(256)
              .NotEmpty();
        RuleFor(v => v.Icon)
              .MaximumLength(256)
              .NotEmpty();

    }
}

