using AutoMapper;
using CRM_Vendas_API.Entities.DTOs.CustomerDto;
using CRM_Vendas_API.Entities.DTOs.DealDto;
using CRM_Vendas_API.Entities.DTOs.InteractionDto;
using CRM_Vendas_API.Entities.DTOs.LeadDto;
using CRM_Vendas_API.Entities.DTOs.UserDto;
using CRM_Vendas_API.Entities.Models;

namespace CRM_Vendas_API.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            // Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();

            // Lead
            CreateMap<Lead, LeadDto>().ReverseMap();
            CreateMap<LeadCreateDto, Lead>();
            CreateMap<LeadUpdateDto, Lead>();

            // Deal
            CreateMap<Deal, DealDto>().ReverseMap();
            CreateMap<DealCreateDto, Deal>();
            CreateMap<DealUpdateDto, Deal>();

            // Interaction
            CreateMap<Interaction, InteractionDto>().ReverseMap();
            CreateMap<InteractionCreateDto, Interaction>();
            CreateMap<InteractionUpdateDto, Interaction>();
        }
    }
}
