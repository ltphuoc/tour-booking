using Autofac;
using BusinessObject.Repository;
using BusinessObject.UnitOfWork;
using BusinessObjects.Services;
using DataAccess.Services;

namespace DataAccess.Helpers
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();

            builder.RegisterType<AccountServices>().As<IAccountServices>();
            builder.RegisterType<DestinationServices>().As<IDestinationServices>();
            builder.RegisterType<DestinationImageServices>().As<IDestinationImageServices>();
            builder.RegisterType<TourServices>().As<ITourSevices>();
            builder.RegisterType<AuthServices>().As<IAuthServices>();
            builder.RegisterType<JwtAuthenticationManager>();
            builder.RegisterType<BookingServices>().As<IBookingSevices>();
            builder.RegisterType<TourDetailServices>().As<ITourDetailSevices>();
            builder.RegisterType<TourGuideServices>().As<ITourGuideSevices>();
            builder.RegisterType<TourPriceServices>().As<ITourPriceSevices>();
            builder.RegisterType<PaymentServices>().As<IPaymentSevices>();
            builder.RegisterType<TransportationServices>().As<ITransportationSevices>();
            builder.RegisterType<JwtAuthenticationManager>().As<IJwtAuthenticationManager>();

        }
    }
}
