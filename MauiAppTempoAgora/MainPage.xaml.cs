using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);
                    if (t != null)
                    {
                        string dados_previsão = "";

                        dados_previsão = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Descrição: {t.description} \n" +
                                         $"Clima: {t.main} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n" +
                                         $"Velocidade do vento: {t.speed} m/s\n" +
                                         $"Visibilidade: {t.visibility} metros\n";

                        lbl_res.Text = dados_previsão;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }
                    {

                    }
                }
                else
                {
                    lbl_res.Text = "Digite a cidade";
                }

            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Sem conexão", httpEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}
