using OrderApp.Models;

namespace OrderApp.Services;

public interface IApiService
{
    Task<List<Item>> GetItemsAsync();
    Task<bool> SubmitOrderAsync(Order order);
}
