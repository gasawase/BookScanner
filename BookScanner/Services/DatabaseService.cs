using BookScanner.Models;
using SQLite;

namespace BookScanner.Services
{
    public class DatabaseService
    {
        private const string DB_NAME = "books.db3";
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
            _connection = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _connection.CreateTableAsync<Book>();
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _connection.Table<Book>().ToListAsync();
        }

        public async Task<Book> GetBookAsync(int id)
        {
            return await _connection.Table<Book>().Where(book => book.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveBookAsync(Book book)
        {
            if (book.Id != 0)
            {
                await _connection.UpdateAsync(book);
            }
            else
            {
                await _connection.InsertAsync(book);
            }
        }

        public async Task DeleteBookAsync(Book book)
        {
            await _connection.DeleteAsync(book);
        }
    }
}
