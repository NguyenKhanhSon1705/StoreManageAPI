namespace StoreManageAPI.ModelReturnData
{
    public class DataReturn
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; } = null;
    }
}
