using System.Windows;
using LibraryManagement.Models;
using LibraryManagement.Data;

namespace LibraryManagement.Views
{
    public partial class AddEditGenreWindow : Window
    {
        private LibraryContext _context;
        public Genre Genre { get; private set; }

        public AddEditGenreWindow(LibraryContext context, Genre genre = null)
        {
            InitializeComponent();
            _context = context;
            Genre = genre ?? new Genre();

            LoadGenreData();
        }

        private void LoadGenreData()
        {
            NameTextBox.Text = Genre.Name ?? string.Empty;
            DescriptionTextBox.Text = Genre.Description ?? string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название жанра");
                return;
            }

            Genre.Name = NameTextBox.Text;
            Genre.Description = DescriptionTextBox.Text;

            if (Genre.Id == 0)
                _context.Genres.Add(Genre);

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