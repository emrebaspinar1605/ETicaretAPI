using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandle : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductWriteRepository _write;
        readonly IProductReadRepository _read;
        public UpdateProductCommandHandle(IProductWriteRepository write, IProductReadRepository read)
        {
            _write = write;
            _read = read;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _read.GetByIdAsync(request.id);
            if (product is null)
            {
                throw new InvalidOperationException("Data Bulunamadı");
            }
            product.Stock = request.Stock;
            product.Name = request.Name;
            product.Price = request.Price;
            await _write.SaveAsync();
            return new();
        }
    }
}
