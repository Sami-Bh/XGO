namespace XGO.ApiGateway.Models
{
    /// <summary>
    /// Request model for retrieving expiring items from storage.
    /// Maps HTTP query parameters to strongly-typed properties.
    /// </summary>
    public class GetExpiringItemsRequest
    {
        /// <summary>
        /// Number of days until expiration to filter by.
        /// If null, returns all items with expiration dates.
        /// Example: 7 returns items expiring within the next week.
        /// </summary>
        public int? ExpiresInDays { get; set; }

        /// <summary>
        /// Whether to include items that have been acknowledged as expired.
        /// Default is false (excludes acknowledged items).
        /// </summary>
        public bool IncludeAcknowledgedExpiredItems { get; set; }

        /// <summary>
        /// Number of items to return per page for pagination.
        /// Minimum value is 5. Default is 5.
        /// </summary>
        public int PageSize { get; set; } = 5;

        /// <summary>
        /// Page number to retrieve (1-based index).
        /// Default is 1 (first page).
        /// </summary>
        public int PageIndex { get; set; } = 1;
    }
}
