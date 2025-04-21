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

            foreach (var book in searched)
            {
                Books.Add(book);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
