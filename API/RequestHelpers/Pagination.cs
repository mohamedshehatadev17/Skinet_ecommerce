namespace API.RequestHelpers
{
    public class Pagination<T>(IReadOnlyList<T> data, int count, int pageIndex, int pageSize)
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}
