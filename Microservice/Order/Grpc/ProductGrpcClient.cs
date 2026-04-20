using Grpc.Core;
using ProductGrpc;
using System.Globalization;

namespace OrderService.Grpc
{
    public class ProductGrpcClient : IProductGrpcClient
    {
        private readonly ProductLookup.ProductLookupClient _client;

        public ProductGrpcClient(ProductLookup.ProductLookupClient client)
        {
            _client = client;
        }

        public async Task<ProductInfo> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var reply = await _client.GetProductByIdAsync(new GetProductByIdRequest
                {
                    Id = productId.ToString()
                }, cancellationToken: cancellationToken);

                return new ProductInfo
                {
                    Id = Guid.Parse(reply.Id),
                    Name = reply.Name,
                    Price = decimal.Parse(reply.Price, CultureInfo.InvariantCulture),
                    Quantity = reply.Quantity
                };
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new Exception($"Product '{productId}' was not found.");
            }
        }
    }
}
