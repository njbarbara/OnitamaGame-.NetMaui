namespace OnitamaMaui.Pages;
using OnitamaMaui.Functionalities;
public partial class SettingsPage : ContentPage
{
    private readonly ISoundManager _soundManager;

    public SettingsPage( )
    {
        var services = Application.Current?.Handler.MauiContext?.Services;
        _soundManager = services?.GetService<ISoundManager>()!;
        InitializeComponent();
    }


    async void GoBackArrow(object sender, EventArgs args)
    {
        if(Shell.Current.Navigation.NavigationStack.Count > 1)
        {
            await Shell.Current.Navigation.PopAsync();
            return;
        }
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("..");
    }

    private void OnSoundSlider_ValueChanged(object sender, ValueChangedEventArgs e) => _soundManager.SetGameSoundVolume((float)e.NewValue);
    private void OnMusic_ValueChanged(object sender, ValueChangedEventArgs e) => _soundManager.SetGameMusicVolume((float)e.NewValue);
}