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

            Console.WriteLine("=== Система управління лікарнею v2.0 ===");

            while (_currentDoctorId == null)
            {
                Console.WriteLine("\n1. Увійти (Login)");
                Console.WriteLine("2. Зареєструвати нового лікаря");
                Console.WriteLine("3. Вихід");
                Console.Write("Ваш вибір: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Login(ctx); break;
                    case "2": RegisterDoctor(ctx); break;
                    case "3": return;
                    default: Console.WriteLine("Невірний вибір."); break;
                }
            }

            while (true)
            {
                Console.WriteLine("\n--- ГОЛОВНЕ МЕНЮ ---");
                Console.WriteLine("1. Переглянути всіх пацієнтів");
                Console.WriteLine("2. Додати нового пацієнта");
                Console.WriteLine("3. Провести візит (Додати запис)");
                Console.WriteLine("4. Вихід");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListPatients(ctx); break;
                    case "2": AddPatient(ctx); break;
                    case "3": AddVisitation(ctx); break;
                    case "4": return;
                    default: Console.WriteLine("Невірний вибір."); break;
                }
            }
        }

        private static void Login(HospitalContext ctx)
        {
            Console.Write("Введіть Email: ");
            var email = Console.ReadLine();
            Console.Write("Введіть Пароль: ");
            var password = Console.ReadLine();

            var doctor = ctx.Doctors.FirstOrDefault(d => d.Email == email && d.Password == password);

            if (doctor != null)
            {
                _currentDoctorId = doctor.DoctorId;
                Console.WriteLine($"\nВітаємо, д-р {doctor.Name}! Ви успішно увійшли.");
            }
            else
            {
                Console.WriteLine("\nПомилка: Невірний email або пароль.");
            }
        }

        private static void RegisterDoctor(HospitalContext ctx)
        {
            Console.Write("Ім'я та Прізвище: ");
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
            Console.WriteLine("Лікаря успішно зареєстровано! Тепер ви можете увійти.");
        }

        private static void AddPatient(HospitalContext ctx)
        {
            Console.Write("Ім'я пацієнта: ");
            var firstName = Console.ReadLine();
            Console.Write("Прізвище пацієнта: ");
            var lastName = Console.ReadLine();
            Console.Write("Email пацієнта: ");
            var email = Console.ReadLine();

            var patient = new Patient
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Address = "Unknown",
                HasInsurance = true
            };

            ctx.Patients.Add(patient);
            ctx.SaveChanges();
            Console.WriteLine("Пацієнта додано.");
        }

        private static void ListPatients(HospitalContext ctx)
        {
            var patients = ctx.Patients.ToList();
            Console.WriteLine("\n--- Список Пацієнтів ---");
            foreach (var p in patients)
            {
                Console.WriteLine($"ID: {p.PatientId} | {p.FirstName} {p.LastName}");
            }
        }

        private static void AddVisitation(HospitalContext ctx)
        {
            Console.Write("Введіть ID пацієнта: ");
            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Невірний формат ID.");
                return;
            }

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
            Console.WriteLine($"Візит успішно додано! (Проводив д-р ID: {_currentDoctorId})");
        }
    }
}