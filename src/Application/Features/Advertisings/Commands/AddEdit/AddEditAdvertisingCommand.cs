// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.Features.Advertisings.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Advertisings.Commands.AddEdit;

public class AddEditAdvertisingCommand : AdvertisingDto, IRequest<Result<int>>, IMapFrom<Advertising>
{

}

public class AddEditAdvertisingCommandHandler : IRequestHandler<AddEditAdvertisingCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditAdvertisingCommandHandler> _localizer;
    public AddEditAdvertisingCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditAdvertisingCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditAdvertisingCommand request, CancellationToken cancellationToken)
    {
 
        if (request.Id > 0)
        {
            var item = await _context.Advertisings.FindAsync(new object[] { request.Id }, cancellationToken);
            item = _mapper.Map(request, item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Advertising>(request);
            _context.Advertisings.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

