//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using CommonServiceLocator;
using Eutyches.Spell.Lexedit.Services;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace Eutyches.Spell.Lexedit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region Methods

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected override Window CreateShell()
        {
            var window = ServiceLocator.Current.GetInstance<MainWindow>();

            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            return window;
        }

        /// <summary>
        /// Used to register types with the container that will be used by your application.
        /// </summary>
        /// <param name="containerRegistry">The container registry.</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var eventAggregator = Container.Resolve<IEventAggregator>();

            // Services
            containerRegistry.RegisterInstance<IFileService>(new FileService(eventAggregator));
            containerRegistry.RegisterInstance<IDialogService>(new DialogService());
            containerRegistry.RegisterInstance<IToolService>(new ToolService());
        }

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the PrismApplication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance
        /// containing the event data.
        /// </param>
        private void PrismApplication_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Using a regular MessageBox
            MessageBox.Show(App.Current.MainWindow,
                e.Exception.ToString(),
                Lexedit.Properties.Resources.ErrorDialogTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true;
        }

        #endregion Methods
    }
}