using Microsoft.EntityFrameworkCore;
using MovieWebAPI.Models;

namespace MovieWebAPI.Persistence.Configuration
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Genre).IsRequired().HasMaxLength(100);
            builder.Property(m => m.ReleaseDate).IsRequired();
            builder.Property(m => m.Rating).IsRequired();

            builder.Property(m => m.Created).IsRequired().ValueGeneratedOnAdd();
            builder.Property(m => m.LastModified).IsRequired().ValueGeneratedOnUpdate();

            builder.HasIndex(m => m.Title);

            builder.HasQueryFilter("Released", m => m.ReleaseDate <= DateTimeOffset.UtcNow);
            builder.HasQueryFilter("HighlyRated", m => m.Rating >= 8.0);

        }
    }
}
