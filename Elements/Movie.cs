

using System.Collections.Generic;
using System.Linq;
using MovieCatalogue_Alpha.Elements;

namespace MovieCatalogue_Alpha.Elements
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public List<MovieActor> MovieActors { get; set; }
        public Director MovieDirector { get; set; }
        public int DirectorID { get; set; }
        public string MovieCountry { get; set; }
        public string Genre { get; set; }

        public Movie()
        {
            MovieActors = new List<MovieActor>();
        }

        public Movie(string title, List<Actor> actors, Director director, string country, string genre)
        {
            Title = title;
            MovieActors = actors?.Select(a => new MovieActor { Movie = this, Actor = a }).ToList() ?? new List<MovieActor>();
            MovieDirector = director;
            DirectorID = director?.DirectorID ?? 0;
            MovieCountry = country;
            Genre = genre;
        }

        public override string ToString()
        {
            var actors = MovieActors != null ? string.Join(", ", MovieActors.Select(ma => $"{ma.Actor?.ActorFirstName} {ma.Actor?.ActorLastName}")) : "N/A";
            var director = MovieDirector != null ? $"{MovieDirector.FirstName} {MovieDirector.LastName}" : "N/A";
            return $"Заглавие: {Title}, Актьори: {actors}, Режисьор: {director}, Държава: {MovieCountry}, Жанр: {Genre}";
        }
    }
    public class MovieActor
    {
        public int MovieID { get; set; }
        public Movie Movie { get; set; }

        public int ActorID { get; set; }
        public Actor Actor { get; set; }
    }
}
