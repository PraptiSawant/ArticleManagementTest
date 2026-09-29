-- REPORT 1
-- Report to show all authors and array of article ids written by them

SELECT
    u.Id AS AuthorId,
    u.Username AS Author,
    STRING_AGG(CAST(c.ArticleId AS VARCHAR(MAX)), ', ') AS ArticleIds
FROM Users u
LEFT JOIN Contents c
    ON u.Id = c.AuthorId
GROUP BY
    u.Id,
    u.Username;



-- REPORT 2
-- Report to show all articles created in past 3 months, by users created in the past 4 months with specified language (+ points for script with variables)

DECLARE @LanguageText VARCHAR(20) = 'English';
DECLARE @Language INT;

-- Map the string text to enum
SET @Language = CASE @LanguageText
    WHEN 'English' THEN 0
    WHEN 'Spanish' THEN 1
    WHEN 'French'  THEN 2
    ELSE -1 
END;

SELECT DISTINCT
    a.Id AS ArticleId,
    a.CreatedAt AS ArticleCreatedAt,
    u.Id AS AuthorId,
    u.Username AS Author,
    c.Language
FROM Articles a
INNER JOIN Contents c
    ON a.Id = c.ArticleId
INNER JOIN Users u
    ON u.Id = c.AuthorId
WHERE
    a.CreatedAt >= DATEADD(MONTH, -3, GETDATE())
    AND u.CreatedAt >= DATEADD(MONTH, -4, GETDATE())
    AND c.Language = @Language;