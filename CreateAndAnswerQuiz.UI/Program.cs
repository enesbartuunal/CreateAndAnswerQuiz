using Blazored.LocalStorage;
using Blazored.Toast;
using CreateAndAnswerQuiz.UI.AuthProvider;
using CreateAndAnswerQuiz.UI.Services.Base;
using CreateAndAnswerQuiz.UI.Services.Implemantation;
using CreateAndAnswerQuiz.UI.Services.Implementation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44392/api/") });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddBlazoredToast();
            builder.Services.AddScoped<IAuthenticationHttpService, AuthenticationHttpService>();
            builder.Services.AddScoped<IQuizHttpService,QuizHttpService>();
            builder.Services.AddScoped<RefreshTokenService>();
            await builder.Build().RunAsync();
        }
    }
}
