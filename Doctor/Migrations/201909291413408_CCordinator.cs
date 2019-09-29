namespace Doctor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CCordinator : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CCordinators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CCordinatorName = c.String(nullable: false),
                        CCordinatorImagePath = c.String(),
                        CCordinatorPhone = c.String(),
                        CCordinatorEmail = c.String(nullable: false),
                        CCordinatorPassword = c.String(nullable: false, maxLength: 200),
                        IsEmailVarified = c.Boolean(nullable: false),
                        ActivationCode = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CCordinators");
        }
    }
}
