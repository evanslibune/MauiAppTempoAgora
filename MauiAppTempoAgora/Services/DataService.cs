﻿using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(String cidade)
        {
            Tempo? t = null;

            string chave = "40f1fbd30a2ead3015971528c7e99a4c";
            
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (resp.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                         resp.StatusCode == System.Net.HttpStatusCode.RequestTimeout ||
                         resp.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    throw new HttpRequestException("Você não esta conectado com a internet.");
                }
                else if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString()
                    }; //fecha obj do tempo
                }//fecha if se o status do servidor for sucesso
            }//fecha using

            return t;
        }
    }
}
