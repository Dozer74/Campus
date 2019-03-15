using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Campus.Core;
using Campus.Core.Models;

namespace Campus.UI
{
    /// <summary>
    /// Логика взаимодействия для SelectStudentView.xaml
    /// </summary>
    public partial class SelectStudentView : Window
    {
        public int SelectedStudentId { get; set; }

        public SelectStudentView()
        {
            InitializeComponent();
        }

        private void SelectStudentView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var context = new DataContext();
            var students = context.Students.ToList();
            var collectionView = CollectionViewSource.GetDefaultView(students);
            collectionView.Filter = ListBox_Filter;

            listBox.ItemsSource = students;
        }

        private bool ListBox_Filter(object obj)
        {
            if (obj is Student student)
            {
                return student.FullName.ToLowerInvariant().Contains(tbFilter.Text.ToLowerInvariant());
            }

            return false;
        }

        private void TbFilter_TextChanged(Object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listBox.ItemsSource).Refresh();
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            SelectedStudentId = ((Student) listBox.SelectedItem).Id;
            DialogResult = true;
        }
    }
}