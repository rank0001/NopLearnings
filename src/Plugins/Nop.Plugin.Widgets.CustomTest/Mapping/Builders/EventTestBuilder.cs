using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Mapping.Builders;
public class EventTestBuilder : NopEntityBuilder<EventTest>
{
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        //map the primary key (not necessary if it is Id field)
        table
        .WithColumn(nameof(EventTest.IsInserted)).AsBoolean().Nullable();   
    }
}
