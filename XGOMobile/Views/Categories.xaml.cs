using XGOMobile.ViewModels;

namespace XGOMobile.Views;

public partial class Categories : ContentPage
{
	public Categories(CategoriesViewModel categoriesViewModel)
	{
        BindingContext = categoriesViewModel;

        InitializeComponent();

    }
}