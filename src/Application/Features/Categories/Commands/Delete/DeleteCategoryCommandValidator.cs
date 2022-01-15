// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);

    }
}
public class DeleteCheckedCategoriesCommandValidator : AbstractValidator<DeleteCheckedCategoriesCommand>
{
    public DeleteCheckedCategoriesCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().NotEmpty();

    }
}

