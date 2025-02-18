CREATE PROCEDURE InsertWord
    @Keyword NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM SensitiveWords WHERE Keyword = @Keyword)
BEGIN
INSERT INTO SensitiveWords (Keyword) VALUES (@Keyword);
END
END
