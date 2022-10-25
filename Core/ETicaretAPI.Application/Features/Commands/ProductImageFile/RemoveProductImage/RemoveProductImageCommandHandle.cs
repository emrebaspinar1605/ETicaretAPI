using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandle : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        readonly IProductReadRepository _read;
        readonly IProductImageFileWriteRepository _write;

        public RemoveProductImageCommandHandle(IProductReadRepository read, IProductImageFileWriteRepository write)
        {
            _read = read;
            _write = write;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _read.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

            Domain.Entities.ProductImageFile? productImageFile = product?.ProductImageFiles
            .FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));

            if (productImageFile is not null)
                product?.ProductImageFiles.Remove(productImageFile);

            
            await _write.SaveAsync();
            return new();

        }


    }
}

