using System.Windows;
using LibraryManagement.ViewModels;
using LibraryManagement.Views;
using LibraryManagement.Models;

namespace LibraryManagement
{
    public partial class MainWindow : Window
    {
        private BookViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new BookViewModel();
            DataContext = _viewModel;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            StatusTextBlock.Text = $"Всего книг: {_viewModel.Books.Count}";
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditBookWindow(_viewModel.GetContext());
            if (window.ShowDialog() == true)
            {
                _viewModel.AddBook(window.Book);
                UpdateStatus();
            }
        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var window = new AddEditBookWindow(_viewModel.GetContext(), selectedBook);
                if (window.ShowDialog() == true)
                {
                    _viewModel.UpdateBook(window.Book);
                    UpdateStatus();
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования");
            }
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var result = MessageBox.Show("Удалить выбранную книгу?", "Подтверждение", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.DeleteBook(selectedBook);
                    UpdateStatus();
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления");
            }
        }

        private void ManageAuthorsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ManageAuthorsWindow(_viewModel.GetContext());
            window.Owner = this;
            window.ShowDialog();
            _viewModel.RefreshData();
        }

        private void ManageGenresButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ManageGenresWindow(_viewModel.GetContext());
            window.Owner = this;
            window.ShowDialog();
            _viewModel.RefreshData();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshData();
            UpdateStatus();
        }
    }
}