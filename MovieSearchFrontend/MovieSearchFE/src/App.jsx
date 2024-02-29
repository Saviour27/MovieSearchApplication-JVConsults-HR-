import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [history, setHistory] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState(null);

  useEffect(() => {
    fetchSearchHistory();
  }, []);

  const fetchSearchHistory = async () => {
    try {
      const response = await fetch('https://localhost:7279/movie/history');
      const data = await response.json();
      setHistory(data);
    } catch (error) {
      console.error('Error fetching search history:', error);
    }
  };

  const handleSearch = async () => {
    try {
      const response = await fetch(`https://localhost:7279/movie/search?title=${searchQuery}`);
      const data = await response.json();
      setSearchResults(data);
      addToHistory(searchQuery);
    } catch (error) {
      console.error('Error searching for movies:', error);
    }
  };

  const addToHistory = (query) => {
    try {
      fetch('https://localhost:7279/movie/search', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ title: query }),
      });
    } catch (error) {
      console.error('Error adding to search history:', error);
    }
  };

  const handleHistoryClick = async (query) => {
    setSearchQuery(query);
    await handleSearch();
  };

  const handleMovieClick = async (imdbID) => {
    try {
      const response = await fetch(`https://localhost:7279/movie/details/${imdbID}`); // Modify the URL here
      const data = await response.json();
      setSelectedMovie(data);
    } catch (error) {
      console.error('Error fetching movie details:', error);
    }
  };

  return (
    <React.Fragment>
      <div className="App">
        <h1>Movie Search App</h1>
        <div>
          <input type="text" value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} />
          <button onClick={handleSearch}>Search</button>
        </div>
        <div>
          {searchResults.map(movie => (
            <div key={movie.imdbID} onClick={() => handleMovieClick(movie.imdbID)}>
              <img src={movie.Poster} alt={movie.Title} />
              <p>{movie.Title} ({movie.Year})</p>
            </div>
          ))}
        </div>
        <div>
          <h2>Search History</h2>
          <ul>
            {history.map((query, index) => (
              <li key={index} onClick={() => handleHistoryClick(query)}>{query}</li>
            ))}
          </ul>
        </div>
      </div>
      {selectedMovie && (
        <div className="MovieDetails">
          <h2>Movie Details</h2>
          <p>Title: {selectedMovie.Title}</p>
          <p>Year: {selectedMovie.Year}</p>
          <p>IMDB ID: {selectedMovie.imdbID}</p>
        </div>
      )}
    </React.Fragment>
  );
}

export default App;
