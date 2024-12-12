namespace Tournament.Core.Dto
{
    public class PaginationMetadata
    {
        public int TotalPages { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }
        public int TotalItems { get; }

        public PaginationMetadata(int totalPages, int pageSize, int currentPage, int totalItems)
        {
            TotalPages = totalPages;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalItems = totalItems;
        }
    }
}
