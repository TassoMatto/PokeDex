namespace CustomControl.Views.controls;

public partial class FilterButton : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(FilterButton),
            string.Empty);

    public string Text 
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public FilterButton()
    {
        InitializeComponent();
    }
}