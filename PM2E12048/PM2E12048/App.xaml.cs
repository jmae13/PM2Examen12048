using PM2E12048.Controller;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin.Essentials;

namespace PM2E12048
{
    public partial class App : Application
    {

        static BDSitios db;

        public static BDSitios SitioBD
        {
            get
            {
                //se declara la ruta donde estan los archivos de la BD
                if (db == null)
                {
                    String FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PM2E12048.db3");//db3 es extencion de sqlite
                    db = new BDSitios(FolderPath);
                }

                return db;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
