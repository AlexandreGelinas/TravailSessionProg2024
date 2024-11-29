using Microsoft.UI.Xaml;

namespace TravailSessionProg2024
{
    /// <summary>
    /// Fournit un comportement spécifique à l'application pour compléter la classe Application par défaut.
    /// </summary>
    public partial class App : Application
    {
        // Propriété pour référencer la fenêtre principale
        public static Window MainWindow { get; private set; }

        /// <summary>
        /// Initialise l'application.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Méthode appelée lorsque l'application est lancée.
        /// </summary>
        /// <param name="args">Détails concernant la demande de lancement et le processus.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }
    }
}
