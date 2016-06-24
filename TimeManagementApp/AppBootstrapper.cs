using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TimeManagementApp.ViewModels;

namespace TimeManagementApp 
{
	public class AppBootstrapper : BootstrapperBase 
	{
        private SimpleContainer _container = new SimpleContainer();
        public AppBootstrapper()
		{
			Initialize();

            _container.Instance<IWindowManager>(new WindowManager());
            _container.Singleton<IEventAggregator, EventAggregator>();

            _container.PerRequest<ShellViewModel>();
            _container.Singleton<SettingsViewModel>();
            _container.GetInstance<SettingsViewModel>().LoadSettings();
		}

		protected override void OnStartup(object sender, StartupEventArgs e) 
		{
			DisplayRootViewFor<ShellViewModel>();
		}

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.GetInstance<SettingsViewModel>().WriteSettings();
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            return instance;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var objects = _container.GetAllInstances(service);
            return objects;
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }
}
