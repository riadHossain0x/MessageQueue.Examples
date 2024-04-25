using MassTransit;
using MassTransit.Configuration;
using MassTransit.Consumer;

namespace RabbitMQConsumer.MassTransit;

public static class NewConsumerExtensions
{
    public static void M2Consumer<TConsumer>(this IReceiveEndpointConfigurator configurator, Func<TConsumer> consumerFactoryMethod,
    Action<IConsumerConfigurator<TConsumer>> configure = null)
    where TConsumer : class, IConsumer
    {
        if (configurator == null)
            throw new ArgumentNullException(nameof(configurator));
        if (consumerFactoryMethod == null)
            throw new ArgumentNullException(nameof(consumerFactoryMethod));

        LogContext.Debug?.Log("Subscribing Consumer: {ConsumerType} (using delegate consumer factory)", TypeCache<TConsumer>.ShortName);

        var delegateConsumerFactory = new DelegateConsumerFactory<TConsumer>(consumerFactoryMethod);

        var consumerConfigurator = new ConsumerConfigurator<TConsumer>(delegateConsumerFactory, configurator);

        configure?.Invoke(consumerConfigurator);

        configurator.AddEndpointSpecification(consumerConfigurator);
    }
}
