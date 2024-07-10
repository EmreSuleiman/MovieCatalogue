using MovieCatalogue_Alpha.Context;
using System.Collections.Generic;

namespace MovieCatalogue_Alpha.Elements
{
    public class Director
    {
        public int DirectorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Director() { }
        public Director(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }
    }
}
