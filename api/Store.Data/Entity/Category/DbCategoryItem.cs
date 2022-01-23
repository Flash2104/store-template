using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entity.Product;

namespace Store.Data.Entity.Category;

public class DbCategoryItem : DbEntity<int>
{
    public string Title { get; set; } = null!;
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public int Order { get; set; }
    public bool IsDisabled { get; set; }

    public int CategoryTreeId { get; set; }
    public virtual DbCategoryTree? CategoryTree { get; set; }

    public virtual List<DbProductsToCategories>? ProductsToCategories { get; set; }
}

internal sealed class DbCategoryItemMapping
{
    public void Map(EntityTypeBuilder<DbCategoryItem> builder)
    {
        builder.ToTable("CategoryItems");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);

        builder
            .HasOne(x => x.CategoryTree)
            .WithMany(x => x.CategoryItems)
            .HasForeignKey(x => x.CategoryTreeId);
        BuildItems(builder);
    }

    private void BuildItems(EntityTypeBuilder<DbCategoryItem> builder)
    {
        var createdDate = new DateTime(2022, 12, 29, 13, 18, 00);
        var treeId = 1;
        var userId = 1;
        var items = new List<DbCategoryItem>()
        {
            new DbCategoryItem() // Страйкбольное оружие
            {
                Id = 1,
                Title = "Страйкбольное оружие",
                Order = 1
            },
            new DbCategoryItem() // Страйкбольное оружие -> Винтовки
            {
                Id = 6,
                Title = "Винтовки",
                Order = 2,
                ParentId = 1
            },
            new DbCategoryItem() // Страйкбольное оружие -> Пулеметы
            {
                Id = 7,
                Title = "Пулеметы",
                Order = 3,
                ParentId = 1
            },
            new DbCategoryItem() // Страйкбольное оружие -> Пистолеты
            {
                Id = 8,
                Title = "Пистолеты",
                Order = 1,
                ParentId = 1
            },
            new DbCategoryItem() // Пневматическое оружие
            {
                Id = 2,
                Title = "Пневматическое оружие",
                Order = 2
            },
            new DbCategoryItem() // Пневматическое оружие -> Винтовки
            {
                Id = 9,
                Title = "Винтовки",
                Order = 2,
                ParentId = 2
            },
            new DbCategoryItem() // Пневматическое оружие -> Пистолеты
            {
                Id = 10,
                Title = "Пистолеты",
                Order = 1,
                ParentId = 2
            },
            new DbCategoryItem() // Аккумуляторы и ЗУ
            {
                Id = 3,
                Title = "Аккумуляторы и ЗУ",
                Order = 3
            },
            new DbCategoryItem() // Аккумуляторы и ЗУ -> Аккумуляторы LiPo
            {
                Id = 11,
                Title = "Зарядные устройства",
                Order = 1,
                ParentId = 3
            },
            new DbCategoryItem() // Аккумуляторы и ЗУ -> Зарядные устройства
            {
                Id = 12,
                Title = "Зарядные устройства",
                Order = 2,
                ParentId = 3
            },
            new DbCategoryItem() // Пиротехника
            {
                Id = 4,
                Title = "Пиротехника",
                Order = 4
            },
            new DbCategoryItem() // Пиротехника -> Выстрелы "СтрайтАрт"
            {
                Id = 13,
                Title = "Выстрелы \"СтрайтАрт\"",
                Order = 1,
                ParentId = 4
            },
            new DbCategoryItem() // Пиротехника -> Мины и ручные гранаты
            {
                Id = 14,
                Title = "Мины и ручные гранаты",
                Order = 2,
                ParentId = 4
            },
            new DbCategoryItem() // Все товары
            {
                Id = 5,
                Title = "Все товары",
                Order = 5
            },
            new DbCategoryItem() // Все товары -> Внешний обвес
            {
                Id = 15,
                Title = "Внешний обвес",
                Order = 1,
                ParentId = 5
            },
            new DbCategoryItem() // Все товары -> Внешний обвес -> Антабки
            {
                Id = 16,
                Title = "Антабки",
                Order = 1,
                ParentId = 15
            },
            new DbCategoryItem() // Все товары -> Внешний обвес -> Глушители
            {
                Id = 17,
                Title = "Глушители",
                Order = 2,
                ParentId = 15
            },
            new DbCategoryItem() // Все товары -> Защита
            {
                Id = 18,
                Title = "Защита",
                Order = 2,
                ParentId = 5
            },
            new DbCategoryItem() // Все товары -> Защита -> Шлема
            {
                Id = 19,
                Title = "Шлема",
                Order = 1,
                ParentId = 18
            },
            new DbCategoryItem() // Все товары -> Защита -> Очки
            {
                Id = 20,
                Title = "Очки",
                Order = 2,
                ParentId = 18
            },
            new DbCategoryItem() // Все товары -> Защита -> Перчатки
            {
                Id = 21,
                Title = "Перчатки",
                Order = 3,
                ParentId = 18
            },
            new DbCategoryItem() // Все товары -> Снаряжение
            {
                Id = 22,
                Title = "Снаряжение",
                Order = 3,
                ParentId = 5
            },
            new DbCategoryItem() // Все товары -> Снаряжение -> Рюкзаки
            {
                Id = 23,
                Title = "Рюкзаки",
                Order = 1,
                ParentId = 22
            },
            new DbCategoryItem() // Все товары -> Снаряжение -> Подсумки
            {
                Id = 24,
                Title = "Подсумки",
                Order = 2,
                ParentId = 22,
            },
        };
        var dbCategoryItems = items.Select(a =>
        {
            a.ModifiedDate = createdDate;
            a.CategoryTreeId = treeId;
            a.CreatedBy = userId;
            a.CreatedDate = createdDate;
            return a;
        }).ToList();
        builder.HasData(dbCategoryItems);
    }
}