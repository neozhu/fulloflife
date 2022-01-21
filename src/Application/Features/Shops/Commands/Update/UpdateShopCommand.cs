// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Shops.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Shops.Commands.Update;

    public class UpdateShopCommand: ShopDto,IRequest<Result>, IMapFrom<Shop>
    {
        
    }

    public class UpdateShopCommandHandler : IRequestHandler<UpdateShopCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateShopCommandHandler> _localizer;
        public UpdateShopCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateShopCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateShopCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateShopCommandHandler method 
           var item =await _context.Shops.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }

