namespace PortalApi.Common
{
    public class ApiResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// Any identity field returned 
        /// </summary>
        public string ResponseId { get; set; }
        public bool IsCached { get; set; }
    }

    public enum ResponseStatus
    {
        Success,
        Warning,
        NotFound,
        Unknown,
        Outage,
        Failed
    }
}
