// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Products.Caching;
using CleanArchitecture.Razor.Application.Features.Products.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Products.Commands.Update;

public class UpdateProductCommand : ProductDto, IRequest<Result>, IMapFrom<Product>, ICacheInvalidator
{
    public string CacheKey => ProductCacheKey.GetAllCacheKey;

    public CancellationTokenSource ResetCacheToken => ProductCacheKey.ResetCacheToken;
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<UpdateProductCommandHandler> _localizer;
    public UpdateProductCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<UpdateProductCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item != null)
        {
            item = _mapper.Map(request, item);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}

