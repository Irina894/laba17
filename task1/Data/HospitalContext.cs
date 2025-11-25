using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        // 2. Ваші таблиці
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Якщо налаштування ще не задані -> задаємо їх тут
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=HospitalDatabase;Integrated Security=True;TrustServerCertificate=True;");

                // АБО, якщо не спрацює перший, розкоментуйте цей (для повноцінного SQL Server):
                // optionsBuilder.UseSqlServer("Server=.;Database=HospitalDatabase;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        // 4. Налаштування зв'язків та обмежень
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId);
                entity.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(true);
                entity.Property(e => e.LastName).HasMaxLength(50).IsUnicode(true);
                entity.Property(e => e.Address).HasMaxLength(250).IsUnicode(true);
                entity.Property(e => e.Email).HasMaxLength(80).IsUnicode(false);
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.HasKey(e => e.VisitationId);
                entity.Property(e => e.Comments).HasMaxLength(250).IsUnicode(true);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(e => e.DiagnoseId);
                entity.Property(e => e.Name).HasMaxLength(50).IsUnicode(true);
                entity.Property(e => e.Comments).HasMaxLength(250).IsUnicode(true);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.MedicamentId);
                entity.Property(e => e.Name).HasMaxLength(50).IsUnicode(true);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.MedicamentId });

                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(e => e.PatientId);

                entity.HasOne(e => e.Medicament)
                      .WithMany(m => m.Prescriptions)
                      .HasForeignKey(e => e.MedicamentId);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DoctorId);

                entity.Property(e => e.Name)
                      .HasMaxLength(100) 
                      .IsUnicode(true); 

                entity.Property(e => e.Specialty)
                      .HasMaxLength(100) 
                      .IsUnicode(true);  
            });
        }
    }
}