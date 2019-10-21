namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AcceptStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Appointments", "Problem", c => c.String());
            AlterColumn("dbo.Appointments", "Weight", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "Weight", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "Problem", c => c.String(nullable: false));
            DropColumn("dbo.Appointments", "AcceptStatus");
        }
    }
}
