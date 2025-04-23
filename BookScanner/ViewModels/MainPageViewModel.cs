using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookScanner.Models;
using BookScanner.Services;

namespace BookScanner.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public readonly DatabaseService _database;

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Book> _allBooks = new();
        public ObservableCollection<Book> Books { get; } = new();

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    SearchBooks();
                }
            }
        }

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    OnPropertyChanged();
                    SearchBooks(); // Apply search AND sort when changed
                    //_ = RefreshBooksAsync(); // fire-and-forget
                    //_ = LoadBooksAsync();
                }
            }
        }

        public List<string> SortOptions { get; } = new()
        {
            "Title (A-Z)",
            "Title (Z-A)",
            "Author (A-Z)",
            "Author (Z-A)",
            "Genre (A-Z)",
            "Genre (Z-A)",
            "Rating (High to Low)",
            "Rating (Low to High)"
        };

        public MainPageViewModel(DatabaseService database)
        {
            _database = database;
        }

        public async Task LoadBooksAsync()
        {
            _allBooks = await _database.GetBooksAsync();
            SearchBooks();
        }

        private void SearchBooks()
        {
            Books.Clear();

            var searched = string.IsNullOrWhiteSpace(_searchQuery)
                ? _allBooks
                : _allBooks.Where(b => 
                !string.IsNullOrWhiteSpace(b.Title) && b.Title.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrWhiteSpace(b.Author) && b.Author.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrWhiteSpace(b.ISBN) && b.ISBN.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)
                );

            IEnumerable<Book> sorted = _selectedSortOption switch
            {
                "Title (A-Z)" => searched.OrderBy(b => b.Title),
                "Title (Z-A)" => searched.OrderByDescending(b => b.Title),
                "Author (A-Z)" => searched.OrderBy(b => b.Author),
                "Author (Z-A)" => searched.OrderByDescending(b => b.Author),
                "Genre (A-Z)" => searched.OrderBy(b => b.Genre),
                "Genre (Z-A)" => searched.OrderByDescending(b => b.Genre),
                "Rating (High to Low)" => searched.OrderByDescending(b => int.TryParse(b.Rating, out var r) ? r : 0),
                "Rating (Low to High)" => searched.OrderBy(b => int.TryParse(b.Rating, out var r) ? r : int.MaxValue),
                _ => searched
            };

            foreach (var book in sorted)
            {
                Books.Add(book);
            }

            OnPropertyChanged(nameof(Books)); // Force UI to refresh if needed
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
