using Grpc.Core;
using ProductGrpc;
using ProductService.Services.ProductService;

namespace ProductService.Grpc
{
    public class ProductLookupService : ProductLookup.ProductLookupBase
    {
        private readonly IProductService _productService;

        public ProductLookupService(IProductService productService)
        {
            _productService = productService;
        }

        public override async Task<ProductReply> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var productId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Product id must be a valid GUID."));
            }

            try
            {
                //var product = await _productService.GetProductById(productId);
                return new ProductReply
                {
                    //Id = product.Id.ToString(),
                    //Name = product.Name,
                    //Price = product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    //Quantity = product.Quantity

                    Id = Guid.NewGuid().ToString(),
                    Name = "Grpc",
                    Price = "1200",//product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    Quantity = 1//product.Quantity
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }
    }
}
