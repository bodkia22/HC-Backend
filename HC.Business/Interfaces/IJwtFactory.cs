using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HC.Business.Interfaces
{
    public interface IJwtFactory
    {
        public string GenerateEncodedToken(List<Claim> claims);
    }
}
