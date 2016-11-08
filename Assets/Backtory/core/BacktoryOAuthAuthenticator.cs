using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Assets.Backtory.core
{
    internal class BacktoryOAuthAuthenticator : IAuthenticator
    {
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
