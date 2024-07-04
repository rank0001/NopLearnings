using FluentMigrator;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Migrations;

[NopSchemaMigration("2024-07-03 00:00:00", "Category. Add some new property")]
public class AddSomeNewProperty : ForwardOnlyMigration
{
    public override void Up()
    {
        var categoryTableName = nameof(Category);
        if (!Schema.Table(categoryTableName).Column(nameof(Category.SomeNewProperty)).Exists())
            Alter.Table(categoryTableName)
                .AddColumn(nameof(Category.SomeNewProperty)).AsString(255).Nullable();
    }
}
