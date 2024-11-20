-- Add new permissions
INSERT INTO Permissions (PermissionName, Description) VALUES 
('AccessADAAccount', 'Can access ADA Account'),
('AccessADAFSAC', 'Can access ADA FSAC'),
('AccessADDED', 'Can access ADDED'),
('AccessADEXStudentInfo', 'Can access ADEX Student Info'),
('AccessADID', 'Can access ADID'),
('AccessADK', 'Can access ADK'),
('AccessADWA', 'Can access ADWA'),
('AccessADMILITARY', 'Can access AD MILITARY'),
('AccessAMPerson', 'Can access AM Person');

-- Assign new permissions to Admin role
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT r.RoleId, p.PermissionId
FROM Roles r
CROSS JOIN Permissions p
WHERE r.RoleName = 'Admin' AND p.PermissionName IN (
    'AccessADAAccount', 'AccessADAFSAC', 'AccessADDED', 'AccessADEXStudentInfo',
    'AccessADID', 'AccessADK', 'AccessADWA', 'AccessADMILITARY', 'AccessAMPerson'
);

-- Assign some permissions to Manager role (you can adjust this as needed)
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT r.RoleId, p.PermissionId
FROM Roles r
CROSS JOIN Permissions p
WHERE r.RoleName = 'Manager' AND p.PermissionName IN (
    'AccessADAAccount', 'AccessADAFSAC', 'AccessADDED', 'AccessADEXStudentInfo'
);

-- Assign limited permissions to User role (you can adjust this as needed)
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT r.RoleId, p.PermissionId
FROM Roles r
CROSS JOIN Permissions p
WHERE r.RoleName = 'User' AND p.PermissionName IN (
    'AccessADAAccount', 'AccessADEXStudentInfo'
);
