namespace MovieSearchAPI.Models
{
    public class OmdbSearchResult
    {
        public List<Movie> Search { get; set; }
        public int TotalResults { get; set; }
        public bool Response { get; set; }
    }
}
