Add-Migration AddFirstName -Context ApplicationDbContext

Update-database -Context ApplicationDbContext

Scaffold-DbContext "Server=.\sqlexpress;Database=tts;UID=sa;Pwd=1234qwe@;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force