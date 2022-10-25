using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandle : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        IProductReadRepository _read;

        public GetProductByIdQueryHandle(IProductReadRepository read)
        {
            _read = read;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
           var product = await _read.GetByIdAsync(request.Id, false);
            return new()
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };
            
        }
    }

}
