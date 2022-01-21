// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Import;

public class ImportShopsCommandValidator : AbstractValidator<ImportShopsCommand>
{
    public ImportShopsCommandValidator()
    {

        RuleFor(v => v.Data)
              .NotNull()
              .NotEmpty();

    }
}

