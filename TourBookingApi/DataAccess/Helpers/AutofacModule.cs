using Autofac;
using BusinessObject.Repository;
using BusinessObject.UnitOfWork;
using DataAccess.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            builder.RegisterType<TourServices>().As<ITourSevices>();
            builder.RegisterType<BookingServices>().As<IBookingSevices>();
            builder.RegisterType<TourDetailServices>().As<ITourDetailSevices>();
            builder.RegisterType<TourGuideServices>().As<ITourGuideSevices>();
            builder.RegisterType<TourPriceServices>().As<ITourPriceSevices>();
        }
    }
}
