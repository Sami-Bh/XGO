using XGOMobile.ViewModels;

namespace XGOMobile.Views;

public partial class Products : ContentPage
{
    public Products(ProductsViewModel productsViewModel)
    {
        InitializeComponent();
        BindingContext = productsViewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        (BindingContext as ProductsViewModel).RefreshPageAsync();
    }
}