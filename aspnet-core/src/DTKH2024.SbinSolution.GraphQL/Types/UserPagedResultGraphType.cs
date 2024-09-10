using Abp.Application.Services.Dto;
using GraphQL.Types;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Types
{
    public class UserPagedResultGraphType : ObjectGraphType<PagedResultDto<UserDto>>
    {
        public UserPagedResultGraphType()
        {
            Name = "UserPagedResultGraphType";
            
            Field(x => x.TotalCount);
            Field(x => x.Items, type: typeof(ListGraphType<UserType>));
        }
    }
}
