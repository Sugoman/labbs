using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using OrderService.Protos;

namespace OrderService.Services
{
    public class OrderService : Protos.OrderService.OrderServiceBase
    {
        private readonly List<OrderResponse> _orders = new();
        private readonly object _lock = new();

        public override Task<OrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var order = new OrderResponse
            {
                Id = Guid.NewGuid().ToString(),
                OrderDate = request.OrderDate,
            };
            order.Items.AddRange(request.Items);

            lock (_lock) _orders.Add(order);

            return Task.FromResult(order);
        }

        public override Task<OrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            var order = _orders.FirstOrDefault(o => o.Id == request.Id);
            return order == null
                ? throw new RpcException(new Status(StatusCode.NotFound, "Order not found"))
                : Task.FromResult(order);
        }

        public override Task<OrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
        {
            lock (_lock)
            {
                var order = _orders.FirstOrDefault(o => o.Id == request.Id) ??
                    throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));

                order.OrderDate = request.OrderDate;
                order.Items.Clear();
                order.Items.AddRange(request.Items);

                return Task.FromResult(order);
            }
        }

        public override Task<Empty> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
        {
            lock (_lock) _orders.RemoveAll(o => o.Id == request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<ListOrdersResponse> ListOrders(Empty request, ServerCallContext context)
        {
            var response = new ListOrdersResponse();
            response.Orders.AddRange(_orders);
            return Task.FromResult(response);
        }

        public override Task<ListOrdersResponse> FilterOrders(FilterOrdersRequest request, ServerCallContext context)
        {
            var query = _orders.AsEnumerable();

            if (request.StartDate != null)
                query = query.Where(o => o.OrderDate >= request.StartDate);

            if (request.EndDate != null)
                query = query.Where(o => o.OrderDate <= request.EndDate);

            if (request.MinTotal > 0)
                query = query.Where(o => o.Items.Sum(i => i.Price) >= request.MinTotal);

            if (request.MaxTotal > 0)
                query = query.Where(o => o.Items.Sum(i => i.Price) <= request.MaxTotal);

            var response = new ListOrdersResponse();
            response.Orders.AddRange(query);
            return Task.FromResult(response);
        }
    }
}
