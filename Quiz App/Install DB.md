🗄️Restore Database
1. Download QuizAppDB.bak from the shared link.
2. Open SSMS and connect to SQL Server.
3. Right-click Databases → Restore Database.
4. Select Device → click ... → Add.
5. Choose QuizAppDB.bak and click OK.
6. Set the destination database name to QuizAppDB.
7. Click OK and wait for the restore to finish.
8. Right-click Databases → Refresh to see the database.
Important: If QuizAppDB already exists, do not overwrite it unless you are sure you do not need its current data.
Official guide: https://learn.microsoft.com/en-us/sql/relational-databases/backup-restore/restore-a-database-backup-using-ssms