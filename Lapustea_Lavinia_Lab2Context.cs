using Microsoft.EntityFrameworkCore;

public class Lapustea_Lavinia_Lab2Context(DbContextOptions<Lapustea_Lavinia_Lab2Context> options) : DbContext(options)
{
    public DbSet<Lapustea_Lavinia_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Lapustea_Lavinia_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Lapustea_Lavinia_Lab2.Models.Authors> Authors { get; set; } = default!;
}
