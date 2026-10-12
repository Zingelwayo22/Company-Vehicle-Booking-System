namespace CompanyVehicleBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepartments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            AddColumn("dbo.AspNetUsers", "DepartmentId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "DepartmentId");
            AddForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Departments", "DepartmentId");
            DropColumn("dbo.AspNetUsers", "Department");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Department", c => c.String(maxLength: 100));
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropColumn("dbo.AspNetUsers", "DepartmentId");
            DropTable("dbo.Departments");
        }
    }
}
