namespace BuildingBlocks.Core
{
    public interface IFilterBase
    {
        int PageSize { get; set; }
        int PageIndex { get; set; }

        int GetSkip();
    }
}