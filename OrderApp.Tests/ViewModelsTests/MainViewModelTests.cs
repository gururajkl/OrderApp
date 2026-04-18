using Moq;
using OrderApp.Commands;
using OrderApp.Models;
using OrderApp.Services;
using OrderApp.ViewModels;
using Xunit;

namespace OrderApp.Tests.ViewModelsTests;

public class MainViewModelTests
{
    public MainViewModel CreateViewModel()
    {
        var apiMockservice = new Mock<IApiService>();
        var messageMockservice = new Mock<IMessageService>();

        apiMockservice.Setup(m => m.GetItemsAsync()).ReturnsAsync([
            new Item { Id = 1, Name = "Laptop" }
        ]);

        apiMockservice.Setup(m => m.SubmitOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

        return new(apiMockservice.Object, messageMockservice.Object);
    }

    [Fact]
    public void PlaceOrderCommand_ShouldBeEnabled_WhenDataIsValid()
    {
        var viewModel = CreateViewModel();

        viewModel.SelectedItem = new Item { Id = 1 };
        viewModel.Quantity = 2;
        viewModel.City = "Bangalore";
        viewModel.SelectedState = "KA";

        var result = viewModel.PlaceOrderCommand.CanExecute(null);

        Assert.True(result);
    }

    [Fact]
    public void PlaceOrderCommand_ShouldBeDisabled_WhenQuantityInvalid()
    {
        var viewModel = CreateViewModel();

        viewModel.SelectedItem = new Item { Id = 1 };
        viewModel.Quantity = 200;
        viewModel.City = "Bangalore";
        viewModel.SelectedState = "KA";

        var result = viewModel.PlaceOrderCommand.CanExecute(null);

        Assert.False(result);
    }

    [Fact]
    public void PlaceOrderCommand_ShouldBeDisabled_WhenItemNotSelected()
    {
        var viewModel = CreateViewModel();

        viewModel.SelectedItem = new Item { Id = 0 };
        viewModel.Quantity = 2;
        viewModel.City = "Bangalore";
        viewModel.SelectedState = "KA";

        var result = viewModel.PlaceOrderCommand.CanExecute(null);

        Assert.False(result);
    }

    [Fact]
    public void PlaceOrderCommand_ShouldBeDisabled_WhenStateNotSelected()
    {
        var vm = CreateViewModel();

        vm.SelectedItem = new Item { Id = 1 };
        vm.Quantity = 2;
        vm.City = "Bangalore";
        vm.SelectedState = "Select a state";

        var result = vm.PlaceOrderCommand.CanExecute(null);

        Assert.False(result);
    }

    [Fact]
    public void City_ShouldReturnError_WhenEmpty()
    {
        var viewModel = CreateViewModel();
        viewModel.City = "";
        var error = viewModel["City"];
        Assert.NotNull(error);
    }

    [Fact]
    public async Task PlaceOrder_ShouldCallSubmitOrderAsync()
    {
        var mockService = new Mock<IApiService>();
        var messageMockservice = new Mock<IMessageService>();

        mockService.Setup(m => m.SubmitOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

        var viewModel = new MainViewModel(mockService.Object, messageMockservice.Object);

        viewModel.SelectedItem = new Item { Id = 1 };
        viewModel.Quantity = 2;
        viewModel.City = "Bangalore";
        viewModel.SelectedState = "KA";

        await ((RelayCommand)viewModel.PlaceOrderCommand).ExecuteAsync();

        mockService.Verify(x => x.SubmitOrderAsync(It.IsAny<Order>()), Times.Once);
        messageMockservice.Verify(m => m.ShowInfoMessage(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LoadItems_ShouldPopulateItems()
    {
        var mockService = new Mock<IApiService>();
        var mockMessageService = new Mock<IMessageService>();

        mockService.Setup(m => m.GetItemsAsync()).ReturnsAsync([
            new Item { Id = 1, Name = "Laptop" },
            new Item { Id = 2, Name = "Phone" }
        ]);

        var viewModel = new MainViewModel(mockService.Object, mockMessageService.Object);

        await viewModel.IntializeLoadItemsAsync();

        Assert.True(viewModel.Items.Any());
    }
}
