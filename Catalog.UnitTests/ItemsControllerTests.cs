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

        private Item CreateRandomItem(){

            return new() {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
