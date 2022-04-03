

using Microsoft.AspNetCore.SignalR;

namespace bagit_api.Services;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
    }
    
}