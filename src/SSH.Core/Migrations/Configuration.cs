namespace SSH.Core.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Constant;
    using DBContext;
    using DTO;
    using Entity;
    using Enum;
    using Infrastructure;
    using IService;

    internal sealed class Configuration : DbMigrationsConfiguration<SSH.Core.DBContext.SSHContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SSHContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            
            #region Elamh
            this.ElmahSeed(context);
            #endregion

            this.SetupSeed(context);

            #region Initial User

            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
            if (roleManager.Roles.Count() == 0)
            {
                this.CreateRole(roleManager, UserRoles.None);
                this.CreateRole(roleManager, UserRoles.Admin);
                this.CreateRole(roleManager, UserRoles.HR);
                this.CreateRole(roleManager, UserRoles.InformationDesk);
                this.CreateRole(roleManager, UserRoles.Accounts);
                this.CreateRole(roleManager, UserRoles.Reception);
                this.CreateRole(roleManager, UserRoles.Pharmacy);
            }

            string emailAddress = "admin@admin.com";
            ApplicationUserManager manager = new ApplicationUserManager(new ApplicationUserStore(context));
            var applicationUser = manager.FindByEmailAsync(emailAddress).Result;
            if (applicationUser == null)
            {
                this.CreateAdmin(manager, context);
            }
            
            //Permission
            var permissions = new List<Permission>
            {
                //User
                new Permission() { Id = 1, Name = "UserListView", CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow, Group = PermissionGroup.User.ToString() },
                new Permission() { Id = 2, Name = "UserDelete", CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow, Group = PermissionGroup.User.ToString() },
                new Permission() { Id = 3, Name = "UserStatusChange", CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow, Group = PermissionGroup.User.ToString() },
                new Permission() { Id = 4, Name = "UserCreate", CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow, Group = PermissionGroup.User.ToString() },
                new Permission() { Id = 5, Name = "UserUpdate", CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow, Group = PermissionGroup.User.ToString() },
                new Permission() { Id = 6, Name = "UserAuthorize", CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow, Group = PermissionGroup.User.ToString() }
            };

            foreach (var permission in permissions)
            {
                var current = context.Permission.FirstOrDefault(x => x.Name == permission.Name);
                if (current == null)
                {
                    context.Permission.Add(permission);
                }
                else
                {
                    // update relevant properties here
                    current.Group = permission.Group;
                }
            }

            context.SaveChanges();

            // User Role Permission
            var listOfUserRolePermissions = new List<UserRolePermission>
            {
                //Admin
                //new UserRolePermission() { RoleID = "22681f8d-0989-4d8e-a49b-a49d701d7102", PermissionID = 1, CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow }
            };

            var allUserRolePermission = context.UserRolePermission.ToList();
            foreach (var item in listOfUserRolePermissions)
            {
                var current = allUserRolePermission.FirstOrDefault(x => x.RoleID == item.RoleID && x.PermissionID == item.PermissionID);
                if (current == null)
                {
                    context.UserRolePermission.Add(item);
                }
            }

            context.SaveChanges();
            
            #endregion

            #region Private Methods
            this.LOVs(context);
            #endregion
        }

        private void CreateAdmin(ApplicationUserManager manager, SSHContext context)
        {
            ApplicationUser applicationUser = new ApplicationUser();

            var adminUser = new UserDTO
            {
                FirstName = "Super",
                MiddleName = "Admin",
                LastName = "Admin",
                Password = "admin123",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Roles = new List<UserRoleDTO> { new UserRoleDTO { RoleId = Roles.GetRoleId(UserRoles.Admin) } },
                AccountRole = "Admin",
                Address = "Address Test",
                CellNumber = "+966123456787",
                DateOfBirth = DateTime.UtcNow,
                DateOfJoining = DateTime.UtcNow,
                DesignationId = 36,
                Education = "Education Test",
                LandLineNumber = "02139876543",
                Status = UserStatus.Active
            };

            applicationUser = adminUser.ConvertToEntity(applicationUser);
            applicationUser.CreatedOn = DateTime.UtcNow;
            applicationUser.LastModifiedOn = DateTime.UtcNow;

            var abc = manager.CreateAsync(applicationUser, "admin123");
            abc.Wait();
            var res = abc.Result;

            applicationUser = manager.FindByEmailAsync(applicationUser.Email).Result;
            manager.AddToRoleAsync(applicationUser.Id, UserRoles.Admin.ToString()).Wait();
        }

        private void CreateRole(ApplicationRoleManager roleManager, UserRoles role)
        {
            roleManager.CreateAsync(new ApplicationRole { Id = Roles.GetRoleId(role), Name = role.ToString(), CreatedOn = DateTime.UtcNow, LastModifiedOn = DateTime.UtcNow }).Wait();
        }

        private void LOVs(SSHContext context)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [LOVs]");
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('[LOVs]', RESEED, 1);");

            var lovs = new List<LOV>()
            {
                //new LOV { Group = "DPPauseReason", Key = "4", Value = "Pause for training", CreatedBy = "1", CreatedOn = DateTime.Now, LastModifiedOn = DateTime.Now, IsActive = true }
            };

            foreach (var lov in lovs)
            {
                var entity = context.LOV.FirstOrDefault(x => x.Group.Trim().ToLower() == lov.Group.Trim().ToLower() && x.Key.Trim().ToLower() == lov.Key.Trim().ToLower());
                if (entity == null)
                {
                    context.LOV.Add(lov);
                }
            }

            context.SaveChanges();
        }

        #region Elmah
        private void ElmahSeed(SSH.Core.DBContext.SSHContext context)
        {
            bool exists = context.Database
                     .SqlQuery<int?>(@"
                        IF EXISTS (SELECT 1 
                                   FROM INFORMATION_SCHEMA.TABLES 
                                   WHERE TABLE_TYPE='BASE TABLE' 
                                   AND TABLE_NAME='ELMAH_Error') 
                           SELECT 1 AS res ELSE SELECT 0 AS res;")
                     .SingleOrDefault() == 0;
            if (exists)
            {
                context.Database.ExecuteSqlCommand(@"CREATE TABLE [dbo].[ELMAH_Error]
                                (
                                    [ErrorId]     UNIQUEIDENTIFIER NOT NULL,
                                    [Application] NVARCHAR(60)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    [Host]        NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    [Type]        NVARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    [Source]      NVARCHAR(60)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    [Message]     NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    [User]        NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    [StatusCode]  INT NOT NULL,
                                    [TimeUtc]     DATETIME NOT NULL,
                                    [Sequence]    INT IDENTITY(1, 1) NOT NULL,
                                    [AllXml]      NTEXT COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
                                ) ");

                context.Database.ExecuteSqlCommand("EXEC('ALTER TABLE [dbo].[ELMAH_Error] WITH NOCHECK ADD CONSTRAINT[PK_ELMAH_Error] PRIMARY KEY([ErrorId])')");

                context.Database.ExecuteSqlCommand("EXEC('ALTER TABLE [dbo].[ELMAH_Error] ADD CONSTRAINT[DF_ELMAH_Error_ErrorId] DEFAULT(NEWID()) FOR[ErrorId]')");

                context.Database.ExecuteSqlCommand(@"EXEC('CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON [dbo].[ELMAH_Error] 
                                (
                                    [Application]   ASC,
                                    [TimeUtc]       DESC,
                                    [Sequence]      DESC
                                )')");

                context.Database.ExecuteSqlCommand(@"EXEC('CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml] (@Application NVARCHAR(60), @ErrorId UNIQUEIDENTIFIER) AS
                                    SET NOCOUNT ON
                                    SELECT [AllXml] FROM [ELMAH_Error] WHERE [ErrorId] = @ErrorId AND [Application] = @Application')");

                context.Database.ExecuteSqlCommand(@"EXEC('CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
                                (@Application NVARCHAR(60), @PageIndex INT = 0, @PageSize INT = 15, @TotalCount INT OUTPUT)
                                AS  
                                    SET NOCOUNT ON 
                                    DECLARE @FirstTimeUTC DATETIME
                                    DECLARE @FirstSequence INT
                                    DECLARE @StartRow INT
                                    DECLARE @StartRowIndex INT

                                    SELECT @TotalCount = COUNT(1) FROM [ELMAH_Error] WHERE [Application] = @Application

                                    SET @StartRowIndex = @PageIndex * @PageSize + 1

                                    IF @StartRowIndex <= @TotalCount
                                    BEGIN 
                                        SET ROWCOUNT @StartRowIndex

                                        SELECT @FirstTimeUTC = [TimeUtc], @FirstSequence = [Sequence] FROM [ELMAH_Error]
                                        WHERE [Application] = @Application ORDER BY [TimeUtc] DESC, [Sequence] DESC 
                                    END
                                    ELSE
                                    BEGIN 
                                        SET @PageSize = 0 
                                    END

                                    SET ROWCOUNT @PageSize

                                    SELECT 
                                        errorId     = [ErrorId], 
                                        application = [Application],
                                        host        = [Host], 
                                        type        = [Type],
                                        source      = [Source],
                                        message     = [Message],
                                        [user]      = [User],
                                        statusCode  = [StatusCode], 
                                        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + ''Z''
                                    FROM [ELMAH_Error] error WHERE [Application] = @Application AND [TimeUtc] <= @FirstTimeUTC
                                    AND [Sequence] <= @FirstSequence ORDER BY [TimeUtc] DESC, [Sequence] DESC FOR XML AUTO')");

                context.Database.ExecuteSqlCommand(@"EXEC('CREATE PROCEDURE [dbo].[ELMAH_LogError] (@ErrorId UNIQUEIDENTIFIER, @Application NVARCHAR(60), @Host NVARCHAR(30),
                                  @Type NVARCHAR(100), @Source NVARCHAR(60), @Message NVARCHAR(500), @User NVARCHAR(50), @AllXml NTEXT, @StatusCode INT,
                                  @TimeUtc DATETIME) AS 

                                 SET NOCOUNT ON

                                 INSERT INTO [ELMAH_Error] ([ErrorId], [Application], [Host], [Type], [Source], [Message], [User], [AllXml], [StatusCode], [TimeUtc])
                                 VALUES (@ErrorId, @Application, @Host, @Type, @Source, @Message, @User, @AllXml, @StatusCode, @TimeUtc)')");
            }

            context.Database.ExecuteSqlCommand(@"EXEC('IF (OBJECT_ID(''ELMAH_GetExceptionReportLogs'') IS NOT NULL)
                                               Begin
                                               DROP PROCEDURE ELMAH_GetExceptionReportLogs
                                               End')");

            context.Database.ExecuteSqlCommand(@"EXEC('CREATE PROCEDURE [dbo].[ELMAH_GetExceptionReportLogs] AS
                                                SET NOCOUNT ON
                                                SELECT Type,Message,[User],TimeUtc FROM [ELMAH_Error]')");

            context.Database.ExecuteSqlCommand(@"EXEC('IF (OBJECT_ID(''ELMAH_GetExceptionReportLogsForGivenDates'') IS NOT NULL)
                                               Begin
                                               DROP PROCEDURE ELMAH_GetExceptionReportLogsForGivenDates
                                               End')");

            context.Database.ExecuteSqlCommand(@"EXEC('CREATE PROCEDURE [dbo].[ELMAH_GetExceptionReportLogsForGivenDates] (@StartDate datetime, @EndDate datetime) AS
                                                begin
                                                    SET NOCOUNT ON
												    if(@EndDate is null)
												    begin
													    SELECT Type,Message,[User],TimeUtc FROM [ELMAH_Error] WHERE [TimeUtc] >= @StartDate
												    end
												    else if(@StartDate is null)
												    Begin
													    SELECT Type,Message,[User],TimeUtc FROM [ELMAH_Error] WHERE [TimeUtc] <= @EndDate
												    End
												    else
												    Begin
													    SELECT Type,Message,[User],TimeUtc FROM [ELMAH_Error] WHERE [TimeUtc] BETWEEN @StartDate AND @EndDate
												    End
                                                End')");
        }
        #endregion

        #region SetupSeed
        private void SetupSeed(SSH.Core.DBContext.SSHContext context)
        {
        }
        #endregion
    }
}