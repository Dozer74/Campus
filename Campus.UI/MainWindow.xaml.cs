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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Campus.Core;
using Campus.Core.Models;
using Campus.Core.Scripts;

namespace Campus.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataContext _context = new DataContext();


        public MainWindow()
        {
            InitializeComponent();
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            cbDorms.ItemsSource = _context.Dorms.ToList();
        }

        private void CbDorms_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            tbAllowedFaculties.Text = "";
            listView.ItemsSource = null;

            var dorm = cbDorms.SelectedItem as Dorm;
            if (dorm == null)
            {
                return;
            }

            var fNames = dorm.AllowedFaculties.Select(f => f.Title);
            tbAllowedFaculties.Text = string.Join(", ", fNames);

            listView.ItemsSource = dorm.Rooms
                .OrderBy(r => r.Number)
                .ToList();
        }

        private void ListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var room = ((FrameworkElement) e.OriginalSource).DataContext as Room;
            if (room == null)
            {
                return;
            }

            var roomView = new RoomView(_context, room.Id);
            roomView.ShowDialog();
        }
    }
}