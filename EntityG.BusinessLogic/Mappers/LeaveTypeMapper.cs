using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class LeaveTypeMapper
    {
        public static LookupDto ToLookup(LeaveType leaveType)
        {
            return new LookupDto
            {
                Id = leaveType.Id.ToString(),
                Value = leaveType.Name
            };
        }
    }
}
