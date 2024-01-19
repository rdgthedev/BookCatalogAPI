using BookCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookCatalog.Data.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<CategoryModel>
    {
        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .HasColumnName("Id")
                .HasColumnType("INT");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.HasMany(x => x.Books)
                .WithOne(x => x.Category)
                .HasConstraintName("FK_Category_Book")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Title, "IX_Category_Title")
                .IsUnique(); 
        }
    }
}
