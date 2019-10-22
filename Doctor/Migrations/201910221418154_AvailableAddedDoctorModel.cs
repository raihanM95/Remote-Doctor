namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvailableAddedDoctorModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "AvailableTime", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "AvailableTime");
        }
    }
}
