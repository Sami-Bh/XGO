using BuildingBlocks.Core;

namespace XGO.Storage.Api.Storage.Application.Models
{
    /// <summary>
    /// Provides filtering criteria for retrieving products based on their expiration status.
    /// </summary>
    /// <remarks>Use this filter to specify the number of days until expiration and to control whether
    /// acknowledged expired items are included in the results. This enables effective management and querying of
    /// product life cycles, supporting scenarios such as inventory monitoring, reporting, and auditing.</remarks>
    public class ExpiringProductsFilter:FilterBase
    {
       /// <summary>
       /// Gets or sets the number of days until the item expires. A value of <see langword="null"/> indicates that the
       /// value is optional.
       /// </summary>
       /// <remarks>This property is optional. If set, the value must be a non-negative integer
       /// representing the number of days until expiration. If <see langword="null"/>, the item will not
       /// expire.</remarks>
        public int? ExpiresInDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether acknowledged expired items are included in the results.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, the results will include items that have been
        /// acknowledged as expired. This can be useful for reporting or auditing purposes.</remarks>
        public bool IncludeAcknowledgedExpiredItems { get; set; }
    }
}
