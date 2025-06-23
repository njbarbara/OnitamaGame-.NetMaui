using OnitamaMaui.Functionalities;

namespace OnitamaMaui.Views;

public partial class Header : ContentView
{	
	public static readonly BindableProperty TitleHeaderProperty = BindableProperty.Create(nameof(PropertyTitle), typeof(string), typeof(Header), string.Empty);

    private readonly ISoundManager _soundManager;

    public string PropertyTitle{
		get => (string)GetValue(Header.TitleHeaderProperty);
        set => SetValue(Header.TitleHeaderProperty, value);
    }

    public Header()
	{
        var services = Application.Current?.Handler.MauiContext?.Services;
        _soundManager = services?.GetService<ISoundManager>()!;
        InitializeComponent();
	}


    async void GoBack(object sender, EventArgs args)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("..");
    }

    async void GoToParameter(object sender, EventArgs args)
    {
        await _soundManager.PlaySound("pawnMove.mp3");
        await Shell.Current.GoToAsync("settings");
    }


}