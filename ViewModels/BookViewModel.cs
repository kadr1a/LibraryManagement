using LibraryManagement.Models;
using LibraryManagement.Data;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private LibraryContext _context;
        private ObservableCollection<Book> _books;
        private ObservableCollection<Author> _authors;
        private ObservableCollection<Genre> _genres;
        private string _searchText;
        private Author _selectedAuthor;
        private Genre _selectedGenre;

        public BookViewModel()
        {
            _context = new LibraryContext();
            _context.Database.EnsureCreated();
            
            LoadData();
        }

        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterBooks();
            }
        }

        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged();
                FilterBooks();
            }
        }

        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
                FilterBooks();
            }
        }

        public LibraryContext GetContext()
        {
            return _context;
        }

        private void LoadData()
        {
            _context.Authors.Load();
            _context.Genres.Load();
            _context.Books.Include(b => b.Author).Include(b => b.Genre).Load();

            Authors = new ObservableCollection<Author>(_context.Authors.Local);
            Genres = new ObservableCollection<Genre>(_context.Genres.Local);
            Books = new ObservableCollection<Book>(_context.Books.Local);
        }

        private void FilterBooks()
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                query = query.Where(b => b.Title.Contains(_searchText));
            }

            if (_selectedAuthor != null)
            {
                query = query.Where(b => b.AuthorId == _selectedAuthor.Id);
            }

            if (_selectedGenre != null)
            {
                query = query.Where(b => b.GenreId == _selectedGenre.Id);
            }

            Books = new ObservableCollection<Book>(query.ToList());
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            LoadData();
        }

        public void UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            _context.SaveChanges();
            LoadData();
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
            LoadData();
        }

        public void RefreshData()
        {
            LoadData();
        }
    }
}