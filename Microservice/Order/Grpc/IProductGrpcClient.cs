namespace OrderService.Grpc
{
    public interface IProductGrpcClient
    {
        Task<ProductInfo> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}
