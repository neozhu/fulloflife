// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Delete;

public class DeleteShopCommand : IRequest<Result>
{
    public int Id { get; set; }
}
public class DeleteCheckedShopsCommand : IRequest<Result>
{
    public int[] Id { get; set; }
}

public class DeleteShopCommandHandler :
             IRequestHandler<DeleteShopCommand, Result>,
             IRequestHandler<DeleteCheckedShopsCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteShopCommandHandler> _localizer;
    private readonly IQiniuService _qiniuService;

    public DeleteShopCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<DeleteShopCommandHandler> localizer,
         IQiniuService qiniuService,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _qiniuService = qiniuService;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteShopCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Shops.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item is null)
        {
            return Result.Success();
        }
        _context.Shops.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        var uri = new Uri(item.Icon);
        var key = Path.GetFileName(uri.LocalPath);
        await _qiniuService.Delete(key);
        return Result.Success();
    }

    public async Task<Result> Handle(DeleteCheckedShopsCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Shops.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        if (items is null)
        {
            return Result.Success();
        }
        foreach (var item in items)
        {
            var uri = new Uri(item.Icon);
            var key = Path.GetFileName(uri.LocalPath);
            await _qiniuService.Delete(key);

            _context.Shops.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

