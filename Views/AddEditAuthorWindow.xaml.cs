using System;
using System.Linq;
using System.Windows;
using LibraryManagement.Models;
using LibraryManagement.Data;

namespace LibraryManagement.Views
{
    public partial class AddEditAuthorWindow : Window
    {
        private LibraryContext _context;
        public Author Author { get; private set; }

        public AddEditAuthorWindow(LibraryContext context, Author author = null)
        {
            InitializeComponent();
            _context = context;
            Author = author ?? new Author();

            LoadAuthorData();
        }

        private void LoadAuthorData()
        {
            FirstNameTextBox.Text = Author.FirstName ?? string.Empty;
            LastNameTextBox.Text = Author.LastName ?? string.Empty;
            
            if (Author.BirthDate != DateTime.MinValue)
                BirthDatePicker.SelectedDate = Author.BirthDate;
            
            CountryTextBox.Text = Author.Country ?? string.Empty;
        }

        private bool IsAuthorDuplicate(string firstName, string lastName, int currentAuthorId)
        {
            return _context.Authors.Any(a => 
                a.FirstName == firstName && 
                a.LastName == lastName && 
                a.Id != currentAuthorId);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Введите имя автора");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Введите фамилию автора");
                return;
            }

            if (IsAuthorDuplicate(FirstNameTextBox.Text, LastNameTextBox.Text, Author.Id))
            {
                MessageBox.Show("Автор с таким именем и фамилией уже существует");
                return;
            }

            Author.FirstName = FirstNameTextBox.Text;
            Author.LastName = LastNameTextBox.Text;
            
            if (BirthDatePicker.SelectedDate.HasValue)
                Author.BirthDate = BirthDatePicker.SelectedDate.Value;
            
            Author.Country = CountryTextBox.Text;

            if (Author.Id == 0)
                _context.Authors.Add(Author);

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