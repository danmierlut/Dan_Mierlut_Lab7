using Dan_Mierlut_Lab7.Models;
namespace Dan_Mierlut_Lab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem != null)
        {
            var selectedProduct = listView.SelectedItem as Product;
            var confirmed = await DisplayAlert("Confirmation", $"Delete {selectedProduct.Description}?", "Yes", "No");
            if (confirmed)
            {
                await App.Database.DeleteProductAsync(selectedProduct);
                var shopList = (ShopList)BindingContext;
                listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
            }
        }
    }
}