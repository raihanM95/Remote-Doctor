using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor
{
    public class DataContex : DbContext
    {
        public DataContex() : base("name=MyDbConnection")
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<CCordinator> CCordinators { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctors> Doctorses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
