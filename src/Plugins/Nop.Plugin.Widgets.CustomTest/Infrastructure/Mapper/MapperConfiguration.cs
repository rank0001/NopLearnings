﻿using AutoMapper;
using AutoMapper.Internal;
using Nop.Core.Configuration;
using Nop.Core.Domain.Affiliates;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Gdpr;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Logging;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Polls;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Topics;
using Nop.Core.Domain.Vendors;
using Nop.Core.Infrastructure.Mapper;
using Nop.Data.Configuration;
using Nop.Services.Authentication.External;
using Nop.Services.Authentication.MultiFactor;
using Nop.Services.Cms;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Pickup;
using Nop.Services.Tax;
using Nop.Web.Areas.Admin.Models.Affiliates;
using Nop.Web.Areas.Admin.Models.Blogs;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Areas.Admin.Models.Cms;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Models.Directory;
using Nop.Web.Areas.Admin.Models.Discounts;
using Nop.Web.Areas.Admin.Models.ExternalAuthentication;
using Nop.Web.Areas.Admin.Models.Forums;
using Nop.Web.Areas.Admin.Models.Localization;
using Nop.Web.Areas.Admin.Models.Logging;
using Nop.Web.Areas.Admin.Models.Messages;
using Nop.Web.Areas.Admin.Models.MultiFactorAuthentication;
using Nop.Web.Areas.Admin.Models.News;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Areas.Admin.Models.Payments;
using Nop.Web.Areas.Admin.Models.Plugins;
using Nop.Web.Areas.Admin.Models.Polls;
using Nop.Web.Areas.Admin.Models.Settings;
using Nop.Web.Areas.Admin.Models.Shipping;
using Nop.Web.Areas.Admin.Models.ShoppingCart;
using Nop.Web.Areas.Admin.Models.Stores;
using Nop.Web.Areas.Admin.Models.Tasks;
using Nop.Web.Areas.Admin.Models.Tax;
using Nop.Web.Areas.Admin.Models.Templates;
using Nop.Web.Areas.Admin.Models.Topics;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.WebOptimizer;
using Nop.Plugin.Widgets.CustomTest.Domain;
using Nop.Plugin.Widgets.CustomTest.Models;

namespace Nop.Plugin.Widgets.CustomTest.Infrastructure.Mapper;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public MapperConfiguration()
    {
        CreatePluginStudentMaps();

        //add some generic mapping rules
        this.Internal().ForAllMaps((mapConfiguration, map) =>
        {
            //exclude Form and CustomProperties from mapping BaseNopModel
            if (typeof(BaseNopModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                //map.ForMember(nameof(BaseNopModel.Form), options => options.Ignore());
                map.ForMember(nameof(BaseNopModel.CustomProperties), options => options.Ignore());
            }

            //exclude ActiveStoreScopeConfiguration from mapping ISettingsModel
            if (typeof(ISettingsModel).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(ISettingsModel.ActiveStoreScopeConfiguration), options => options.Ignore());

            //exclude some properties from mapping configuration and models
            if (typeof(IConfig).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(IConfig.Name), options => options.Ignore());

            //exclude Locales from mapping ILocalizedModel
            if (typeof(ILocalizedModel).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(ILocalizedModel<ILocalizedModel>.Locales), options => options.Ignore());

            //exclude some properties from mapping store mapping supported entities and models
            if (typeof(IStoreMappingSupported).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(IStoreMappingSupported.LimitedToStores), options => options.Ignore());
            if (typeof(IStoreMappingSupportedModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                map.ForMember(nameof(IStoreMappingSupportedModel.AvailableStores), options => options.Ignore());
                map.ForMember(nameof(IStoreMappingSupportedModel.SelectedStoreIds), options => options.Ignore());
            }

            //exclude some properties from mapping ACL supported entities and models
            if (typeof(IAclSupported).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(IAclSupported.SubjectToAcl), options => options.Ignore());
            if (typeof(IAclSupportedModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                map.ForMember(nameof(IAclSupportedModel.AvailableCustomerRoles), options => options.Ignore());
                map.ForMember(nameof(IAclSupportedModel.SelectedCustomerRoleIds), options => options.Ignore());
            }

            //exclude some properties from mapping discount supported entities and models
            if (typeof(IDiscountSupportedModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                map.ForMember(nameof(IDiscountSupportedModel.AvailableDiscounts), options => options.Ignore());
                map.ForMember(nameof(IDiscountSupportedModel.SelectedDiscountIds), options => options.Ignore());
            }

            if (typeof(IPluginModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                //exclude some properties from mapping plugin models
                map.ForMember(nameof(IPluginModel.ConfigurationUrl), options => options.Ignore());
                map.ForMember(nameof(IPluginModel.IsActive), options => options.Ignore());
                map.ForMember(nameof(IPluginModel.LogoUrl), options => options.Ignore());

                //define specific rules for mapping plugin models
                if (typeof(IPlugin).IsAssignableFrom(mapConfiguration.SourceType))
                {
                    map.ForMember(nameof(IPluginModel.DisplayOrder), options => options.MapFrom(plugin => ((IPlugin)plugin).PluginDescriptor.DisplayOrder));
                    map.ForMember(nameof(IPluginModel.FriendlyName), options => options.MapFrom(plugin => ((IPlugin)plugin).PluginDescriptor.FriendlyName));
                    map.ForMember(nameof(IPluginModel.SystemName), options => options.MapFrom(plugin => ((IPlugin)plugin).PluginDescriptor.SystemName));
                }
            }
        });
    }

    #endregion

    #region Properties
    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 0;

    protected virtual void CreatePluginStudentMaps()
    {
        CreateMap<StudentCreateModel, Student>();
        #endregion
    }
}