// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Import;

public class ImportCategoriesCommandValidator : AbstractValidator<ImportCategoriesCommand>
{
    public ImportCategoriesCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
              .NotEmpty();

    }
}

