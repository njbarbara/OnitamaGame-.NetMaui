namespace OnitamaMaui.Pages;
using OnitamaMaui.Functionalities;

public partial class HomePage : ContentPage
{
    private readonly ISoundManager _soundManager;

    public HomePage()
	{
        var services = Application.Current?.Handler.MauiContext?.Services;
        _soundManager = services?.GetService<ISoundManager>()!;
        _soundManager?.PlayMusic("background_music.mp3");		
        InitializeComponent();
    }

     
    private async void PlayerButton(object sender, EventArgs args)
	{
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("selection");
    }

    private async void HistoricButton(object sender, EventArgs args)
	{
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("historics");
    }

    private async void CreditButton(object sender, EventArgs args)
	{
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("credits");
    }

    private async void SettingsButton(object sender, EventArgs args)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("settings");
    }
    private async void RuleButton(object sender, EventArgs args)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("rules");
    }

    private async void CloseApp(object sender, EventArgs args)
	{
        await _soundManager.PlaySound("pawnMove.mp3");
        bool results = await DisplayAlert("Attention", "Do you want to quit the app ?", "Yes", "No");
        if (results) Application.Current?.Quit();
    }

}