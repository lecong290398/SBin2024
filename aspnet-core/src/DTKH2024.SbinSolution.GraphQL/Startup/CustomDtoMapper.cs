using AutoMapper;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Startup
{
    public static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Roles, options => options.Ignore())
                .ForMember(dto => dto.OrganizationUnits, options => options.Ignore());
        }
    }
}