using FluentAssertions;
using GoodFood.Recipes.Application.Common.Paging;
using Xunit;

namespace GoodFood.Recipes.Application.Tests.Common.Paging
{
    public class PagedRequestTests
    {
        [Theory]
        [InlineData(1, 5, 0)]
        [InlineData(9, 5, 40)]
        public void PagedRequest_ValidInput_ReturnsExpectedValues(int currentPage, int pageSize, int expectedRowsToSkip)
        {
            // Act
            var pagedRequest = new PagedRequest { CurrentPage = currentPage, PageSize = pageSize };

            // Assert
            pagedRequest.RowsToSkip.Should().Be(expectedRowsToSkip);
        }
    }
}