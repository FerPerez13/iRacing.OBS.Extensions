using Aydsko.iRacingData.Common;
using Aydsko.iRacingData.Member;
using Aydsko.iRacingData.Stats;

namespace obk.iRacing.Service.Interfaces;

public interface IIRacingService
{
    Task<bool> Login(string email, string password, int customerId);
    
    Task<DataResponse<MemberProfile>> GetMemberProfileAsync();
    
    Task<DataResponse<DriverInfo[]>> GetDriverInfoAsync(int driverId);
    
    Task<DataResponse<MemberRecentRaces>> GetMemberRecentRacesAsync();
    
}