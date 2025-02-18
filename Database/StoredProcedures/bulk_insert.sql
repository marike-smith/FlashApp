CREATE TYPE KeywordTableType AS TABLE (
    Keyword NVARCHAR(100) UNIQUE
    );

CREATE PROCEDURE InsertReservedKeywordsBulk
    @Keywords KeywordTableType READONLY
AS
BEGIN
INSERT INTO SensitiveWords (Keyword)
SELECT k.Keyword FROM @Keywords k
WHERE NOT EXISTS (SELECT 1 FROM SensitiveWords r WHERE r.Keyword = k.Keyword);
END
