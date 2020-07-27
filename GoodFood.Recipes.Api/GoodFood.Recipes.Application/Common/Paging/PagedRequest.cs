using FluentValidation;

namespace GoodFood.Recipes.Application.Common.Paging
{
    public class PagedRequest
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int RowsToSkip => this.PageSize * (this.CurrentPage - 1);

        internal static PagedRequest Default { get; } = 
            new PagedRequest 
            {
                CurrentPage = 1,
                PageSize = 5
            };
    }

    public class PagedRequestValidator : AbstractValidator<PagedRequest>
    {
        public PagedRequestValidator()
        {
            RuleFor(x => x.CurrentPage).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}