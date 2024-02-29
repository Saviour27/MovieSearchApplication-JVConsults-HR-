namespace MovieSearchAPI.Services
{
    public interface ISearchHistoryService
    {
        void AddToSearchHistory(string query);
        IEnumerable<string> GetSearchHistory();
    }
}
