using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Migrations;

[NopSchemaMigration("2024/07/08 01:40:55:1687541", "Other.ProductViewTracker base schema", MigrationProcessType.Installation)]

public class SchemaMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<ProductViewTrackerRecord>();
        Create.TableFor<Student>();
    }
}