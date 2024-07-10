using System.Collections.Generic;

namespace MovieCatalogue_Alpha.Elements
{
    public class Actor
    {
        public int ActorID { get; set; }
        public string ActorFirstName { get; set; }
        public string ActorLastName { get; set; }
        public List<MovieActor> MovieActors { get; set; }

        // Parameterless constructor for EF Core
        public Actor()
        {
            MovieActors = new List<MovieActor>();
        }

        public Actor(string actorfirstname, string actorlastname)
        {
            this.ActorFirstName = actorfirstname;
            this.ActorLastName = actorlastname;
            this.MovieActors = new List<MovieActor>();
        }
    }
}
