namespace OnitamaMaui.Views;

public partial class GameSection : ContentView
{
    public static readonly BindableProperty PropertyButtonTitleProperty =
        BindableProperty.Create(nameof(PropertyButtonTitle), typeof(string), typeof(GameSection), string.Empty);

    public string PropertyButtonTitle
    {
        get => (string)GetValue(PropertyButtonTitleProperty);
        set => SetValue(PropertyButtonTitleProperty, value);
    }

    public GameSection()
    {
        InitializeComponent();
        BindingContext = this;
    }
}