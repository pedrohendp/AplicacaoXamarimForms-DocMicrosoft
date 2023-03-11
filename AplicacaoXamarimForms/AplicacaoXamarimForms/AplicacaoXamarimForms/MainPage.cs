using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AplicacaoXamarimForms
{
    public class MainPage : ContentPage
    {
        Entry TextoNumeroTelefone;
        Button ButtonTraducao;
        Button ButtonChamada;
        string NumeroTraduzido;
        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout painel = new StackLayout()
            {
                Spacing = 15
            };

            painel.Children.Add(new Label
            {
                Text = "Insira um Número de telefone:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            });

            painel.Children.Add(TextoNumeroTelefone = new Entry
            {
                Text = "1-855-XAMARIN",
                HorizontalTextAlignment = TextAlignment.Center, 
                VerticalTextAlignment = TextAlignment.Center,
            });

            painel.Children.Add(ButtonTraducao = new Button
            {
                Text = "Traduzir"
            });

            painel.Children.Add(ButtonChamada = new Button
            {
                Text = "Ligar Agora",
                IsEnabled = false,
            });

            ButtonTraducao.Clicked += OnTranslate;
            ButtonChamada.Clicked += OnCall;
            this.Content = painel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string numeroInserido = TextoNumeroTelefone.Text;
            NumeroTraduzido = Core.PhonewordTranslator.ToNumber(numeroInserido);

            if (!string.IsNullOrEmpty(NumeroTraduzido))
            {
                ButtonChamada.IsEnabled = true;
                ButtonChamada.Text = $"Chamando {NumeroTraduzido}";
            }
            else
            {
                ButtonChamada.IsEnabled = false;
                ButtonChamada.Text = "Chamar";
            }
        }

        async void OnCall(object sender, EventArgs e)
        {
            if (await this.DisplayAlert(
                "Disque um número",
                $"Gostaria de ligar para {NumeroTraduzido} ?",
                "Sim",
                "Não"))
            {
                try
                {
                    PhoneDialer.Open(NumeroTraduzido);
                }
                catch(ArgumentNullException)
                {
                    await DisplayAlert("Impossível ligar", $"O número de telefone {NumeroTraduzido} não é válido.", "Ok");
                }
                catch(FeatureNotSupportedException)
                {
                    await DisplayAlert("Impossível ligar", "O discador padrão não é suportado", "Ok");
                }
                catch (Exception) 
                {
                    //Outros erros podem acontecer
                    await DisplayAlert("Impossível ligar", "Discagem falhou", "Ok");
                }
            }
        }
    }
}
