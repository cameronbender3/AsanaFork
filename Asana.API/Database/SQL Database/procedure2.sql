CREATE PROCEDURE ProjectExpandAll
AS
    SELECT
        p.*,
        t.ID AS ToDoID,
        t.NAME AS ToDoName,
        t.DESCRIPTION AS ToDoDescription,
        t.IS_COMPLETED,
        t.PRIORITY,
        t.DueDate
    FROM Project p
    LEFT JOIN ToDo t ON p.ID = t.ProjectId
