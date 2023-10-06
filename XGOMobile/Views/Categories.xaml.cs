using XGOMobile.ViewModels;

namespace XGOMobile.Views;

public partial class Categories : ContentPage
{
    public Categories(CategoriesViewModel categoriesViewModel)
    {
        BindingContext = categoriesViewModel;

        InitializeComponent();


    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        (BindingContext as CategoriesViewModel).RefreshPageDataAsync();
    }
}