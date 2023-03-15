using Autofac;
using Autofac.Integration.Mvc;
using NetTrackBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TYT.Attributes;

namespace TSS___TrackYourTruck_sales_support
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;

            base.Init();
        }

        private void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            GSA.Security.GSAFormsAuthenticationService.AttachRolesToUser();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExitHttpsIfNotRequiredAttribute());
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        public static IDependencyResolver RegisterDI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterInstance<IFacadeBiz>(new TytFacadeBiz()); //Singleton
            //builder.RegisterInstance(new TytFacadeBiz()).As<IFacadeBiz>(); //Singleton
            //builder.RegisterType<TytFacadeBiz>().As<IFacadeBiz>().SingleInstance();
            //builder.Register<IFacadeBiz>(r => new TytFacadeBiz()).SingleInstance();

            IContainer container = builder.Build();
            return new AutofacDependencyResolver(container);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(RegisterDI());
        }
    }
}