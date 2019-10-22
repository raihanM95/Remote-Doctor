namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentScheduleAddedAppointmentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentTime", c => c.String(maxLength: 20));
            AddColumn("dbo.Appointments", "AppointmentDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Appointments", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "Time", c => c.String(maxLength: 20));
            DropColumn("dbo.Appointments", "AppointmentDate");
            DropColumn("dbo.Appointments", "AppointmentTime");
        }
    }
}
