using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using System.Collections.ObjectModel;

namespace LibraryManagement.Views
{
    public partial class ManageAuthorsWindow : Window
    {
        private LibraryContext _context;

        public ManageAuthorsWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            _context.Authors.Load();
            AuthorsDataGrid.ItemsSource = _context.Authors.Local.ToObservableCollection();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditAuthorWindow(_context);
            if (dialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadAuthors();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
            {
                var dialog = new AddEditAuthorWindow(_context, selectedAuthor);
                if (dialog.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadAuthors();
                }
            }
            else
            {
                MessageBox.Show("Выберите автора для редактирования");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
            {
                var result = MessageBox.Show($"Удалить автора {selectedAuthor.FullName}?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _context.Authors.Remove(selectedAuthor);
                    _context.SaveChanges();
                    LoadAuthors();
                }
            }
            else
            {
                MessageBox.Show("Выберите автора для удаления");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}