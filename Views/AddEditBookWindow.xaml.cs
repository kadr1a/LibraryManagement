using System.Windows;
using System.Linq;
using LibraryManagement.Models;
using LibraryManagement.Data;

namespace LibraryManagement.Views
{
    public partial class AddEditBookWindow : Window
    {
        private LibraryContext _context;
        public Book Book { get; private set; }

        public AddEditBookWindow(LibraryContext context, Book book = null)
        {
            InitializeComponent();
            _context = context;
            Book = book ?? new Book();

            LoadComboBoxes();
            LoadBookData();
        }

        private void LoadComboBoxes()
        {
            AuthorComboBox.ItemsSource = _context.Authors.ToList();
            GenreComboBox.ItemsSource = _context.Genres.ToList();
        }

        private void LoadBookData()
        {
            TitleTextBox.Text = Book.Title ?? string.Empty;
            
            if (Book.Author != null)
                AuthorComboBox.SelectedItem = Book.Author;
            
            if (Book.Genre != null)
                GenreComboBox.SelectedItem = Book.Genre;
            
            YearTextBox.Text = Book.PublishYear.ToString();
            ISBNTextBox.Text = Book.ISBN ?? string.Empty;
            QuantityTextBox.Text = Book.QuantityInStock.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название книги");
                return;
            }

            if (AuthorComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите автора");
                return;
            }

            if (GenreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр");
                return;
            }

            Book.Title = TitleTextBox.Text;
            Book.Author = (Author)AuthorComboBox.SelectedItem;
            Book.AuthorId = Book.Author.Id;
            Book.Genre = (Genre)GenreComboBox.SelectedItem;
            Book.GenreId = Book.Genre.Id;
            
            if (int.TryParse(YearTextBox.Text, out int year))
                Book.PublishYear = year;
            
            Book.ISBN = ISBNTextBox.Text;
            
            if (int.TryParse(QuantityTextBox.Text, out int quantity))
                Book.QuantityInStock = quantity;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}