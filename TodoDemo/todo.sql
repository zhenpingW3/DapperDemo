CREATE TABLE Todo (
                    Id INT PRIMARY KEY,
                    Title NVARCHAR(100) NOT NULL,
                    Completed BIT NOT NULL,
                    Description NVARCHAR(255),
                    Creator NVARCHAR(50),
                    CreateTime DATETIME NOT NULL DEFAULT GETDATE()
                )