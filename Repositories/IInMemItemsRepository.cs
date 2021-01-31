using AppServiceNet5.Entities;
using System;
using System.Collections.Generic;

namespace AppServiceNet5.Repositories
{
    public interface IInMemItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();

        void CreateItem(Item item);

    }
}