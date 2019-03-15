using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Campus.Core.Models;

namespace Campus.Core.Scripts
{
    static class RandomExtensions
    {
        public static T Choose<T>(this Random random, IList<T> list)
        {
            return Choose(random, list, 1)[0];
        }

        public static List<T> Choose<T>(this Random random, IList<T> list, int count)
        {
            return list
                .OrderBy(i => Guid.NewGuid())
                .Take(count)
                .ToList();
        }
    }

    public class DbInitializer
    {
        public static void GenerateInitData(DataContext context, int dormsCount = 7, int roomsCount = 140, int studentsCount = 500)
        {
            // Loads files
            var facultiesTitles = File.ReadAllLines(@"Scripts\Faculties.txt");
            var maleNames = File.ReadAllLines(@"Scripts\MaleNames.txt");
            var maleSurnames = File.ReadAllLines(@"Scripts\MaleSurnames.txt");
            var femaleNames = File.ReadAllLines(@"Scripts\FemaleNames.txt");
            var femaleSurnames = File.ReadAllLines(@"Scripts\FemaleSurnames.txt");
           
            var random = new Random();


            // Generate new entities
            var faculties = facultiesTitles
                .Select(f => new Faculty
                {
                    Title = f
                }).ToList();
            context.Faculties.AddRange(faculties);
            context.SaveChanges();

            var students = Enumerable.Repeat(0, studentsCount)
                .Select(i =>
                {
                    var gender = random.Next(2) == 0 ? Gender.Male : Gender.Female;
                    var names = gender == Gender.Male ? maleNames : femaleNames;
                    var surnames = gender == Gender.Male ? maleSurnames : femaleSurnames;

                    return new Student
                    {
                        Faculty = random.Choose(faculties),
                        Gender = gender,
                        FirstName = random.Choose(names),
                        LastName = random.Choose(surnames)
                    };
                }).ToList();
            context.Students.AddRange(students);
            context.SaveChanges();

            var dorms = new List<Dorm>();
            for (int i = 0; i < dormsCount; i++)
            {
                dorms.Add(new Dorm
                {
                    AllowedFaculties = random.Choose(faculties, 4),
                    Number = i + 1
                });
            }
            context.Dorms.AddRange(dorms);
            context.SaveChanges();

            var rooms = Enumerable.Repeat(0, roomsCount)
                .Select(i => new Room
                {
                    Number = random.Next(1, 1001),
                    Capacity = random.Next(1, 6),
                    Dorm = random.Choose(dorms)
                }).ToList();
            context.Rooms.AddRange(rooms);
            context.SaveChanges();

        }
    }
}