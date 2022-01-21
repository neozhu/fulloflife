// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.AddEdit;

public class AddEditShopCommand : ShopDto, IRequest<Result<int>>, IMapFrom<Shop>
{

}

public class AddEditShopCommandHandler : IRequestHandler<AddEditShopCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditShopCommandHandler> _localizer;
    public AddEditShopCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditShopCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditShopCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Shops.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item is null)
            {
                throw new NullReferenceException(string.Format(_localizer["not found shop entry by {0}"], request.Id));
            }
            item = _mapper.Map(request, item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Shop>(request);
            _context.Shops.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

