using AppServiceNet5.Dtos;
using AppServiceNet5.Entities;
using AppServiceNet5.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServiceNet5.Controllers
{
    [Route("api/[controller]")] // or you can change "items"
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IInMemItemsRepository _repository;

        public ItemsController(IInMemItemsRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await _repository.GetItemsAsync())
                    .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreateDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            Item UpdateItem = existingItem with // this with new feature goes from RECORDS 
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await _repository.UpdateItemAsync(UpdateItem);

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await _repository.GetItemAsync(id);
            await _repository.DeleteItemAsync(existingItem.Id);

            return NoContent();
        }
    }
}
