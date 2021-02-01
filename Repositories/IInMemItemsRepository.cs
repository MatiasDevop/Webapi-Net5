using AppServiceNet5.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServiceNet5.Repositories
{
    public interface IInMemItemsRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();

        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);

        Task DeleteItemAsync(Guid id);

    }
}