using Microsoft.Extensions.Caching.Memory;

namespace MovieSearchAPI.Services
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private const string CacheKey = "SearchHistory";
        private const int MaxHistoryItems = 5;
        private readonly IMemoryCache _cache;

        public SearchHistoryService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddToSearchHistory(string query)
        {
            var searchHistory = _cache.GetOrCreate(CacheKey, entry => new List<string>());

            searchHistory.Insert(0, query);

            if (searchHistory.Count > MaxHistoryItems)
                searchHistory.RemoveAt(MaxHistoryItems);

            _cache.Set(CacheKey, searchHistory);
        }

        public IEnumerable<string> GetSearchHistory()
        {
            var searchHistory = _cache.Get<IEnumerable<string>>(CacheKey);
            return searchHistory ?? Enumerable.Empty<string>();
        }
    }
}
