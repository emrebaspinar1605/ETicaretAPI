using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Product.DeleteProduct
{
    public class RemoveProductCommandHandle : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
    {
        readonly IProductWriteRepository _write;

        public RemoveProductCommandHandle(IProductWriteRepository write)
        {
            _write = write;
        }

        public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _write.RemoveAsync(request.Id);
            await _write.SaveAsync();
            return new();
        }
    }
}
