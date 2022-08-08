using Prism.Mvvm;

namespace PrismApp.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private string _title = "Prism Application";

    public MainWindowViewModel()
    {
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}