#EF Scripts

Run the following from your Developer Powershell.
You may need to install dotnet/entityframework tools.
Note: this is a command line, so passwords with special characters may need help.
```
dotnet ef dbcontext scaffold "data source=your-fully-qualified-server-name-here;initial catalog=your-database-name-here;integrated security=False;user id=your-user-name;password=your-password;multipleactiveresultsets=True;encrypt=true;Trusted_Connection=false;" Microsoft.EntityFrameworkCore.SqlServer --use-database-names --no-onconfiguring -f -c PlaceholderContext -o Models
```
