using OrderApp.Models;

namespace OrderApp.Services;

public class ApiService : IApiService
{
    public async Task<List<Item>> GetItemsAsync()
    {
        await Task.Delay(1500);
        return
        [
            new Item { Id = 1, Name = "Laptop" },
            new Item { Id = 2, Name = "Phone" },
            new Item { Id = 3, Name = "Headphones" }
        ];
    }

    public async Task<bool> SubmitOrderAsync(Order order)
    {
        await Task.Delay(1000);
        return true;
    }
}
