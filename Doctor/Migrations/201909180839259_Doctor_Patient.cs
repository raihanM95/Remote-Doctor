namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Doctor_Patient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        RunningMedicine = c.Boolean(nullable: false),
                        BP = c.String(),
                        Problem = c.String(nullable: false),
                        Weight = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        DoctorsId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorsId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.DoctorsId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorName = c.String(nullable: false),
                        DoctorBirthDate = c.DateTime(nullable: false),
                        DoctorEmail = c.String(nullable: false),
                        DoctorImagePath = c.String(),
                        DoctorDegree = c.String(nullable: false, maxLength: 200),
                        RegNo = c.String(nullable: false),
                        DoctorDetails = c.String(nullable: false, maxLength: 200),
                        DoctorPassword = c.String(nullable: false, maxLength: 200),
                        IsEmailVarified = c.Boolean(nullable: false),
                        ActivationCode = c.Guid(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptName = c.String(nullable: false),
                        DeptDetails = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false),
                        PatientBirthDate = c.DateTime(),
                        PatientPhone = c.String(),
                        PatientEmail = c.String(nullable: false),
                        BloodGroup = c.String(),
                        PatientImagePath = c.String(),
                        PatientPassword = c.String(nullable: false, maxLength: 200),
                        IsEmailVarified = c.Boolean(nullable: false),
                        ActivationCode = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageText = c.String(nullable: false),
                        Sender = c.String(nullable: false),
                        Reciver = c.String(nullable: false),
                        time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        Dose = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        Comment = c.String(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Advice = c.String(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestName = c.String(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                        ImagePath = c.String(),
                        AppointmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .Index(t => t.AppointmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Medicines", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Appointments", "DoctorsId", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Reports", new[] { "AppointmentId" });
            DropIndex("dbo.Prescriptions", new[] { "AppointmentId" });
            DropIndex("dbo.Medicines", new[] { "AppointmentId" });
            DropIndex("dbo.Doctors", new[] { "DepartmentId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "DoctorsId" });
            DropTable("dbo.Reports");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Medicines");
            DropTable("dbo.Chats");
            DropTable("dbo.Patients");
            DropTable("dbo.Departments");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
