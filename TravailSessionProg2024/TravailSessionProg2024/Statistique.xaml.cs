using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TravailSessionProg2024
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Statistique : Page
    {
        
        //private singleton singleton;

        public Statistique()
        {
            this.InitializeComponent();
            //singleton = singleton.GetInstance;
            ChargerStatistiques();
        }

        private void ChargerStatistiques()
        {
            // Nombre total d'adhérents

            // Nombre total d'activités

            // Nombre d'adhérents par activité

            // Moyenne des notes par activité

            // Moyenne du prix par activité

            // Revenu total d'une activité

            // Total depensé par adhérent
        }
    }
}
