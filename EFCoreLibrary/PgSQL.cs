/*
 * 1) NuGet -> Npgsql.EntityFrameworkCore.PostgreSQL
 * 2) connectionString -> 
 *  {
     "SqlConnectionString": "Data Source=MSI\\SQLEXPRESS;Database=MSTUCABotDb;Integrated Security=true;TrustServerCertificate=True; Encrypt=False;",
     "PostgreeSqlConnectionString": "Server=localhost;Database=MSTUCABotDb;Port=5432;User Id=postgres;Password=MaksPlay2;"
	}

	3) Method change:
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}
 */