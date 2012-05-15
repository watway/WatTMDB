using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatTmdb.V3;

namespace WatTmdb.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Tmdb api = new Tmdb("apikey", null);

            Console.Write("Enter Movie Title: ");
            var search = Console.ReadLine();
            if (string.IsNullOrEmpty(search))
                return;

            int page = 1;
            var result = api.SearchMovie(search, page);
            Console.WriteLine(string.Format("{0} matches found", result.total_results));
            while (result.results.Count > 0)
            {
                foreach (var movie in result.results)
                    Console.WriteLine(string.Format("{0} {1} {2}", movie.id, movie.title, movie.release_date));

                result = api.SearchMovie(search, ++page);
            }

            Console.WriteLine("Press any key...");
            Console.Read();
        }
    }
}
