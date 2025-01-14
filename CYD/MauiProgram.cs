using CommunityToolkit.Maui; // <--- Użycie narzędzi z CommunityToolkit dla aplikacji Maui
using Microsoft.Extensions.Logging; // <--- Użycie przestrzeni nazw do logowania
using CYD.Pages; // <--- Użycie przestrzeni nazw aplikacji (strony aplikacji)
using Syncfusion.Maui.Toolkit.Hosting; // <--- Użycie narzędzi Syncfusion dla Maui
using Microsoft.Maui.Controls.Hosting; // <--- Użycie przestrzeni nazw do tworzenia aplikacji Maui

namespace CYD
{
    /** @brief Główna klasa konfigurująca Maui */
    public static class MauiProgram
    {
        /** @brief Tworzenie aplikacji Maui */
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder(); // <--- Tworzymy buildera aplikacji Maui

            builder
                .UseMauiApp<App>() // <--- Ustawienie głównej aplikacji (klasy App) dla aplikacji Maui
                .UseMauiCommunityToolkit() // <--- Dodanie narzędzi z CommunityToolkit do aplikacji Maui
                .ConfigureSyncfusionToolkit() // <--- Dodanie narzędzi Syncfusion do aplikacji Maui
                .ConfigureMauiHandlers(handlers => // <--- Konfiguracja niestandardowych handlerów (brak ich w tej konfiguracji)
                {
                })
                .ConfigureFonts(fonts => // <--- Konfiguracja czcionek, które będą dostępne w aplikacji
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // <--- Dodanie czcionki OpenSans-Regular
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); // <--- Dodanie czcionki OpenSans-Semibold
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold"); // <--- Dodanie czcionki SegoeUI-Semibold
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily); // <--- Dodanie czcionki FluentSystemIcons-Regular
                });

#if DEBUG // <--- Tylko w trybie debugowania
            builder.Logging.AddDebug(); // <--- Dodanie logowania debugowego
            builder.Services.AddLogging(configure => configure.AddDebug()); // <--- Konfiguracja logowania debugowego
#endif

            /** @brief Rejestracja usług w kontenerze DI (Dependency Injection) */
            builder.Services.AddSingleton<ProjectRepository>(); // <--- Rejestracja repozytorium projektów jako singleton
            builder.Services.AddSingleton<TaskRepository>(); // <--- Rejestracja repozytorium zadań jako singleton
            builder.Services.AddSingleton<CategoryRepository>(); // <--- Rejestracja repozytorium kategorii jako singleton
            builder.Services.AddSingleton<TagRepository>(); // <--- Rejestracja repozytorium tagów jako singleton
            builder.Services.AddSingleton<SeedDataService>(); // <--- Rejestracja usługi do inicjalizacji danych jako singleton
            builder.Services.AddSingleton<ModalErrorHandler>(); // <--- Rejestracja usługi obsługi błędów modalnych jako singleton
            builder.Services.AddSingleton<MainPageModel>(); // <--- Rejestracja modelu strony głównej jako singleton
            builder.Services.AddSingleton<ProjectListPageModel>(); // <--- Rejestracja modelu strony listy projektów jako singleton
            builder.Services.AddSingleton<ManageMetaPageModel>(); // <--- Rejestracja modelu strony zarządzania metadanymi jako singleton

            /** @brief Rejestracja stron z trasami (routing) jako transient (krótkoterminowe) */
            builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project"); // <--- Rejestracja strony szczegółów projektu z trasą
            builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task"); // <--- Rejestracja strony szczegółów zadania z trasą

            return builder.Build(); // <--- Budowanie i zwrócenie aplikacji Maui
        }
    }
}
