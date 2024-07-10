using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MovieCatalogue_Alpha.Context;
using MovieCatalogue_Alpha.Elements;
using Microsoft.EntityFrameworkCore;
using MovieCatalogue_Alpha.Context.MovieCatalogue_Alpha.Context;

namespace MovieCatalogue_Alpha
{
    public partial class Form1 : Form
    {
        string[] genres = { "Романтика", "Екшън", "Трилър", "Хорър" };
        string[] countries = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua & Deps", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Central African Rep", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Congo {Democratic Rep}", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland {Republic}", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea North", "Korea South", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar, {Burma}", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russian Federation", "Rwanda", "St Kitts & Nevis", "St Lucia", "Saint Vincent & the Grenadines", "Samoa", "San Marino", "Sao Tome & Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var context = new CatalogueContext())
            {
                var movies = context.Movie.Include(m => m.MovieActors).ThenInclude(ma => ma.Actor).Include(m => m.MovieDirector).ToList();
                foreach (var movie in movies)
                {
                    MovieSelectBox.Items.Add(movie);
                }
            }
        }

        private void LoadMovies()
        {
            using (var context = new CatalogueContext())
            {
                var movies = context.Movie.ToList();
                MovieSelectBox.Items.AddRange(movies.Select(m => m.Title).ToArray());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                string selectedItem = listBox.SelectedItem as string;
                MessageBox.Show($"Избран филм: {selectedItem}");
            }
        }

        private void MovieSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (MovieSelectBox.SelectedItem is Movie selectedMovie)
            {
                using (var context = new CatalogueContext())
                {
                    var movie = context.Movie
                        .Include(m => m.MovieActors)
                        .ThenInclude(ma => ma.Actor)
                        .Include(m => m.MovieDirector)
                        .FirstOrDefault(m => m.MovieID == selectedMovie.MovieID);

                    if (movie != null)
                    {
                        listBox1.Items.Add(movie.ToString());
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Actor> actors = new List<Actor>();
            Director director = null;

            string[] actorFullNames = ActorBox.Text.ToString().Split(',');
            string directorFullName = DirectorBox.Text.ToString();

            foreach (var actorFullName in actorFullNames)
            {
                string[] actorPartName = actorFullName.Trim().Split(' ');
                if (actorPartName.Length >= 2)
                {
                    string firstName = actorPartName[0];
                    string lastName = actorPartName[1];
                    actors.Add(new Actor(firstName, lastName));
                }
            }

            string[] directorPartName = directorFullName.Split(' ');
            if (directorPartName.Length >= 2)
            {
                string firstName = directorPartName[0];
                string lastName = directorPartName[1];
                director = new Director(firstName, lastName);
            }

            using (var context = new CatalogueContext())
            {
                var movie = new Movie(TitleBox.Text, actors, director, CountryBox.SelectedItem.ToString(), GenreBox.SelectedItem.ToString());
                context.Movie.Add(movie);
                context.SaveChanges();
            }

            DisplayMovies();
        }
        private void DisplayMovies()
        {
            listBox1.Items.Clear();
            using (var context = new CatalogueContext())
            {
                var movies = context.Movie.Include(m => m.MovieActors).Include(m => m.MovieDirector).ToList();
                foreach (var movie in movies)
                {
                    listBox1.Items.Add(movie.ToString());
                }
            }
        }


        private void Remove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var selectedMovieString = listBox1.SelectedItem.ToString();
                using (var context = new CatalogueContext())
                {
                    var movie = context.Movie
                        .Include(m => m.MovieActors)
                        .ThenInclude(ma => ma.Actor)
                        .Include(m => m.MovieDirector)
                        .AsEnumerable()
                        .FirstOrDefault(m => m.ToString() == selectedMovieString);

                    if (movie != null)
                    {
                        context.Movie.Remove(movie);
                        context.SaveChanges();

                        var itemToRemove = MovieSelectBox.Items.Cast<Movie>().FirstOrDefault(m => m.MovieID == movie.MovieID);
                        if (itemToRemove != null)
                        {
                            MovieSelectBox.Items.Remove(itemToRemove);
                        }
                        listBox1.Items.Clear();
                    }
                }
            }
        }
    }
}