namespace Domain.DTOs
{
    public class Paginacao<T>
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public required ICollection<T> Dados { get; set; }
    }
}
