using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // Установите Newtonsoft.Json через NuGet

namespace m9_1
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public override string ToString() => $"{FirstName} {LastName}, Возраст: {Age}";
    }

    class Program
    {
        static List<Student> students = new List<Student>();
        static string filePath = "students.json";

        static void Main()
        {
            LoadStudents();
            while (true)
            {
                Console.WriteLine("1. Добавить студента\n2. Удалить студента\n3. Редактировать студента\n4. Показать всех студентов\n5. Поиск студента\n6. Сортировка студентов\n7. Выход");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": RemoveStudent(); break;
                    case "3": EditStudent(); break;
                    case "4": ShowStudents(); break;
                    case "5": SearchStudent(); break;
                    case "6": SortStudents(); break;
                    case "7": SaveStudents(); return;
                }
            }
        }

        static void AddStudent()
        {
            var student = new Student
            {
                FirstName = Prompt("Имя: "),
                LastName = Prompt("Фамилия: "),
                Age = int.Parse(Prompt("Возраст: "))
            };
            students.Add(student);
        }

        static void RemoveStudent()
        {
            ShowStudents();
            int index = int.Parse(Prompt("Введите индекс студента для удаления: "));
            if (index >= 0 && index < students.Count)
            {
                students.RemoveAt(index);
                Console.WriteLine("Студент удален.");
            }
        }

        static void EditStudent()
        {
            ShowStudents();
            int index = int.Parse(Prompt("Введите индекс студента для редактирования: "));
            if (index >= 0 && index < students.Count)
            {
                var student = students[index];
                student.FirstName = Prompt("Новое имя (оставьте пустым для пропуска): ", student.FirstName);
                student.LastName = Prompt("Новая фамилия (оставьте пустым для пропуска): ", student.LastName);
                string ageInput = Prompt("Новый возраст (оставьте пустым для пропуска): ");
                if (int.TryParse(ageInput, out int newAge)) student.Age = newAge;
            }
        }

        static void ShowStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Список студентов пуст.");
                return;
            }

            Console.WriteLine("Список студентов:");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i}: {students[i]}");
            }
        }

        static void SearchStudent()
        {
            string searchQuery = Prompt("Введите имя или фамилию для поиска: ");
            var foundStudents = students.FindAll(s => s.FirstName.Contains(searchQuery) || s.LastName.Contains(searchQuery));

            if (foundStudents.Count > 0)
            {
                Console.WriteLine("Найденные студенты:");
                foundStudents.ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine("Студенты не найдены.");
            }
        }

        static void SortStudents() => students.Sort((s1, s2) => s1.LastName.CompareTo(s2.LastName));

        static void LoadStudents()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            }
        }

        static void SaveStudents()
        {
            var json = JsonConvert.SerializeObject(students, Newtonsoft.Json.Formatting.Indented); // Уточнено пространство имен
            File.WriteAllText(filePath, json);
            Console.WriteLine("Список студентов сохранен.");
        }

        static string Prompt(string message, string defaultValue = "")
        {
            Console.Write(message);
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }
    }
}
