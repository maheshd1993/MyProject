using AutoMapper;
using Svam.EF;
using Svam.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Traders.Models;

namespace Svam.AutoMapperConfig 
{
    public  class AutomapperConfig
    {
        public static void MapIt()
        {
            Mapper.CreateMap<crm_roleassigntbl, CreatRoleModel>();
            Mapper.CreateMap<CreatRoleModel, crm_roleassigntbl>();

            Mapper.CreateMap<crm_createleadsetting, CreateLeadSettingDTO>();
            Mapper.CreateMap<CreateLeadSettingDTO, crm_createleadsetting>();

            Mapper.CreateMap<crm_viewleadsetting, ViewLeadSettingDTO>();
            Mapper.CreateMap<ViewLeadSettingDTO, crm_viewleadsetting>();

            Mapper.CreateMap<crm_ticketcreatesetting, CreateTicketSettingDTO>();
            Mapper.CreateMap<CreateTicketSettingDTO, crm_ticketcreatesetting>();

            Mapper.CreateMap<crm_ticketviewsetting, ViewTecketSettingDTO>();
            Mapper.CreateMap<ViewTecketSettingDTO, crm_ticketviewsetting>();
        }

      

    }
}