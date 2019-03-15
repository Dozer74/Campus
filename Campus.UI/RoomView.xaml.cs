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
    /// Логика взаимодействия для RoomView.xaml
    /// </summary>
    public partial class RoomView : Window
    {
        private readonly DataContext _context;
        private readonly Room _room;

        public RoomView(DataContext context, int roomId)
        {
            _context = context;
            _room = context.Rooms.Find(roomId);
            DataContext = _room;
            InitializeComponent();
        }

        private void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(
                message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            if (_room.Capacity == _room.Tenants.Count)
            {
                ShowErrorMessageBox(
                    "В эту комнату невозможно заселить ещё одного студента: " +
                    "в ней не осталось свободных мест."
                );
                return;
            }

            var selectForm = new SelectStudentView();
            if (selectForm.ShowDialog() != true)
            {
                return;
            }

            var student = _context.Students.Find(selectForm.SelectedStudentId);
            if (student == null)
            {
                return;
            }

            if (student.Room != null)
            {
                ShowErrorMessageBox(
                    $"Этому студенту уже назначена комната: Общежитие № {student.Room.Dorm.Number}, " +
                    $"комната № {student.Room.Number}\n" +
                    "Студент не может числиться в двух и более комнатах одновременно");
                return;
            }

            if (!_room.Dorm.AllowedFaculties.Select(f => f.Id).Contains(student.Faculty.Id))
            {
                var message =
                    "В этом общежитии могут проживать только студенты, обучающиеся на следующих факультетах:\n\n";
                foreach (var faculty in _room.Dorm.AllowedFaculties)
                {
                    message += faculty.Title + "\n";
                }

                message += $"\nВыбранный студент обучается на на факультете {student.Faculty.Title}." +
                           $"\nСтудент не может проживать в этои общежитии";

                ShowErrorMessageBox(message);
                return;
            }

            if (_room.Tenants.Count > 0)
            {
                var gender = _room.Tenants.First().Gender;
                if (student.Gender != gender)
                {
                    ShowErrorMessageBox("Проживание студентов разного пола в одной комнате запрещено");
                    return;
                }
            }

            _room.Tenants.Add(student);
            _context.SaveChanges();

            DataContext = null;
            DataContext = _room;
        }

        private void Button_Click_1(Object sender, RoutedEventArgs e)
        {
            var student = listBox.SelectedItem as Student;
            if (student == null)
            {
                return;
            }

            if (MessageBox.Show(
                    $"Вы уверены, что хотите выселить студента {student.FullName} " +
                    $"из комнаты № {student.Room.Number} " +
                    $"(Общежитие № {student.Room.Dorm.Number})",
                    "Подтвердите действие",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes
            )
            {
                _room.Tenants.Remove(student);
                _context.SaveChanges();

                DataContext = null;
                DataContext = _room;
            }
        }
    }
}