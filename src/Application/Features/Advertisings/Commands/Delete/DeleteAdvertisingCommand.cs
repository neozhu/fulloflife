// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Commands.Delete;

public class DeleteAdvertisingCommand : IRequest<Result>
{
    public int Id { get; set; }
}
public class DeleteCheckedAdvertisingsCommand : IRequest<Result>
{
    public int[] Id { get; set; }
}

public class DeleteAdvertisingCommandHandler :
             IRequestHandler<DeleteAdvertisingCommand, Result>,
             IRequestHandler<DeleteCheckedAdvertisingsCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteAdvertisingCommandHandler> _localizer;
    public DeleteAdvertisingCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<DeleteAdvertisingCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteAdvertisingCommand request, CancellationToken cancellationToken)
    {

        var item = await _context.Advertisings.FindAsync(new object[] { request.Id }, cancellationToken);
        _context.Advertisings.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> Handle(DeleteCheckedAdvertisingsCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Advertisings.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            _context.Advertisings.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

