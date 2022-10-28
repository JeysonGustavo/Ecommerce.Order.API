using Ecommerce.Order.API.Core.EventBus.Subscriber;
using Microsoft.Extensions.Hosting;

namespace Ecommerce.Order.API.Core.Listener
{
    public class ProductStockChangedListener : IHostedService
    {
        #region Properties
        private readonly ISubscriber _subscribe;
        #endregion

        #region Constructor
        public ProductStockChangedListener(ISubscriber subscribe)
        {
            _subscribe = subscribe;
        }
        #endregion

        #region StartAsync
        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _subscribe.InitializeSubscribers();

            return Task.CompletedTask;
        }
        #endregion

        #region StopAsync
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
