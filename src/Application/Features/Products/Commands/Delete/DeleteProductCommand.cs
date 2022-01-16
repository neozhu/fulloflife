// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Products.Caching;

namespace CleanArchitecture.Razor.Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<Result>, ICacheInvalidator
{
    public int Id { get; set; }

    public string CacheKey => ProductCacheKey.GetAllCacheKey;

    public CancellationTokenSource ResetCacheToken => ProductCacheKey.ResetCacheToken;
}
public class DeleteCheckedProductsCommand : IRequest<Result>, ICacheInvalidator
{
    public int[] Id { get; set; } = Array.Empty<int>();
    public string CacheKey => ProductCacheKey.GetAllCacheKey;

    public CancellationTokenSource ResetCacheToken => ProductCacheKey.ResetCacheToken;
}

public class DeleteProductCommandHandler :
             IRequestHandler<DeleteProductCommand, Result>,
             IRequestHandler<DeleteCheckedProductsCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IQiniuService _qiniuService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteProductCommandHandler> _localizer;
    public DeleteProductCommandHandler(
        IApplicationDbContext context,
        IQiniuService qiniuService,
        IStringLocalizer<DeleteProductCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _qiniuService = qiniuService;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item is not null)
        {
            _context.Products.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            if (item.Images is not null)
            {
                foreach (var img in item.Images)
                {
                    var uri = new Uri(img);
                    var key = Path.GetFileName(uri.LocalPath);
                    await _qiniuService.Delete(key);
                }
            }
            if (item.SmallImages is not null)
            {
                foreach (var img in item.SmallImages)
                {
                    var uri = new Uri(img);
                    var key = Path.GetFileName(uri.LocalPath);
                    await _qiniuService.Delete(key);
                }
            }
        }

        return Result.Success();
    }

    public async Task<Result> Handle(DeleteCheckedProductsCommand request, CancellationToken cancellationToken)
    {
        //TODO:Implementing DeleteCheckedProductsCommandHandler method 
        var items = await _context.Products.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            _context.Products.Remove(item);

            if (item.Images is not null)
            {
                foreach (var img in item.Images)
                {
                    var uri = new Uri(img);
                    var key = Path.GetFileName(uri.LocalPath);
                    await _qiniuService.Delete(key);
                }
            }
            if (item.SmallImages is not null)
            {
                foreach (var img in item.SmallImages)
                {
                    var uri = new Uri(img);
                    var key = Path.GetFileName(uri.LocalPath);
                    await _qiniuService.Delete(key);
                }
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

