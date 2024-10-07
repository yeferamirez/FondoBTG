using Amazon.DynamoDBv2;
using Amazon.SimpleEmail;
using Amazon.SimpleNotificationService;
using Amazon.SimpleSystemsManagement;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Npgsql;
using PensionFund.Business.Services;
using PensionFund.Domain.Constants;
using PensionFund.Infrastructure.Clients;
using PensionFund.Infrastructure.Interfaces.Clients;
using PensionFund.Infrastructure.Interfaces.Repositories;
using PensionFund.Infrastructure.Repositories;

namespace PensionFund.IoCContainer
{
    public static class IoCServiceCollection
    {
        private static readonly string SYSTEM_MANAGER_PATH = Environment.GetEnvironmentVariable("SYSTEM_MANAGER_PATH");

        public static ContainerBuilder BuildContext(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            return BuildContext(builder, configuration);
        }
        public static ContainerBuilder BuildContext(this ContainerBuilder builder, IConfiguration configuration)
        {
            RegisterClients(builder);
            RegisterRepositories(builder);
            RegisterServices(builder);
            return builder;
        }

        private static void RegisterClients(ContainerBuilder builder)
        {
            builder
                 .Register((context, parameters) => new AmazonSimpleSystemsManagementClient())
                 .As<IAmazonSimpleSystemsManagement>()
                 .SingleInstance();

            builder
                .Register((context, parameters) => new SystemManagerClient(
                    context.Resolve<IAmazonSimpleSystemsManagement>()))
                .Named<ISystemManagerClient>("SystemManagerClient")
                .SingleInstance();

            builder
                .Register((context, parameters) => new AmazonDynamoDBClient())
                .As<IAmazonDynamoDB>()
                .SingleInstance();

            builder
                .Register((context, parameters) => new CacheClient(
                    context.Resolve<IAmazonDynamoDB>()))
           .Named<ICacheClient>("DynamoClient")
           .SingleInstance();

            builder
                .Register((context, parameters) =>
                {
                    var parameterStoreClient = context.ResolveNamed<ISystemManagerClient>("SystemManagerClient");
                    ParameterStoreRepository parameterStoreRepository = new ParameterStoreRepository(parameterStoreClient, SYSTEM_MANAGER_PATH);
                    NpgsqlConnectionStringBuilder connectionBuilder = BuildNpgsqlConnectionString(parameterStoreRepository);
                    return new RdsClient(connectionBuilder.ConnectionString.ToString());
                })
           .Named<IRdsClient>("RdsClient")
           .SingleInstance();

            builder
                .Register((context, parameters) => new AmazonSimpleEmailServiceClient())
                .As<IAmazonSimpleEmailService>()
                .SingleInstance();

            builder
                .Register((context, parameters) => new EmailClient(
                    context.Resolve<IAmazonSimpleEmailService>()))
                .Named<IEmailClient>("EmailClient")
                .SingleInstance();

            builder
               .Register((context, parameters) =>
               {
                   var parameterStoreClient = context.ResolveNamed<ISystemManagerClient>("SystemManagerClient");
                   var client = new AmazonSimpleNotificationServiceClient();
                   return new SmsClient(client);
               })
          .Named<ISmsClient>("RdsClient")
          .SingleInstance();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder
                .Register((context, parameters) =>
                {
                    var parameterStoreClient = context.ResolveNamed<ISystemManagerClient>("SystemManagerClient");
                    return new ParameterStoreRepository(parameterStoreClient, SYSTEM_MANAGER_PATH);
                })
                .As<IParameterStoreRepository>()
                .SingleInstance();

            builder
              .Register((context, parameters) =>
              {
                  var rdsClient = context.ResolveNamed<IRdsClient>("RdsClient");
                  return new RdsRepository(rdsClient);
              })
              .As<IRdsRepository>()
              .SingleInstance();

            builder
              .Register((context, parameters) =>
              {
                  var dynamoClient = context.ResolveNamed<ICacheClient>("DynamoClient");
                  var parameterStoreClient = context.ResolveNamed<ISystemManagerClient>("SystemManagerClient");
                  var parameterStoreRepository = new ParameterStoreRepository(parameterStoreClient, SYSTEM_MANAGER_PATH);
                  return new CacheRepository(dynamoClient,
                      parameterStoreRepository.GetParameterStore(ParameterStoreConstants.CONFIGURATIONS_TABLE_NAME).GetAwaiter().GetResult(),
                      parameterStoreRepository.GetParameterStore(ParameterStoreConstants.TRANSACTIONS_TABLE_NAME).GetAwaiter().GetResult(),
                      parameterStoreRepository.GetParameterStore(ParameterStoreConstants.CLIENT_TABLE_NAME).GetAwaiter().GetResult());
              })
              .As<ICacheRepository>()
              .SingleInstance();

            builder
              .Register((context, parameters) =>
              {
                  var emailClient = context.ResolveNamed<IEmailClient>("EmailClient");
                  var parameterStoreClient = context.ResolveNamed<ISystemManagerClient>("SystemManagerClient");
                  var parameterStoreRepository = new ParameterStoreRepository(parameterStoreClient, SYSTEM_MANAGER_PATH);
                  return new EmailRepository(emailClient);
              })
              .As<IEmailRepository>()
              .SingleInstance();

            builder
              .Register((context, parameters) =>
              {
                  var smsClient = context.ResolveNamed<ISmsClient>("SmsClient");
                  var parameterStoreClient = context.ResolveNamed<ISystemManagerClient>("SystemManagerClient");
                  var parameterStoreRepository = new ParameterStoreRepository(parameterStoreClient, SYSTEM_MANAGER_PATH);
                  return new SmsRepository(smsClient);
              })
              .As<ISmsRepository>()
              .SingleInstance();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder
                .Register((context, parameters) => new PensionFundService(
                    context.Resolve<ICacheRepository>(),
                    context.Resolve<IRdsRepository>(),
                    context.Resolve<NotificationService>()
                    ))
                .As<PensionFundService>()
                .SingleInstance();

            builder
                .Register((context, parameters) => new NotificationService(
                    context.Resolve<IEmailRepository>(),
                    context.Resolve<ISmsRepository>()
                    ))
                .As<NotificationService>()
                .SingleInstance();
        }

        public static NpgsqlConnectionStringBuilder BuildNpgsqlConnectionString(ParameterStoreRepository parameterStoreRepository)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Host = parameterStoreRepository.GetParameterStore(ParameterStoreConstants.DB_HOST).GetAwaiter().GetResult();
            builder.Port = Convert.ToInt32(parameterStoreRepository.GetParameterStore(ParameterStoreConstants.DB_PORT).GetAwaiter().GetResult());
            builder.Database = parameterStoreRepository.GetParameterStore(ParameterStoreConstants.DB_NAME).GetAwaiter().GetResult();
            builder.Username = parameterStoreRepository.GetParameterStore(ParameterStoreConstants.DB_USER).GetAwaiter().GetResult();
            builder.Password = parameterStoreRepository.GetParameterStore(ParameterStoreConstants.DB_PASSWORD).GetAwaiter().GetResult();
            builder.ServerCompatibilityMode = ServerCompatibilityMode.Redshift;
            return builder;
        }
    }
}
