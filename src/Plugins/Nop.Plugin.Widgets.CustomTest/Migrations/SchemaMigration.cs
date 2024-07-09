using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Migrations;

[NopSchemaMigration("2024/07/09 07:30:55:1687541", "Other.ProductViewTracker base schema", MigrationProcessType.Update)]

public class SchemaMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(ProductViewTrackerRecord))).Exists())
            Create.TableFor<ProductViewTrackerRecord>();
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Student))).Exists())
            Create.TableFor<Student>();
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(EventTest))).Exists())
            Create.TableFor<EventTest>();
    }
}