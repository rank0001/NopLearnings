
using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.TopSellingProducts.Models;
namespace Nop.Plugin.Widgets.TopSellingProducts.Infrastructure.Mapper;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor
    public MapperConfiguration()
    {
        CreatePluginStudentMaps();
        //add some generic mapping rules
    }

    #endregion

    #region Properties
    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 5;

    protected virtual void CreatePluginStudentMaps()
    {
        #endregion
    }
}