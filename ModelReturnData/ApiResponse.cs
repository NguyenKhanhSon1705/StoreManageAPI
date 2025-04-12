namespace StoreManageAPI.ModelReturnData
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; } = false;
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
