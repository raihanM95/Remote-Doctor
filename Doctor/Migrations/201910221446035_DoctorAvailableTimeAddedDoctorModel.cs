namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorAvailableTimeAddedDoctorModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "StartTime", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Doctors", "EndTime", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Doctors", "AvailableTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "AvailableTime", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Doctors", "EndTime");
            DropColumn("dbo.Doctors", "StartTime");
        }
    }
}
