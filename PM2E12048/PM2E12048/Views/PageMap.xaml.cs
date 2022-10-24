using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Essentials;
using Plugin.Geolocator;

namespace PM2E12048.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMap : ContentPage
    {
        public PageMap()
        {
            InitializeComponent();
        }

        Plugin.Media.Abstractions.MediaFile Filefoto = null;

        protected async override void OnAppearing()
        {

            base.OnAppearing();
            try
            {
                var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var tokendecancelacion = new System.Threading.CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                if (location != null)
                {

                    var lat = Convert.ToDouble(mtxtLat.Text);
                    var lon = Convert.ToDouble(mtxtLon.Text);
                    var Nomsitio = nomSitio.Text;

                    var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        var geocodeAddress =
                            $"Pais:     {placemark.CountryName}\n" +
                            $"Depto:       {placemark.AdminArea}\n" +
                            $"Ciudad:    {placemark.SubAdminArea}\n" +
                            $"Colonia:        {placemark.Locality}\n" +
                            $"Direccion:    {placemark.Thoroughfare}\n";

                       await DisplayAlert("Ubicacion", geocodeAddress, "ok");

                        var pin = new Pin()
                        {
                            Position = new Position(lat, lon),
                            Label = Nomsitio,

                        };

                        Mapa.Pins.Add(pin);
                        Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromMeters(100.00)));

                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Advertencia", "Este dispositivo no soporta GPS" + fnsEx, "Ok");
            }
            catch (FeatureNotEnabledException)
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



        }

        private async void btnCompartir_Clicked(object sender, EventArgs e)
        {

            var geocodeAddress = "";

            var lat = Convert.ToDouble(mtxtLat.Text);
            var lon = Convert.ToDouble(mtxtLon.Text);
            var Nomsitio = nomSitio.Text;

            var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                geocodeAddress =
               $"\nPais:     {placemark.CountryName}\n" +
               $"Depto:       {placemark.AdminArea}\n" +
               $"Ciudad:    {placemark.SubAdminArea}\n" +
               $"Colonia:        {placemark.Locality}\n" +
               $"Direccion:    {placemark.Thoroughfare}\n";
            }

            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {

                    Title = "Compartiendo ubicacion \n",
                    Text = "Lugar " + Nomsitio + geocodeAddress + " \n",
                    Uri = "https://maps.google.com/?q=" + lon + "," + lat

                });
            }
            catch
            {

            }

        }
    }
}