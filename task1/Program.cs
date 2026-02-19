using System;
using System.Linq;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase
{
    public class Startup
    {
        private static int? _currentDoctorId = null;

        static void Main(string[] args)
        {
            using var ctx = new HospitalContext();

            Console.WriteLine("Система 'Лікарня' з Авторизацією ");

            while (_currentDoctorId == null)
            {
                Console.WriteLine("\nБудь ласка, увійдіть або зареєструйтесь:");
                Console.WriteLine("1. Увійти (Login)");
                Console.WriteLine("2. Зареєструвати нового лікаря");
                Console.WriteLine("3. Вихід");
                Console.Write("Ваш вибір: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": Login(ctx); break;
                        case "2": RegisterDoctor(ctx); break;
                        case "3": return;
                        default: Console.WriteLine("Невірний вибір."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }

            while (true)
            {
                Console.WriteLine($"\n--- ГОЛОВНЕ МЕНЮ (Ви увійшли як ID: {_currentDoctorId}) ");
                Console.WriteLine("1. Переглянути список пацієнтів");
                Console.WriteLine("2. Додати нового пацієнта");
                Console.WriteLine("3. Додати візит (для поточного лікаря)");
                Console.WriteLine("4. Вихід");
                Console.Write("Ваш вибір: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": ListPatients(ctx); break;
                        case "2": AddPatient(ctx); break;
                        case "3": AddVisitation(ctx); break;
                        case "4": return;
                        default: Console.WriteLine("Невірний вибір."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }

        private static void Login(HospitalContext ctx)
        {
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Пароль: ");
            var password = Console.ReadLine();

            var doctor = ctx.Doctors.FirstOrDefault(d => d.Email == email && d.Password == password);

            if (doctor != null)
            {
                _currentDoctorId = doctor.DoctorId;
                Console.WriteLine($"\nУспішний вхід! Вітаємо, {doctor.Name}.");
            }
            else
            {
                Console.WriteLine("\nНевірний email або пароль!");
            }
        }

        private static void RegisterDoctor(HospitalContext ctx)
        {
            Console.Write("ПІБ Лікаря: ");
            var name = Console.ReadLine();
            Console.Write("Спеціальність: ");
            var specialty = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Пароль: ");
            var password = Console.ReadLine();

            var doctor = new Doctor
            {
                Name = name,
                Specialty = specialty,
                Email = email,
                Password = password
            };

            ctx.Doctors.Add(doctor);
            ctx.SaveChanges();
            Console.WriteLine("Лікаря успішно зареєстровано! Тепер оберіть пункт '1', щоб увійти.");
        }

        private static void AddPatient(HospitalContext ctx)
        {
            Console.Write("Ім'я: ");
            var firstName = Console.ReadLine();
            Console.Write("Прізвище: ");
            var lastName = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();

            var patient = new Patient
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Address = "Не вказано",
                HasInsurance = true
            };

            ctx.Patients.Add(patient);
            ctx.SaveChanges();
            Console.WriteLine("Пацієнта додано.");
        }

        private static void ListPatients(HospitalContext ctx)
        {
            var patients = ctx.Patients.ToList();
            foreach (var p in patients)
            {
                Console.WriteLine($"ID: {p.PatientId} | {p.FirstName} {p.LastName} | {p.Email}");
            }
        }

        private static void AddVisitation(HospitalContext ctx)
        {
            Console.Write("Введіть ID пацієнта: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                var patient = ctx.Patients.Find(patientId);
                if (patient == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    return;
                }

                Console.Write("Коментар до візиту: ");
                var comment = Console.ReadLine();

                var visitation = new Visitation
                {
                    Date = DateTime.Now,
                    Comments = comment,
                    PatientId = patientId,
                    DoctorId = _currentDoctorId 
                };

                ctx.Visitations.Add(visitation);
                ctx.SaveChanges();
                Console.WriteLine("Візит успішно записано.");
            }
            else
            {
                Console.WriteLine("Невірний формат ID.");
            }
        }
    }
}