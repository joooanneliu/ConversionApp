using System;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace ConversionApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set => this.RaiseAndSetIfChanged(ref _inputText, value);
        }

        private string _greeting;
        public string Greeting
        {
            get => _greeting;
            set => this.RaiseAndSetIfChanged(ref _greeting, value);
        }

        public ReactiveCommand<Unit, Unit> GetGreetingCommand { get; }

        public MainWindowViewModel()
        {
            _httpClient = new HttpClient();

            GetGreetingCommand = ReactiveCommand.CreateFromTask(GetServerGreeting);

            // Example default value for InputText
            InputText = "0";
        }

        private async Task GetServerGreeting()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5000/output?text={InputText}");
                response.EnsureSuccessStatusCode();
                Greeting = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Greeting = "Failed to fetch response from server";
            }
        }
    }
}
