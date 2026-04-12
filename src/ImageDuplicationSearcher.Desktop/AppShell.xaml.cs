namespace ImageDuplicationSearcher.Desktop;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        InitializeComponent();
        Items.Add(new ShellContent
        {
            Title = string.Empty,
            Route = nameof(MainPage),
            Content = mainPage
        });
    }
}
