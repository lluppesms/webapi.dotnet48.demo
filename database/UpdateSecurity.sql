-- This script will add your web app's managed identity into the dbo role
CREATE USER [yourWebAppName] FROM EXTERNAL PROVIDER
ALTER ROLE db_owner ADD MEMBER  [yourWebAppName] 
