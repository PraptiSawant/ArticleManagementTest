# ArticleManagement

How to Run

1. Clone repository
2. Update connection string
3. Run update-database
4. Run project
5. Open swagger

Implemented Details

## CRUD apis :

  Implemented for both Articles and Contents.<br>
  Controllers: 
  
      Controllers/ArticlesController.cs  
      Controllers/ContentsController.cs 

## User Data
  User records added manually.<br>
  INSERT INTO Users
  VALUES
  ('Riya',GETDATE()),
  ('Siya',GETDATE()),
  ('John',GETDATE())
  
## Article Details
  Returns an article along with its associated content items.<br>
  Endpoint: GET /api/Articles/{id}/details<br>
  Location: Controllers/ArticlesController.cs

## Paginated Articles API
  Endpoint: GET /api/Articles/paged<br>
  Location: Controllers/ArticlesController.cs

## Database Report Queries
  The DB queries are available in the Scripts/Reports.sql file.

