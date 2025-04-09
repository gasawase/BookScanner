using BookScanner.Models;
using System.Text.Json;

namespace BookScanner.Services
{
    public static class BookApi
    {
        /// <summary>
        /// Retrieves book information from Google Books API using the provided ISBN.
        /// </summary>
        /// <param name="isbn">The ISBN to look up</param>
        /// <returns>A Book object containing the book information, or null if not found/error occurs</returns>
        public static async Task<Book?> LookupISBN(string isbn)
        {
            try
            {
                var httpClient = new HttpClient();
                var jsonResponse = await httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}");

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var jsonString = await jsonResponse.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var googleBooksApiResponse = JsonSerializer.Deserialize<GoogleBooksApiResponse>(jsonString, options);

                    if (googleBooksApiResponse != null && googleBooksApiResponse.Items != null && googleBooksApiResponse.Items.Length > 0)
                    {
                        var item = googleBooksApiResponse.Items[0].VolumeInfo;

                        return new Book
                        {
                            Title = item.Title,
                            Author = string.Join(", ", item.Authors ?? ["Unknown Author"]),
                            Genre = item.Categories.FirstOrDefault() ?? "Unknown Genre",
                            Description = item.Description,
                            // Prefer ISBN13, then ISBN10, then fallback to the provided ISBN used in the search
                            ISBN = item.IndustryIdentifiers.FirstOrDefault(id => id.Type == "ISBN_13")?.Identifier ?? item.IndustryIdentifiers.FirstOrDefault(id => id.Type == "ISBN_10")?.Identifier ?? isbn,
                        };
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Classes to deserialize Google Books API response into
        private class GoogleBooksApiResponse
        {
            public int TotalItems { get; set; }
            public GoogleBooksApiItem[] Items { get; set; }

        }

        private class GoogleBooksApiItem
        {
            public GoogleBooksApiVolumeInfo VolumeInfo { get; set; }
        }

        private class GoogleBooksApiVolumeInfo
        {
            public string Title { get; set; }
            public string[] Authors { get; set; }
            public string Description { get; set; }
            public GoogleBooksApiIndustryIdentifier[] IndustryIdentifiers { get; set; }
            public string[] Categories { get; set; }

        }

        private class GoogleBooksApiIndustryIdentifier
        {
            public string Type { get; set; }
            public string Identifier { get; set; }
        }
    }
}
