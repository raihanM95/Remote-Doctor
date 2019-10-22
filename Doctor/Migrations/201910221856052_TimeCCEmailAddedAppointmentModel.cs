namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeCCEmailAddedAppointmentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Time", c => c.String(maxLength: 20));
            AddColumn("dbo.Appointments", "CCEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "CCEmail");
            DropColumn("dbo.Appointments", "Time");
        }
    }
}
