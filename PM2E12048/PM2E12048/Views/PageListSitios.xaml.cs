using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Diagnostics;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Xamarin.Essentials;
using PM2E12048.Models;

namespace PM2E12048.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListSitios : ContentPage
    {
        public PageListSitios()
        {
            InitializeComponent();
        }
        private async void Cargar_Sitios()

        {
            var sitios = await App.SitioBD.getListSitio();
            Lista.ItemsSource = sitios;
        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {


            bool answer = await DisplayAlert("Aviso de Confirmación", "¿Desea eliminar este registro?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                var idSitio = (Sitios)(sender as MenuItem).CommandParameter;
                var result = await App.SitioBD.DeleteSitio(idSitio);

                if (result == 1)
                {
                  await  DisplayAlert("Aviso", "Registro Eliminado", "OK");
                    Cargar_Sitios();
                }
                else
                {
                  await   DisplayAlert("Aviso", "Revisa", "OK");
                }
            };

        }

        private async void Lista_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            var sitio = (Sitios)e.Item;


            bool answer = await DisplayAlert("AVISO", "¿Desea ver la ubicacion de esta imagen en el mapa?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {

                PageMap map = new PageMap();
                map.BindingContext = sitio;
                await Navigation.PushAsync(map);
            };

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Lista.ItemsSource = await App.SitioBD.getListSitio();
        }

        private async void IrMapa_Clicked(object sender, EventArgs e)
        {
            var idSitio = (Sitios)(sender as MenuItem).CommandParameter;
           

            bool answer = await DisplayAlert("AVISO", "¿Desea ir al mapa?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {
                try
                {
                    var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var tokendecancelacion = new System.Threading.CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                    if (location != null)
                    {

                        PageMap map = new PageMap();
                        await Navigation.PushAsync(map);
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("Advertencia", "Este dispositivo no soporta GPS" + fnsEx, "Ok");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("Advertencia", "Error de Dispositivo, validar si el GPS esta activo", "Ok");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("Advertencia", "Sin Permisos de Geolocalizacion" + pEx, "Ok");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Advertencia", "Sin Ubicacion " + ex, "Ok");
                }
            };
        }


    }
}