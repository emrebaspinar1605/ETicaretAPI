using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandle : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _write;
        readonly IStorageService _storage;
        readonly IProductReadRepository _read;
        public UploadProductImageCommandHandle(IProductImageFileWriteRepository write, IProductReadRepository read, IStorageService storage)
        {
            _write = write;
            _read = read;
            _storage = storage;
        }
        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storage.UploadAsync("photo-images",request.Files);
            var product = await _read.GetByIdAsync(request.Id);

            await _write.AddRangeAsync(result.Select(p => new Domain.Entities.ProductImageFile
            {
                FileName = p.fileName,
                Path = p.pathOrContainerName,
                Storage = _storage.StorageName,
                Products = new List<Domain.Entities.Product>() { product }
            }).ToList());

            await _write.SaveAsync();
            return new();
        }
    }
}
