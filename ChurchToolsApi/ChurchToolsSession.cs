using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchToolsApi
{
    public sealed class ChurchToolsSession : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _applicationName;

        public ChurchToolsSession(Uri baseUri, string applicationName)
        {
            _client = new HttpClient()
            {
                BaseAddress = baseUri,
            };

            _applicationName = applicationName;
        }

        public Task<T> InvokeApiRequestAsync<T>(string modulePath, string functionName, object parameter)
            => InvokeApiRequestAsync<T>(modulePath, functionName, parameter, CancellationToken.None);

        public async Task<T> InvokeApiRequestAsync<T>(string modulePath, string functionName, object parameter, CancellationToken cancellationToken)
        {
            var responseObject = await InvokeApiRequestInternalAsync(modulePath, functionName, parameter, cancellationToken).ConfigureAwait(false);
            if (responseObject.Value<string>("status") == "success")
            {
                return responseObject["data"].ToObject<T>();
            }

            throw new RequestFailedException(responseObject);
        }

        public Task InvokeApiRequestAsync(string modulePath, string functionName, object parameter)
          => InvokeApiRequestAsync(modulePath, functionName, parameter, CancellationToken.None);

        public async Task InvokeApiRequestAsync(string modulePath, string functionName, object parameter, CancellationToken cancellationToken)
        {
            var responseObject = await InvokeApiRequestInternalAsync(modulePath, functionName, parameter, cancellationToken);

            if (responseObject.Value<string>("status") != "success")
                throw new RequestFailedException(responseObject);
        }

        private async Task<JObject> InvokeApiRequestInternalAsync(string modulePath, string functionName, object parameter, CancellationToken cancellationToken)
        {
            JObject requestBody = parameter != null ? JObject.FromObject(parameter) : new JObject();
            requestBody.Add("func", functionName);

            var content = new StringContent(requestBody.ToString(), Encoding.ASCII, "application/json");
            var response = await _client.PostAsync($"?q={modulePath}/ajax", content).ConfigureAwait(false);


            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unexpedted status code. Expected: {HttpStatusCode.OK}, Got: {response.StatusCode} ({response.ReasonPhrase}");
            }

            using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var responseStreamReader = new StreamReader(responseStream))
            using (var responseJsonReader = new JsonTextReader(responseStreamReader))
            {
                return await JObject.LoadAsync(responseJsonReader, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var loginParameters = new PasswordLoginParameter()
            {
                ApplicationName = _applicationName,
                Email = email,
                Password = password
            };

            await InvokeApiRequestAsync("login", "login", loginParameters, cancellationToken).ConfigureAwait(false);
        }

        public Task LoginAsync(int id, string token) => LoginAsync(id, token, CancellationToken.None);

        public async Task LoginAsync(int id, string token, CancellationToken cancellationToken)
        {
            var loginParameters = new TokenLoginParameter()
            {
                ApplicationName = _applicationName,
                Id = id,
                Token = token,
            };

            await InvokeApiRequestAsync("login", "loginWithToken", loginParameters, cancellationToken).ConfigureAwait(false);

        }

        public Task<(int Id, string Token)> GetUserLoginToken(string email, string password)
            => GetUserLoginToken(email, password, CancellationToken.None);

        public async Task<(int Id, string Token)> GetUserLoginToken(string email, string password, CancellationToken cancellation)
        {
            var loginParameters = new PasswordLoginParameter()
            {
                ApplicationName = _applicationName,
                Email = email,
                Password = password
            };

            var result = await InvokeApiRequestAsync<UserToken>("login", "getUserLoginToken", loginParameters, cancellation).ConfigureAwait(false);

            return (result.Id, result.Token);
        }

        public Task LogoutAsync()
        {
            return LogoutAsync(CancellationToken.None);
        }

        public Task LogoutAsync(CancellationToken cancellationToken)
        {
            return InvokeApiRequestAsync("login", "logout", null, cancellationToken);
        }    

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        async void Dispose(bool disposing)
        {
            if (disposedValue || !disposing)
                return;

            await LogoutAsync().ConfigureAwait(false);
            _client.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
