
-- Create UserRoles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserRoles' and xtype='U')
BEGIN
    CREATE TABLE UserRoles (
        UserId INT,
        RoleId INT,
        CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
        CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
        CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
    );
END
