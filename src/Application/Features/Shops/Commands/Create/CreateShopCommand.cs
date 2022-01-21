// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Create;

public class CreateShopCommand : ShopDto, IRequest<Result<int>>, IMapFrom<Shop>
{

}

public class CreateShopCommandHandler : IRequestHandler<CreateShopCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CreateShopCommand> _localizer;
    public CreateShopCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<CreateShopCommand> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateShopCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Shop>(request);
        _context.Shops.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(item.Id);
    }
}

