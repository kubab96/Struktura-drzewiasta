namespace TreeStructure.Models
{
    public class NodeQuery
    {
        public SortDirection SortDirection { get; set; }
    }

    public enum SortDirection
    {
        ASCENDING,
        DESCENDING
    }
}
