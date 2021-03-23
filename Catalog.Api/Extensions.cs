
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            // new form or way to do
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
            // {
            //     Id = item.Id,
            //     Name = item.Name,
            //     Price = item.Price,
            //     CreatedDate = item.CreateDate
            // };
        }
    }
}
