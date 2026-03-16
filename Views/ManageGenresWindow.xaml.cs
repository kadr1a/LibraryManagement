using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Views
{
    public partial class ManageGenresWindow : Window
    {
        private LibraryContext _context;

        public ManageGenresWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadGenres();
        }

        private void LoadGenres()
        {
            _context.Genres.Load();
            GenresDataGrid.ItemsSource = _context.Genres.Local.ToObservableCollection();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditGenreWindow(_context);
            if (dialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadGenres();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem is Genre selectedGenre)
            {
                var dialog = new AddEditGenreWindow(_context, selectedGenre);
                if (dialog.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadGenres();
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для редактирования");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem is Genre selectedGenre)
            {
                var result = MessageBox.Show($"Удалить жанр {selectedGenre.Name}?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _context.Genres.Remove(selectedGenre);
                    _context.SaveChanges();
                    LoadGenres();
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для удаления");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}