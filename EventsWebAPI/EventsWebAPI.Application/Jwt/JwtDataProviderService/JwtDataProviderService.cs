using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace EventsWebAPI.Application.Jwt.JwtDataProviderService
{
    public class JwtDataProviderService
    {
        public Guid GetUserIDFromToken(KeyValuePair<string, StringValues> headerData)
        {
            var token = headerData.Value.ToString()["Bearer ".Length..];
            var handler = new JwtSecurityTokenHandler();
            var stringUserID = handler.ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type == "ID").Value;
            var UserID = Guid.Parse(stringUserID);
            return UserID;
        }
        public string GetUserRoleFromToken(KeyValuePair<string, StringValues> headerData)
        {
            var token = headerData.Value.ToString()["Bearer ".Length..];
            var handler = new JwtSecurityTokenHandler();
            var UserRole = handler.ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type == "Role").Value;
            return UserRole;
        }
    }
}
