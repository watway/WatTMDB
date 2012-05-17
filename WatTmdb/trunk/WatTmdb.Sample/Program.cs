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

            Console.WriteLine("Synchronous call...");
            {
                int page = 1;
                while (true)
                {
                    var result = api.SearchMovie(search, page);
                    if (result != null)
                    {
                        if (page == 1)
                            Console.WriteLine(string.Format("{0} matches found", result.total_results));

                        if (result.results.Count == 0)
                            break;

                        foreach (var movie in result.results)
                            Console.WriteLine(string.Format("{0} {1} {2}", movie.id, movie.title, movie.release_date));
                    }
                    else if (api.Error != null)
                        Console.WriteLine(string.Format("{0} {1}", api.Error.status_code, api.Error.status_message));
                }
            }

            Console.WriteLine("Asynchronous call...");
            {
                api.SearchMovie(search, 1, null, result =>
                    {
                        if (result.Data != null)
                        {
                            foreach (var movie in result.Data.results)
                                Console.WriteLine(string.Format("{0} {1} {2}", movie.id, movie.title, movie.release_date));
                        }
                        else
                            Console.WriteLine(string.Format("{0} {1}", result.Error.status_code, result.Error.status_message));
                    });
            }

            Console.WriteLine("Press any key...");
            Console.Read();
        }
    }
}
