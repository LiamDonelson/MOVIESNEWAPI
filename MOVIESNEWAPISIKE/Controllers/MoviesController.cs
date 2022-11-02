using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOVIESNEWAPISIKE.Models;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }
    //Write a API Endpoint that takes a string for title, and returns all movies with a similar title

    //By using the {} I've set this action up to expect a title as a parameter 
    //Linq is your friend with API calls 
    [HttpGet("SearchByTitle/{title}")]
    public async Task<ActionResult<IEnumerable<MovieInventory>>> SearchByTitle(string title)
    {
        return await _context.MovieInventories.Where(m => m.MovieName.Contains(title)).ToListAsync();
    }

    [HttpGet("SearchByGenre/{genre}")]
    public async Task<ActionResult<IEnumerable<MovieInventory>>> SearchByMovie(string genre)
    {
        return await _context.MovieInventories.Where(m => m.Genere.Contains(genre)).ToListAsync();
    }


    [HttpGet("Random")]
    public MovieInventory RandomMovies()
    {
        List<MovieInventory> movies = _context.MovieInventories.ToList();
        var rnd = new Random();
        int index = rnd.Next(movies.Count);

        return movies[index];
    }




    [HttpGet("RandomGenre")]
    public async Task<ActionResult<IEnumerable<MovieInventory>>> RandomGenre() 
    {

        // adds new movie list with all the movies 
        List<MovieInventory>Movie = _context.MovieInventories.ToList();

        // creates a new list that will take in all the genres
        List<string> Genres = new List<string>();

        // goes through each movie, and adds their gerene on a list if it is not already on that list
        foreach(var movie in _context.MovieInventories)
        {
            if (Genres.Contains(movie.Genere))
            {

            }
            else
            {
                Genres.Add(movie.Genere);
            }
        }

        // gets a new random variable 
        var rnd = new Random();
        // chooses a random number based on the number of generes 
        int index = rnd.Next(Genres.Count);

        // Gets a random genere in the index of the list
        string pickedgenre = Genres[index];

        return await _context.MovieInventories.Where(m => m.Genere.Contains(pickedgenre)).ToListAsync();
    }

    [HttpGet("RandomGenrefromuserpicked")]

    public async Task<ActionResult<IEnumerable<MovieInventory>>> RandomGenreUserPicked(string genre)
    {

        // adds new movie list with all the movies 
        List<MovieInventory> Movie = _context.MovieInventories.ToList();

        List<string> SelectedGenre = new List<string>();

        foreach(MovieInventory movie in _context.MovieInventories)
        {
            if (movie.Genere == genre)
            {
                SelectedGenre.Add(movie.MovieName);
            }

        }

        // gets a new random variable 
        var rnd = new Random();
        // chooses a random number based on the number of generes 
        int index = rnd.Next(SelectedGenre.Count);

        string pickedmovie = SelectedGenre[index];

        return await _context.MovieInventories.Where(m => m.MovieName.Contains(pickedmovie)).ToListAsync();

    }



    // GET: api/Movies/5
    [HttpGet("{Id}")]
    public async Task<ActionResult<MovieInventory>> GetMovie(int Id)
    {
        var movie = await _context.MovieInventories.FindAsync(Id);

        if (movie == null)
        {
            return NotFound();
        }

        return movie;
    }

    // PUT: api/Movies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{Id}")]
    public async Task<IActionResult> PutMovie(int Id, MovieInventory movie)
    {
        if (Id != movie.Id)
        {
            return BadRequest();
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Movies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieInventory>> PostMovie(MovieInventory movie)
    {
        _context.MovieInventories.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
    }

    // DELETE: api/Movies/5
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteMovie(int Id)
    {
        var movie = await _context.MovieInventories.FindAsync(Id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.MovieInventories.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    //In API controller you may write non-endpoint helper methods 
    //Examples of helper methods: 
    //Validation - for rare cases SQL and C# can't cover something 
    //Sorting or format output - say you only want the movie titles in alphabetical order 
    //These helpers exist to be called by endpoints, but can't be called directly by HTTP 
    //Helpers need to be private, as any public method in the API controller is treated as an endpoint
    private bool MovieExists(int id)
    {
        return _context.MovieInventories.Any(e => e.Id == id);
    }

    //This method handles creating the endpoint 
    [HttpGet("GetTitles")]
    public async Task<ActionResult<IEnumerable<String>>> GetTitlesAlphabetical()
    {
        return GetTitles();
    }

    //this method handles the functionality 
    private List<string> GetTitles()
    {
        List<string> titles = new List<string>();
        List<MovieInventory> movies = _context.MovieInventories.ToList();

        foreach (MovieInventory m in movies)
        {
            titles.Add(m.MovieName);
        }
        titles.Sort();
        return titles;
    }
}