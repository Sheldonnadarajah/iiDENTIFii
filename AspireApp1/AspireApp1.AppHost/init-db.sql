
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AspireDb')
CREATE DATABASE AspireDb;
GO

USE AspireDb;
GO


IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PostTags')
DROP TABLE [PostTags];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Tags')
DROP TABLE [Tags];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Likes')
DROP TABLE [Likes];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Comments')
DROP TABLE [Comments];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Posts')
DROP TABLE [Posts];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
DROP TABLE [Users];

-- Create User table
CREATE TABLE [Users] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL UNIQUE,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Role] TINYINT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);


CREATE TABLE [Posts] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [UserId] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
);


CREATE TABLE [Comments] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [PostId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY ([PostId]) REFERENCES [Posts]([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
);

-- Create Like table
CREATE TABLE [Likes] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [PostId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY ([PostId]) REFERENCES [Posts]([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT UQ_Like_Post_User UNIQUE ([PostId], [UserId])
);


CREATE TABLE [Tags] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);


-- PostTags join table for many-to-many relationship
CREATE TABLE [PostTags] (
    [PostId] INT NOT NULL,
    [TagId] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY ([PostId]) REFERENCES [Posts]([Id]),
    FOREIGN KEY ([TagId]) REFERENCES [Tags]([Id]),
    CONSTRAINT UQ_Post_Tag UNIQUE ([PostId], [TagId])
);




INSERT INTO [Users] ([Username], [Email], [PasswordHash], [Role], [CreatedAt]) VALUES
('Moderator', 'moderator1@example.com', 'Z0jsi3ZoO3OIcZ0X29H2gvCFOxRC1LMe0Sw/Qe7VE9s=', 1, '2024-01-01T10:00:00Z'),
('Alice', 'alice@example.com', 'FakeHash1', 0, '2024-02-15T12:00:00Z'),
('Bob', 'bob@example.com', 'FakeHash2', 0, '2024-03-10T15:30:00Z'),
('Charlie', 'charlie@example.com', 'FakeHash3', 0, '2024-03-12T09:00:00Z'),
('Diana', 'diana@example.com', 'FakeHash4', 0, '2024-03-14T11:00:00Z');

-- Seed Tags
INSERT INTO [Tags] ([Name], [CreatedAt]) VALUES
('Tech', '2024-01-10T09:00:00Z'),
('Life', '2024-02-20T11:00:00Z'),
('News', '2024-03-05T14:00:00Z'),
('Fun', '2024-04-01T16:00:00Z');

-- Seed Posts (5 for Alice, 5 for Bob)
INSERT INTO [Posts] ([Title], [Content], [UserId], [CreatedAt]) VALUES
('Alice Post 1', 'Content of Alice Post 1', 2, '2024-02-16T08:00:00Z'),
('Alice Post 2', 'Content of Alice Post 2', 2, '2024-02-18T10:30:00Z'),
('Alice Post 3', 'Content of Alice Post 3', 2, '2024-02-20T13:45:00Z'),
('Alice Post 4', 'Content of Alice Post 4', 2, '2024-02-25T09:15:00Z'),
('Alice Post 5', 'Content of Alice Post 5', 2, '2024-03-01T17:20:00Z'),
('Bob Post 1', 'Content of Bob Post 1', 3, '2024-03-11T08:00:00Z'),
('Bob Post 2', 'Content of Bob Post 2', 3, '2024-03-13T10:30:00Z'),
('Bob Post 3', 'Content of Bob Post 3', 3, '2024-03-15T13:45:00Z'),
('Bob Post 4', 'Content of Bob Post 4', 3, '2024-03-20T09:15:00Z'),
('Bob Post 5', 'Content of Bob Post 5', 3, '2024-03-25T17:20:00Z');

-- Seed Comments (multiple per post, from different users)
INSERT INTO [Comments] ([PostId], [UserId], [Content], [CreatedAt]) VALUES
(1, 3, 'Nice post, Alice!', '2024-02-16T09:00:00Z'),
(1, 4, 'I agree with Bob, great post!', '2024-02-16T09:10:00Z'),
(1, 5, 'Very insightful, thanks for sharing.', '2024-02-16T09:20:00Z'),
(2, 3, 'Great thoughts!', '2024-02-18T11:00:00Z'),
(2, 4, 'Love this perspective.', '2024-02-18T11:10:00Z'),
(3, 3, 'Interesting!', '2024-02-20T14:00:00Z'),
(3, 5, 'I learned something new today.', '2024-02-20T14:10:00Z'),
(4, 3, 'Thanks for sharing.', '2024-02-25T10:00:00Z'),
(4, 4, 'This is helpful.', '2024-02-25T10:10:00Z'),
(5, 3, 'Good read.', '2024-03-01T18:00:00Z'),
(5, 5, 'Keep it up!', '2024-03-01T18:10:00Z'),
(6, 2, 'Nice post, Bob!', '2024-03-11T09:00:00Z'),
(6, 4, 'Great job, Bob!', '2024-03-11T09:10:00Z'),
(7, 2, 'Great info!', '2024-03-13T11:00:00Z'),
(7, 5, 'Super useful.', '2024-03-13T11:10:00Z'),
(8, 2, 'Very helpful.', '2024-03-15T14:00:00Z'),
(8, 4, 'Thanks for posting.', '2024-03-15T14:10:00Z'),
(9, 2, 'Thanks for this.', '2024-03-20T10:00:00Z'),
(9, 5, 'Appreciate the details.', '2024-03-20T10:10:00Z'),
(10, 2, 'Good job.', '2024-03-25T18:00:00Z'),
(10, 4, 'Well written!', '2024-03-25T18:10:00Z');

-- Seed Likes (each user likes the other's posts)
INSERT INTO [Likes] ([PostId], [UserId], [CreatedAt]) VALUES
(1, 3, '2024-02-16T09:05:00Z'),
(2, 3, '2024-02-18T11:05:00Z'),
(3, 3, '2024-02-20T14:05:00Z'),
(4, 3, '2024-02-25T10:05:00Z'),
(5, 3, '2024-03-01T18:05:00Z'),
(6, 2, '2024-03-11T09:05:00Z'),
(7, 2, '2024-03-13T11:05:00Z'),
(8, 2, '2024-03-15T14:05:00Z'),
(9, 2, '2024-03-20T10:05:00Z'),
(10, 2, '2024-03-25T18:05:00Z');

-- Seed PostTags (random tags for each post)
INSERT INTO [PostTags] ([PostId], [TagId], [CreatedAt]) VALUES
(1, 1, '2024-02-16T08:10:00Z'), (1, 2, '2024-02-16T08:15:00Z'),
(2, 2, '2024-02-18T10:40:00Z'), (2, 3, '2024-02-18T10:45:00Z'),
(3, 1, '2024-02-20T13:55:00Z'), (3, 4, '2024-02-20T14:00:00Z'),
(4, 3, '2024-02-25T09:25:00Z'), (4, 2, '2024-02-25T09:30:00Z'),
(5, 4, '2024-03-01T17:30:00Z'), (5, 1, '2024-03-01T17:35:00Z'),
(6, 2, '2024-03-11T08:10:00Z'), (6, 3, '2024-03-11T08:15:00Z'),
(7, 1, '2024-03-13T10:40:00Z'), (7, 4, '2024-03-13T10:45:00Z'),
(8, 3, '2024-03-15T13:55:00Z'), (8, 2, '2024-03-15T14:00:00Z'),
(9, 4, '2024-03-20T09:25:00Z'), (9, 1, '2024-03-20T09:30:00Z'),
(10, 2, '2024-03-25T17:30:00Z'), (10, 3, '2024-03-25T17:35:00Z');