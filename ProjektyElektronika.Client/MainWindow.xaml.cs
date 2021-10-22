using Newtonsoft.Json;
using ProjektyElektronika.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektyElektronika.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var http = new HttpClient();
                var str = http.GetStringAsync("https://localhost:5001/weatherforecast").Result;
                var obiekt = JsonConvert.DeserializeObject<WeatherForecast[]>(str);
                pole.Text = $"temperatura na polu: {obiekt[0].TemperatureC} C";
            } catch (Exception ex)
            {
                pole.Text = ex.ToString();
            }
        }
    }
}
