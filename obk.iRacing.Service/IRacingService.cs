using Aydsko.iRacingData;
using Aydsko.iRacingData.Common;
using Aydsko.iRacingData.Member;
using obk.iRacing.Service.Interfaces;

namespace obk.iRacing.Service;

public class IRacingService : IIRacingService
{
    private readonly IDataClient _dataClient;
    
    public IRacingService(IDataClient dataClient)
    {
        _dataClient = dataClient;
    }
    
    public async Task<bool> Login(string email, string password, int customerId)
    {
        _dataClient.UseUsernameAndPassword(email, password);
        var dataResponse = await _dataClient.GetMemberProfileAsync(customerId).ConfigureAwait(false);
        if (dataResponse.Data.CustomerId == customerId)
            return true;
        return false;
    }

    public async Task<DataResponse<MemberProfile>> GetMemberProfileAsync()
    {
        var result =  await _dataClient.GetMemberProfileAsync().ConfigureAwait(false);
        
        return result;
    }
}