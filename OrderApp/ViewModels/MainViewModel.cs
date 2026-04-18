using OrderApp.Commands;
using OrderApp.Models;
using OrderApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace OrderApp.ViewModels;

public class MainViewModel : BaseViewModel, IDataErrorInfo
{
    private readonly IApiService _apiService;
    private readonly IMessageService _messageService;

    public ObservableCollection<Item> Items { get; set; } = [];

    public ObservableCollection<string> States { get; set; } = ["KA", "MH", "TN", "AP", "TS"];

    private bool _isQuantityTouched;
    private int? _quantity;
    public int? Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            _isQuantityTouched = true;
            OnPropertyChanged(nameof(Quantity));
        }
    }

    private string _city;
    public string City
    {
        get => _city;
        set
        {
            _city = value;
            OnPropertyChanged(nameof(City));
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set
        {
            _isSubmitting = value;
            OnPropertyChanged(nameof(IsSubmitting));
        }
    }

    private Item _selectedItem;
    public Item SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    private string _selectedState;
    public string SelectedState
    {
        get => _selectedState;
        set
        {
            _selectedState = value;
            OnPropertyChanged(nameof(SelectedState));
        }
    }

    private string _loaderMessage;
    public string LoaderMessage
    {
        get => _loaderMessage;
        set
        {
            _loaderMessage = value;
            OnPropertyChanged(nameof(LoaderMessage));
        }
    }

    public ICommand PlaceOrderCommand { get; set; }

    public MainViewModel(IApiService apiService, IMessageService messageService)
    {
        _apiService = apiService;
        _messageService = messageService;

        PlaceOrderCommand = new RelayCommand(PlaceOrder, CanPlaceOrder);

        Items.Insert(0, new Item { Id = 0, Name = "Select an Item" });
        States.Insert(0, "Select a state");
        SelectedItem = Items.First();
        SelectedState = States.First();
    }

    public async Task IntializeLoadItemsAsync()
    {
        await LoadItems();
    }

    private bool CanPlaceOrder()
    {
        return !IsSubmitting && SelectedItem.Id > 0 && Quantity.HasValue && Quantity.Value >= 1 && Quantity.Value <= 100
            && !string.IsNullOrEmpty(City) && City.Length <= 50 && SelectedState != "Select a state";
    }

    private async Task PlaceOrder()
    {
        try
        {
            IsSubmitting = true;
            IsLoading = true;
            LoaderMessage = "Placing items";

            var order = new Order
            {
                ItemId = SelectedItem?.Id ?? 0,
                Quantity = Quantity.Value,
                City = City,
                State = SelectedState
            };

            var result = await _apiService.SubmitOrderAsync(order);

            if (result)
            {
                IsLoading = false;
                _messageService.ShowInfoMessage("Order placed successfully!");
            }
            else
            {
                _messageService.ShowInfoMessage("Order failed");
            }
        }
        finally
        {
            IsSubmitting = false;
            IsLoading = false;
        }
    }

    private async Task LoadItems()
    {
        try
        {
            IsLoading = true;
            LoaderMessage = "Loading items";
            List<Item> items = await _apiService.GetItemsAsync();
            items.ForEach(item => Items.Add(item));
        }
        catch (Exception ex)
        {
            _messageService.ShowInfoMessage($"Failed to load items: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public string Error => null;
    public string this[string columnName] => columnName switch
    {
        nameof(Quantity) when _isQuantityTouched && !Quantity.HasValue => "Quantity is required",

        nameof(Quantity) when _isQuantityTouched && (Quantity.Value < 1 || Quantity.Value > 100)
            => "Quantity must be between 1 and 100.",

        nameof(City) when !string.IsNullOrWhiteSpace(City) && City.Length > 50 => "City must be <= 50 characters",

        nameof(City) when City != null && string.IsNullOrWhiteSpace(City) => "City is required",

        _ => null
    };
}
