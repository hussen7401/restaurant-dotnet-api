namespace core.DTOs
{
    public class ResponseDto<T>
    {
        public required string Status { get; set; }
        public required int Code { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }
}
