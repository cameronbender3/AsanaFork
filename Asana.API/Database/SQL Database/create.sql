CREATE DATABASE ASANADB

CREATE TABLE ToDo(
    ID INT IDENTITY(1,1) NOT NULL
    , NAME NVARCHAR(255) NULL
    , DESCRIPTION NVARCHAR(255) NULL
    , IS_COMPLETED bit NULL
    , PRIORITY INT NULL
)

ALTER TABLE ToDo
ADD ProjectId INT NULL,
    DueDate DATETIME NOT NULL;

SELECT * FROM ToDo

CREATE TABLE Project(
    ID INT IDENTITY(1,1) NOT NULL
    , NAME NVARCHAR(255) NULL
    , DESCRIPTION NVARCHAR(255) NULL
    , COMPLETE_PERCENT INT NOT NULL
)

INSERT INTO Project (NAME, DESCRIPTION, COMPLETE_PERCENT)
VALUES ('My New Project', 'This is a test project', 0);

DELETE FROM Project
WHERE ID = 1;

select * from Project

DECLARE @newId INT;

EXEC ToDoInsert 
    @todoName = 'Another ToDo Expand',
    @description = 'This is a sample ToDo item.',
    @isCompleted = 0,
    @priority = 1,
    @DueDate = '2025-08-01',
    @projectId = 3,      
    @id = @newId OUTPUT;

SELECT @newId AS NewToDoId;

EXEC ToDoUpdate
    @id = 1,
    @todoName = 'Updated ToDo',
    @description = 'Works',
    @isCompleted = 1,
    @priority = 2,
    @dueDate = '2025-08-15',
    @projectId = 1;

EXEC ToDoDelete
    @id = 1


EXEC ProjectUpdate
    @id = 1,
    @projectName = "Updated project",
    @description = "Updated description",
    @complete_percent = 100


DECLARE @newId INT;

EXEC ProjectInsert
    @projectName = 'Project NoToDos',
    @description = 'Description 2',
    @complete_percent = 0,
    @id = @newId OUTPUT;

SELECT @newId AS NewProjectId;

EXEC ProjectDelete
    @id = 1

EXEC ProjectExpandById
    @id = 3

    
EXEC ProjectExpandAll