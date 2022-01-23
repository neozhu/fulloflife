// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Commands.Import;

public class ImportAdvertisingsCommandValidator : AbstractValidator<ImportAdvertisingsCommand>
{
    public ImportAdvertisingsCommandValidator()
    {

        RuleFor(v => v.Data)
               .NotNull()
               .NotEmpty();

    }
}

