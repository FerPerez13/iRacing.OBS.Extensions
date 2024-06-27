using Aydsko.iRacingData.Common;
using Aydsko.iRacingData.Member;

namespace obk.iRacing.Service.Interfaces;

public interface IIRacingService
{
    Task<bool> Login(string email, string password, int customerId);
    
    Task<DataResponse<MemberProfile>> GetMemberProfileAsync();
}