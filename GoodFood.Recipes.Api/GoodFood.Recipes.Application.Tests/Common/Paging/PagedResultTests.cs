using System;
using AutoFixture;
using FluentAssertions;
using GoodFood.Recipes.Application.Common.Paging;
using Xunit;

namespace GoodFood.Recipes.Application.Tests.Common.Paging
{
    public class PagedResultTests
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public void PagedResult_NullPagedRequest_ThrowsException()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => new PagedResult<object>(null, 10, this.fixture.CreateMany<object>(5)));
        }

        [Theory]
        [InlineData(111, 5, 23)]
        [InlineData(97, 7, 14)]
        [InlineData(0, 5, 0)]
        [InlineData(1, 5, 1)]
        public void PagedResult_ValidInput_ReturnsExpectedValues(int totalItems, int pageSize, int expectedTotalPages)
        {
            // Arrange
            var pagedRequest = new PagedRequest { CurrentPage = 1, PageSize = pageSize };

            // Act
            var pagedResult = new PagedResult<object>(pagedRequest, totalItems, this.fixture.CreateMany<object>(pageSize));

            // Assert
            pagedResult.CurrentPage.Should().Be(pagedRequest.CurrentPage);
            pagedResult.PageSize.Should().Be(pagedRequest.PageSize);
            pagedResult.TotalItems.Should().Be(totalItems);
            pagedResult.TotalPages.Should().Be(expectedTotalPages);
        }
    }
}