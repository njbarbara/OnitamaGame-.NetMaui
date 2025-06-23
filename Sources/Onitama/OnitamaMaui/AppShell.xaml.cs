using OnitamaMaui.Pages;
namespace OnitamaMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("credits", typeof(CreditPage));
            Routing.RegisterRoute("historics", typeof(HistoricGamePage));
            Routing.RegisterRoute("home", typeof(HomePage));
            Routing.RegisterRoute("creation", typeof(CreatePage));
            Routing.RegisterRoute("selection", typeof(GameSelectionPage));
            Routing.RegisterRoute("game", typeof(GamePage));
            Routing.RegisterRoute("settings", typeof(SettingsPage));
            Routing.RegisterRoute("rules", typeof(RulePage));
            Routing.RegisterRoute("victory", typeof(VictoryPage));

        }
    }
}
