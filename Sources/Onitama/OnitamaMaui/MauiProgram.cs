using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Onitama.Persistance.Stub;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using OnitamaMaui.Functionalities;
using OnitamaMaui.Pages;
using OnitamaPersistanceJson; // ajout pour utiliser la stub
using Plugin.Maui.Audio;


namespace OnitamaMaui
{

    public class GameSettings
    {
        public GameMod CurrentGameMod { get; set; } = GameMod.Classique;
    }


    public static class MauiProgram
    {
        public static IServiceProvider? Services { get; private set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("SF_Shai_Fontai.ttf", "Shai_Fonts");
                    fonts.AddFont("OrchideeMedium.ttf", "OrchideeMedium");
                });
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddSingleton<ISoundManager, SoundManager>();

            builder.Services.AddTransient<HomePage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif





            builder.Services.AddTransient<GameSettings>();
            builder.Services.AddTransient<IGameBoardManager, ClassicGameBoardManager>();
            builder.Services.AddTransient<IDeckManager, DeckManager>();
            builder.Services.AddSingleton<IloadManager, JsonLoadManager>();
            builder.Services.AddSingleton<ISaveManager, JsonSaveManager>();


            builder.Services.AddTransient<IGameManager>(provider =>
            {
                var settings = provider.GetRequiredService<GameSettings>();
                var boardManager = provider.GetRequiredService<IGameBoardManager>();

                IMoveManager moveManager = settings.CurrentGameMod == GameMod.Classique
                    ? new ClassicBoardMoveManager()
                    : new ApocalypseBoardMoveManager(5, 5);

                var deckManager = provider.GetRequiredService<IDeckManager>();
                var loadManager = provider.GetRequiredService<IloadManager>();
                var saveManager = provider.GetRequiredService<ISaveManager>();

                return new GameManager(boardManager, moveManager, deckManager, loadManager, saveManager);
            });


            builder.Services.AddTransient<GamePage>();


            builder.Services.AddTransient<CreatePage>();

            var app = builder.Build();

            Services = app.Services;

            return app;
        }
    }

}