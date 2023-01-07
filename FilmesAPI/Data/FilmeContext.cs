using FilmesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Data;

public class FilmeContext : DbContext
{
	public FilmeContext(DbContextOptions<FilmeContext>context) : base(context)
	{

	}

	public DbSet<Filme> Filmes { get; set; }
}
