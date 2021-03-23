using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemsRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()           //UnitOfWork_StateUnderTest_ExpectedBehavior()
        {
            //Arrange
            //var repositoryStub = new Mock<IItemsRepository>(); this is above
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            //var loggerStub = new Mock<ILogger<ItemsController>>(); this is above

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);  

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            //Assert.IsType<NotFoundResult>(result.Result);
            result.Result.Should().BeOfType<NotFoundResult>();// this is better

        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()           //UnitOfWork_StateUnderTest_ExpectedBehavior()
        {
            // Arrange 
            var expectedItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);  

            //Act
            var result = await controller.GetItemAsync(Guid.NewGuid());
            
            //Assert
            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>());

            // this is not best practice
            // Assert.IsType<ItemDto>(result.Value);
            // var dto = (result as ActionResult<ItemDto>).Value;
            // Assert.Equal(expectedItem.Id, dto.Id);
            // Assert.Equal(expectedItem.Name, dto.Name);
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItem_ReturnsAllItems()           
        {
            // Arrange
            var expectedItems = new[] {CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);    
            // Act
            var actualItems = await controller.GetItemsAsync();
            
            // Assert
            actualItems.Should().BeEquivalentTo(
                expectedItems,
                options => options.ComparingByMembers<Item>());

        }

         [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()           
        {
            // Arrange
            var itemToCreate = new CreateItemDto( // new way to handle DTOs
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                rand.Next(1000));
            // {
            //     Name = Guid.NewGuid().ToString(),
            //     Price = rand.Next(1000)
            // };

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);    
            // Act
            var result = await controller.CreateItemAsync(itemToCreate);
            
            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNotContent()           
        {
            // Arrange
            Item existingItem = CreateRandomItem();
            
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                existingItem.Price + 3);
            // ) {
            //     Name = Guid.NewGuid().ToString(),
            //     Price = existingItem.Price + 3 
            // };

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);  
            // Act
            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);
            
            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNotContent()           
        {
            // Arrange
            Item existingItem = CreateRandomItem();
            
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);  
            // Act
            var result = await controller.DeleteItemAsync(existingItem.Id);
            
            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        private Item CreateRandomItem(){

            return new() {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
