using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Zoho
{
    /// <summary>
    /// Zoho base class to verify the communication and get the access token from hard coded
    /// grant token and refresh that with the refresh token
    /// </summary>
    public class ZohoAccessManager
    {
        /// <summary>
        /// Self Client Grant Key
        /// </summary>
        private string _GrantKey;
        private string _ClientID;
        private string _ClientSecret;
        private string _RedirectUri;

        private string _RefreshToken;

        public ZohoAccessManager(string grantKey, string clientId, string clientSecret, string redirectUri, string refreshToken)
        {
            _GrantKey = grantKey;
            _ClientID = clientId;
            _ClientSecret = clientSecret;
            _RedirectUri = redirectUri;

            _RefreshToken = refreshToken;
        }

        /// <summary>
        /// Getting the access token from the grant token once
        /// </summary>
        public void GetAccessToken()
        {
            var client = new RestClient("https://accounts.zoho.com/oauth/v2/token");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_id", _ClientID);
            request.AddParameter("client_secret", _ClientSecret);
            request.AddParameter("redirect_uri", _RedirectUri);
            request.AddParameter("code", _GrantKey);


            var restResponse = client.Execute(request);
            var content = (JObject)JsonConvert.DeserializeObject(restResponse.Content);

            var accessToken = (string)content["access_token"];
            var refreshToken = (string)content["refresh_token"];

            //TODO: Saving in configuration file
        }

        /// <summary>
        /// Get new access token in case of access token expiration submitting the refresh token 
        /// </summary>
        public string GetNewToken()
        {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("client_id", _ClientID);
            request.AddParameter("client_secret", _ClientSecret);
            request.AddParameter("refresh_token", _RefreshToken);

            var client = new RestClient("https://accounts.zoho.com/oauth/v2/token");
            var response = client.Execute(request);
            var content = (JObject)JsonConvert.DeserializeObject(response.Content);
            var accesstoken = (string)content["access_token"];

            return accesstoken;
        }
    }
}
