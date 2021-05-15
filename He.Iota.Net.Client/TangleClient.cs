namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Http;

    public partial class TangleClient 
    {
        private string baseUrl = "http://127.0.0.1:14265";
        private readonly HttpClient HttpClient;
        private readonly Lazy<JsonSerializerSettings> Settings;

        public TangleClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            Settings = new System.Lazy<JsonSerializerSettings>(CreateSerializerSettings);
        }
    
        private JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }
    
        public string BaseUrl
        {
            get { return baseUrl; }
            set { baseUrl = value; }
        }
    
        protected JsonSerializerSettings JsonSerializerSettings { get { return Settings.Value; } }
    
        partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings);
    
    
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url);
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder);
        partial void ProcessResponse(HttpClient client, HttpResponseMessage response);
        /// <summary>Returns the health of the node.</summary>
        /// <returns>Successful operation: indicates that the node is healthy.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task HealthAsync()
        {
            return HealthAsync(System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns the health of the node.</summary>
        /// <returns>Successful operation: indicates that the node is healthy.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task HealthAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/health");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            return;
                        }
                        else
                        if (status == 403)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, responseText, headers, null);
                        }
                        else
                        if (status == 500)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, responseText, headers, null);
                        }
                        else
                        if (status == 503)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the node isn\u00b4t healthy.", status, responseText, headers, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns general information about the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<InfoResponse> InfoAsync()
        {
            return InfoAsync(System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns general information about the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<InfoResponse> InfoAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/info");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<InfoResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns tips that are ideal for attaching a message.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<TipsResponse> TipsAsync()
        {
            return TipsAsync(System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns tips that are ideal for attaching a message.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<TipsResponse> TipsAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/tips");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<TipsResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that there are no tips available or the node isn\u00b4t synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }

        public async Task<SubmitMessageResponse> SubmitMessage(SubmitMessageRequest submitMessageRequest)
        {
            if (submitMessageRequest is null)
            {
                throw new ArgumentNullException("body");
            }

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages");

            submitMessageRequest.Payload.Index = this.StringToHex(submitMessageRequest.Payload.Index);
            submitMessageRequest.Payload.Data = this.StringToHex(submitMessageRequest.Payload.Data);

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(submitMessageRequest);

            // Wrap our JSON inside a StringContent which then can be used by the this.HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var uri = urlBuilder.ToString();

            var response = await this.HttpClient.PostAsync(uri, httpContent);
            var responseText = await response.Content.ReadAsStringAsync();

            try
            {
                var typedBody = JsonConvert.DeserializeObject<SubmitMessageResponse>(responseText, JsonSerializerSettings);
                return typedBody;
            }
            catch (JsonException exception)
            {
                throw;
            }
        }

        private string StringToHex(string source)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.UTF8.GetBytes(source);

            foreach (var t in bytes)
            {
                sb.Append(t.ToString("x"));
            }

            return sb.ToString();
        }
    
        /// <summary>Submit a message.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<SubmitMessageResponse> MessagesAsync(SubmitMessageRequest body)
        {
            return MessagesAsync(body, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Submit a message.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<SubmitMessageResponse> MessagesAsync(SubmitMessageRequest body, System.Threading.CancellationToken cancellationToken)
        {
            if (body == null)
                throw new ArgumentNullException("body");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(body, Settings.Value));
                    content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request.Content = content;
                    request.Method = new HttpMethod("POST");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 201)
                        {
                            var objectResponse = await ReadObjectResponseAsync<SubmitMessageResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node can\u00b4t auto-fill the parents or perform Proof-of-Work.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Search for messages matching a given indexation key.</summary>
        /// <param name="index">Hex-encoded indexation key that should be searched for.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<MessagesFindResponse> Messages2Async(string index)
        {
            return Messages2Async(index, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Search for messages matching a given indexation key.</summary>
        /// <param name="index">Hex-encoded indexation key that should be searched for.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessagesFindResponse> Messages2Async(string index, System.Threading.CancellationToken cancellationToken)
        {
            if (index == null)
                throw new ArgumentNullException("index");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages?");
            urlBuilder.Append(System.Uri.EscapeDataString("index") + "=").Append(System.Uri.EscapeDataString(ConvertToString(index, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<MessagesFindResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns message data as JSON by its identifier.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<MessageResponse> Messages3Async(string messageId)
        {
            return Messages3Async(messageId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns message data as JSON by its identifier.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageResponse> Messages3Async(string messageId, System.Threading.CancellationToken cancellationToken)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}");
            urlBuilder.Replace("{messageId}", System.Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<MessageResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Find the metadata of a given message.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<MessageMetadataResponse> MetadataAsync(string messageId)
        {
            return MetadataAsync(messageId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Find the metadata of a given message.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageMetadataResponse> MetadataAsync(string messageId, System.Threading.CancellationToken cancellationToken)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}/metadata");
            urlBuilder.Replace("{messageId}", System.Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<MessageMetadataResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns message raw bytes by its identifier.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<FileResponse> RawAsync(string messageId)
        {
            return RawAsync(messageId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns message raw bytes by its identifier.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<FileResponse> RawAsync(string messageId, System.Threading.CancellationToken cancellationToken)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}/raw");
            urlBuilder.Replace("{messageId}", System.Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/octet-stream"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200 || status == 206)
                        {
                            var responseStream = response.Content == null ? System.IO.Stream.Null : await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            var fileResponse = new FileResponse(status, headers, responseStream, null, response); 
                            disposeClient = false; disposeResponse = false; // response and client are disposed by FileResponse
                            return fileResponse;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns the children of a message.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<MessageChildrenResponse> ChildrenAsync(string messageId)
        {
            return ChildrenAsync(messageId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns the children of a message.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageChildrenResponse> ChildrenAsync(string messageId, System.Threading.CancellationToken cancellationToken)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}/children");
            urlBuilder.Replace("{messageId}", System.Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<MessageChildrenResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Find an output by its identifier.</summary>
        /// <param name="outputId">Identifier of the output encoded in hex. An output is identified by the concatenation of `transactionid+outputindex`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<OutputResponse> OutputsAsync(string outputId)
        {
            return OutputsAsync(outputId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Find an output by its identifier.</summary>
        /// <param name="outputId">Identifier of the output encoded in hex. An output is identified by the concatenation of `transactionid+outputindex`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<OutputResponse> OutputsAsync(string outputId, System.Threading.CancellationToken cancellationToken)
        {
            if (outputId == null)
                throw new ArgumentNullException("outputId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/outputs/{outputId}");
            urlBuilder.Replace("{outputId}", System.Uri.EscapeDataString(ConvertToString(outputId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<OutputResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get the balance of a bech32-encoded address.</summary>
        /// <param name="address">bech32 encoded address</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<BalanceAddressResponse> AddressesAsync(string address)
        {
            return AddressesAsync(address, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get the balance of a bech32-encoded address.</summary>
        /// <param name="address">bech32 encoded address</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<BalanceAddressResponse> AddressesAsync(string address, System.Threading.CancellationToken cancellationToken)
        {
            if (address == null)
                throw new ArgumentNullException("address");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/{address}");
            urlBuilder.Replace("{address}", System.Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<BalanceAddressResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get the balance of a hex-encoded Ed25519 address.</summary>
        /// <param name="address">hex-encoded Ed25519 address</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<BalanceAddressResponse> Ed25519Async(string address)
        {
            return Ed25519Async(address, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get the balance of a hex-encoded Ed25519 address.</summary>
        /// <param name="address">hex-encoded Ed25519 address</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<BalanceAddressResponse> Ed25519Async(string address, System.Threading.CancellationToken cancellationToken)
        {
            if (address == null)
                throw new ArgumentNullException("address");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/ed25519/{address}");
            urlBuilder.Replace("{address}", System.Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<BalanceAddressResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get all outputs that use a given bech32-encoded address.</summary>
        /// <param name="address">bech32-encoded address that is referenced by the outputs.</param>
        /// <param name="includespent">Set to true to also include the known spent outputs for the given address.</param>
        /// <param name="type">Allows to filter the results by output type. Set to value `0` to filter outputs of type `SigLockedSingleOutput`. Set to value `1` to filter outputs of type `SigLockedDustAllowanceOutput`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<OutputsAddressResponse> Outputs2Async(string address, bool? includespent, int? type)
        {
            return Outputs2Async(address, includespent, type, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get all outputs that use a given bech32-encoded address.</summary>
        /// <param name="address">bech32-encoded address that is referenced by the outputs.</param>
        /// <param name="includespent">Set to true to also include the known spent outputs for the given address.</param>
        /// <param name="type">Allows to filter the results by output type. Set to value `0` to filter outputs of type `SigLockedSingleOutput`. Set to value `1` to filter outputs of type `SigLockedDustAllowanceOutput`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<OutputsAddressResponse> Outputs2Async(string address, bool? includespent, int? type, System.Threading.CancellationToken cancellationToken)
        {
            if (address == null)
                throw new ArgumentNullException("address");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/{address}/outputs?");
            urlBuilder.Replace("{address}", System.Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));
            if (includespent != null)
            {
                urlBuilder.Append(System.Uri.EscapeDataString("include-spent") + "=").Append(System.Uri.EscapeDataString(ConvertToString(includespent, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (type != null)
            {
                urlBuilder.Append(System.Uri.EscapeDataString("type") + "=").Append(System.Uri.EscapeDataString(ConvertToString(type, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<OutputsAddressResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get all outputs that use a given hex-encoded Ed25519 address.</summary>
        /// <param name="address">hex-encoded Ed25519 address that is referenced by the outputs.</param>
        /// <param name="includespent">Set to true to also include the known spent outputs for the given address.</param>
        /// <param name="type">Allows to filter the results by output type. Set to value `0` to filter outputs of type `SigLockedSingleOutput`. Set to value `1` to filter outputs of type `SigLockedDustAllowanceOutput`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<OutputsAddressResponse> Outputs3Async(string address, bool? includespent, int? type)
        {
            return Outputs3Async(address, includespent, type, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get all outputs that use a given hex-encoded Ed25519 address.</summary>
        /// <param name="address">hex-encoded Ed25519 address that is referenced by the outputs.</param>
        /// <param name="includespent">Set to true to also include the known spent outputs for the given address.</param>
        /// <param name="type">Allows to filter the results by output type. Set to value `0` to filter outputs of type `SigLockedSingleOutput`. Set to value `1` to filter outputs of type `SigLockedDustAllowanceOutput`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<OutputsAddressResponse> Outputs3Async(string address, bool? includespent, int? type, System.Threading.CancellationToken cancellationToken)
        {
            if (address == null)
                throw new ArgumentNullException("address");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/ed25519/{address}/outputs?");
            urlBuilder.Replace("{address}", System.Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));
            if (includespent != null)
            {
                urlBuilder.Append(System.Uri.EscapeDataString("include-spent") + "=").Append(System.Uri.EscapeDataString(ConvertToString(includespent, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (type != null)
            {
                urlBuilder.Append(System.Uri.EscapeDataString("type") + "=").Append(System.Uri.EscapeDataString(ConvertToString(type, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<OutputsAddressResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns all stored receipts.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ReceiptsResponse> ReceiptsAsync()
        {
            return ReceiptsAsync(System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns all stored receipts.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ReceiptsResponse> ReceiptsAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/receipts");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ReceiptsResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns all stored receipts for a given migration index.</summary>
        /// <param name="migratedAt">Migration index to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ReceiptsResponse> Receipts2Async(double migratedAt)
        {
            return Receipts2Async(migratedAt, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns all stored receipts for a given migration index.</summary>
        /// <param name="migratedAt">Migration index to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ReceiptsResponse> Receipts2Async(double migratedAt, System.Threading.CancellationToken cancellationToken)
        {
            if (migratedAt == null)
                throw new ArgumentNullException("migratedAt");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/receipts/{migratedAt}");
            urlBuilder.Replace("{migratedAt}", System.Uri.EscapeDataString(ConvertToString(migratedAt, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ReceiptsResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns information about the treasury.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<TreasuryResponse> TreasuryAsync()
        {
            return TreasuryAsync(System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns information about the treasury.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<TreasuryResponse> TreasuryAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/treasury");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<TreasuryResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 503)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the node is not synced.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Returns the included message of a transaction.</summary>
        /// <param name="transactionId">Identifier of the transaction to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<MessageResponse> IncludedMessageAsync(string transactionId)
        {
            return IncludedMessageAsync(transactionId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns the included message of a transaction.</summary>
        /// <param name="transactionId">Identifier of the transaction to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageResponse> IncludedMessageAsync(string transactionId, System.Threading.CancellationToken cancellationToken)
        {
            if (transactionId == null)
                throw new ArgumentNullException("transactionId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/transactions/{transactionId}/included-message");
            urlBuilder.Replace("{transactionId}", System.Uri.EscapeDataString(ConvertToString(transactionId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<MessageResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Look up a milestone by a given milestone index.</summary>
        /// <param name="index">Index of the milestone to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<MilestoneResponse> MilestonesAsync(double index)
        {
            return MilestonesAsync(index, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Look up a milestone by a given milestone index.</summary>
        /// <param name="index">Index of the milestone to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MilestoneResponse> MilestonesAsync(double index, System.Threading.CancellationToken cancellationToken)
        {
            if (index == null)
                throw new ArgumentNullException("index");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/milestones/{index}");
            urlBuilder.Replace("{index}", System.Uri.EscapeDataString(ConvertToString(index, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<MilestoneResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get all UTXO changes of a given milestone.</summary>
        /// <param name="index">Index of the milestone to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<UTXOChangesResponse> UtxoChangesAsync(double index)
        {
            return UtxoChangesAsync(index, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get all UTXO changes of a given milestone.</summary>
        /// <param name="index">Index of the milestone to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<UTXOChangesResponse> UtxoChangesAsync(double index, System.Threading.CancellationToken cancellationToken)
        {
            if (index == null)
                throw new ArgumentNullException("index");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/milestones/{index}/utxo-changes");
            urlBuilder.Replace("{index}", System.Uri.EscapeDataString(ConvertToString(index, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<UTXOChangesResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get information about the peers of the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<PeersResponse> PeersAsync()
        {
            return PeersAsync(System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get information about the peers of the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<PeersResponse> PeersAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/peers");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<PeersResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Add a given peer to the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<AddPeerResponse> Peers2Async(AddPeerRequest body)
        {
            return Peers2Async(body, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Add a given peer to the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<AddPeerResponse> Peers2Async(AddPeerRequest body, System.Threading.CancellationToken cancellationToken)
        {
            if (body == null)
                throw new ArgumentNullException("body");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/peers");
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(body, Settings.Value));
                    content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request.Content = content;
                    request.Method = new HttpMethod("POST");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<AddPeerResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Get information about a given peer.</summary>
        /// <param name="peerId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<PeerResponse> Peers3Async(string peerId)
        {
            return Peers3Async(peerId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get information about a given peer.</summary>
        /// <param name="peerId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<PeerResponse> Peers3Async(string peerId, System.Threading.CancellationToken cancellationToken)
        {
            if (peerId == null)
                throw new ArgumentNullException("peerId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/peers/{peerId}");
            urlBuilder.Replace("{peerId}", System.Uri.EscapeDataString(ConvertToString(peerId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            var objectResponse = await ReadObjectResponseAsync<PeerResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            return objectResponse.Object;
                        }
                        else
                        if (status == 400)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the provided data is invalid.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 403)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 404)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that the requested data was not found.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        if (status == 500)
                        {
                            var objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                            if (objectResponse.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                            }
                            throw new ApiException<ErrorResponse>("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, objectResponse.Text, headers, objectResponse.Object, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        /// <summary>Remove/disconnect a given peer.</summary>
        /// <param name="peerId">Identifier of the peer.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task Peers4Async(string peerId)
        {
            return Peers4Async(peerId, System.Threading.CancellationToken.None);
        }
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Remove/disconnect a given peer.</summary>
        /// <param name="peerId">Identifier of the peer.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task Peers4Async(string peerId, System.Threading.CancellationToken cancellationToken)
        {
            if (peerId == null)
                throw new ArgumentNullException("peerId");
    
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/peers/{peerId}");
            urlBuilder.Replace("{peerId}", System.Uri.EscapeDataString(ConvertToString(peerId, System.Globalization.CultureInfo.InvariantCulture)));
    
            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("DELETE");
    
                    PrepareRequest(client, request, urlBuilder);
    
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client, request, url);
    
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse = true;
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }
    
                        ProcessResponse(client, response);
    
                        var status = (int)response.StatusCode;
                        if (status == 204)
                        {
                            return;
                        }
                        else
                        if (status == 400)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the provided data is invalid.", status, responseText, headers, null);
                        }
                        else
                        if (status == 403)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, responseText, headers, null);
                        }
                        else
                        if (status == 404)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the requested data was not found.", status, responseText, headers, null);
                        }
                        else
                        if (status == 500)
                        {
                            string responseText = ( response.Content == null ) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, responseText, headers, null);
                        }
                        else
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse)
                            response.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }
        }
    
        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }
    
            public T Object { get; }
    
            public string Text { get; }
        }
    
        public bool ReadResponseAsString { get; set; }
        
        protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Threading.CancellationToken cancellationToken)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }
        
            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serializer = JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }
    
        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "";
            }
        
            if (value is System.Enum)
            {
                var name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute)) 
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }
        
                    var converted = System.Convert.ToString(System.Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    return converted == null ? string.Empty : converted;
                }
            }
            else if (value is bool) 
            {
                return System.Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[]) value);
            }
            else if (value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array) value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }
        
            var result = System.Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
        }
    }

    /// <summary>A message is the object nodes gossip around in the network. It always references two other messages that are known as parents. It is stored as a vertex on the tangle data structure that the nodes maintain. A message can have a maximum size of 32Kb.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Message 
    {
        /// <summary>Network identifier. This field signifies for which network the message is meant for. It also tells which protocol rules apply to the message. It is computed out of the first 8 bytes of the `BLAKE2b-256` hash of the concatenation of the network type and protocol version string.</summary>
        [JsonProperty("networkId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string NetworkId { get; set; }
    
        /// <summary>The identifiers of the messages this message references.</summary>
        [JsonProperty("parentMessageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ParentMessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        [JsonProperty("payload", Required = Required.Always)]
        public TransactionPayload Payload { get; set; }
    
        /// <summary>The nonce which lets this message fulfill the Proof-of-Work requirement.</summary>
        [JsonProperty("nonce", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Nonce { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The Transaction Payload to be embedded into a message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TransactionPayload 
    {
        /// <summary>Set to value 0 to denote a Transaction Payload.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("essence", Required = Required.Always)]
        public TransactionEssence Essence { get; set; } = new TransactionEssence();
    
        [JsonProperty("unlockBlocks", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<SignatureUnlockBlock> UnlockBlocks { get; set; } = new System.Collections.ObjectModel.Collection<SignatureUnlockBlock>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Describes the essence data making up a transaction by defining its inputs and outputs and an optional payload.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TransactionEssence 
    {
        /// <summary>Set to value 0 to denote a Transaction Essence.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("inputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<UTXOInput> Inputs { get; set; } = new System.Collections.ObjectModel.Collection<UTXOInput>();
    
        [JsonProperty("outputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<Outputs> Outputs { get; set; } = new System.Collections.ObjectModel.Collection<Outputs>();
    
        [JsonProperty("payload", Required = Required.AllowNull)]
        public IndexationPayload Payload { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Describes an input which references an unspent transaction output to consume.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class UTXOInput 
    {
        /// <summary>Set to value 0 to denote an UTXO Input.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        /// <summary>The BLAKE2b-256 hash of the transaction from which the UTXO comes from.</summary>
        [JsonProperty("transactionId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string TransactionId { get; set; }
    
        /// <summary>The index of the output on the referenced transaction to consume.</summary>
        [JsonProperty("transactionOutputIndex", Required = Required.Always)]
        public int TransactionOutputIndex { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Describes a deposit to a single address which is unlocked via a signature.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SigLockedSingleOutput 
    {
        /// <summary>Set to value 0 to denote a SigLockedSingleOutput.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("address", Required = Required.Always)]
        public Ed25519Address Address { get; set; }
    
        /// <summary>The amount of tokens to deposit with this SigLockedSingleOutput output.</summary>
        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Output type for deposits that enables an address to receive dust outputs. It can be consumed as an input like a regular SigLockedSingleOutput</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SigLockedDustAllowanceOutput 
    {
        /// <summary>Set to value 1 to denote a SigLockedDustAllowanceOutput.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("address", Required = Required.Always)]
        public Ed25519Address Address { get; set; }
    
        /// <summary>The amount of tokens to deposit with this SigLockedDustAllowanceOutput output.</summary>
        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The Ed25519 address.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Ed25519Address 
    {
        /// <summary>Set to value 0 to denote an Ed25519 Address.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        /// <summary>The hex-encoded BLAKE2b-256 hash of the Ed25519 public key.</summary>
        [JsonProperty("address", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Address { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Defines an unlock block containing signature(s) unlocking input(s).</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SignatureUnlockBlock 
    {
        /// <summary>Denotes a Signature Unlock Block.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("signature", Required = Required.Always)]
        public Ed25519Signature Signature { get; set; } = new Ed25519Signature();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The Ed25519 signature.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Ed25519Signature 
    {
        /// <summary>Set to value 0 to denote an Ed25519 Signature.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        /// <summary>The public key of the Ed25519 keypair which is used to verify the signature.</summary>
        [JsonProperty("publicKey", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string PublicKey { get; set; }
    
        /// <summary>The signature signing the serialized Transaction Essence.</summary>
        [JsonProperty("signature", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Signature { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>References a previous unlock block in order to substitute the duplication of the same unlock block data for inputs which unlock through the same data.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReferenceUnlockBlock 
    {
        /// <summary>Set to value 1 to denote a Reference Unlock Block.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        /// <summary>Represents the index of a previous unlock block.</summary>
        [JsonProperty("reference", Required = Required.Always)]
        public int Reference { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The Milestone Payload to be embedded into a message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MilestonePayload 
    {
        /// <summary>Set to value 1 to denote a Milestone Payload.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        /// <summary>The index of the milestone.</summary>
        [JsonProperty("index", Required = Required.Always)]
        public int Index { get; set; }
    
        /// <summary>The Unix timestamp at which the milestone was issued. The unix timestamp is specified in seconds.</summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        public int Timestamp { get; set; }
    
        /// <summary>The identifiers of the messages this milestone  references.</summary>
        [JsonProperty("parents", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> Parents { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        /// <summary>256-bit hash based on the message IDs of all the not-ignored state-mutating transactions referenced by the milestone.</summary>
        [JsonProperty("inclusionMerkleProof", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string InclusionMerkleProof { get; set; }
    
        [JsonProperty("nextPoWScore", Required = Required.Always)]
        public double NextPoWScore { get; set; }
    
        [JsonProperty("nextPoWScoreMilestoneIndex", Required = Required.Always)]
        public double NextPoWScoreMilestoneIndex { get; set; }
    
        /// <summary>An array of public keys to validate the signatures. The keys must be in lexicographical order.</summary>
        [JsonProperty("publicKeys", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> PublicKeys { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        /// <summary>An array of signatures signing the serialized Milestone Essence. The signatures must be in the same order as the specified public keys.</summary>
        [JsonProperty("signatures", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> Signatures { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The Indexation Payload to be embedded into a message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class IndexationPayload 
    {
        /// <summary>Set to value 2 to denote a Indexation Payload.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        /// <summary>The indexation key to find/look up this message. It has a size between 1 and 64 bytes and must be encoded as a hex-string.</summary>
        [JsonProperty("index", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Index { get; set; }
    
        /// <summary>The optional data to attach. This may have a length of 0.</summary>
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Data { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TreasuryTransactionPayload 
    {
        /// <summary>Set to value 4 to denote a Treasury Payload.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("input", Required = Required.Always)]
        public TreasuryInput Input { get; set; }
    
        [JsonProperty("output", Required = Required.Always)]
        public TreasuryOutput Output { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TreasuryInput 
    {
        /// <summary>Set to value 1 to denote a TreasuryInput.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("milestoneId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MilestoneId { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TreasuryOutput 
    {
        /// <summary>Set to value 2 to denote a TreasuryOutput.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }
    
        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The peer of a node.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Peer 
    {
        /// <summary>The identifier of the peer.</summary>
        [JsonProperty("id", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Id { get; set; }
    
        /// <summary>The addresses of the peer.</summary>
        [JsonProperty("multiAddresses", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> MultiAddresses { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        /// <summary>The alias of the peer.</summary>
        [JsonProperty("alias", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }
    
        [JsonProperty("relation", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PeerRelation Relation { get; set; }
    
        /// <summary>Tells whether the peer is connected or not.</summary>
        [JsonProperty("connected", Required = Required.Always)]
        public bool Connected { get; set; }
    
        [JsonProperty("gossip", Required = Required.Always)]
        public Gossip Gossip { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Information about the gossip stream with the peer.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Gossip 
    {
        /// <summary>Information about the most recent heartbeat of the peer. The heartbeat is `null` if none has been received yet.</summary>
        [JsonProperty("heartbeat", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Heartbeat Heartbeat { get; set; }
    
        /// <summary>Metrics about the gossip stream with the peer.</summary>
        [JsonProperty("metrics", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Metrics Metrics { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Heartbeat 
    {
        /// <summary>The most recent milestone that has been solidified by the node.</summary>
        [JsonProperty("solidMilestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int SolidMilestoneIndex { get; set; }
    
        /// <summary>Tells from which starting point the node holds data.</summary>
        [JsonProperty("prunedMilestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int PrunedMilestoneIndex { get; set; }
    
        /// <summary>The most recent milestone known to the node.</summary>
        [JsonProperty("latestMilestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int LatestMilestoneIndex { get; set; }
    
        /// <summary>Tells how many connected peers the node has.</summary>
        [JsonProperty("connectedNeighbors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int ConnectedNeighbors { get; set; }
    
        /// <summary>Tells how many synced peers the node has.</summary>
        [JsonProperty("syncedNeighbors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int SyncedNeighbors { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Metrics 
    {
        /// <summary>The number of received messages that were new for the node.</summary>
        [JsonProperty("newMessages", Required = Required.Always)]
        public int NewMessages { get; set; }
    
        /// <summary>The number of received messages that already were known to the node.</summary>
        [JsonProperty("knownMessages", Required = Required.Always)]
        public int KnownMessages { get; set; }
    
        /// <summary>The number of received messages from the peer.</summary>
        [JsonProperty("receivedMessages", Required = Required.Always)]
        public int ReceivedMessages { get; set; }
    
        /// <summary>The number of received message requests from the peer.</summary>
        [JsonProperty("receivedMessageRequests", Required = Required.Always)]
        public int ReceivedMessageRequests { get; set; }
    
        /// <summary>The number of received milestone requests from the peer.</summary>
        [JsonProperty("receivedMilestoneRequests", Required = Required.Always)]
        public int ReceivedMilestoneRequests { get; set; }
    
        /// <summary>The number of received heartbeats from the peer.</summary>
        [JsonProperty("receivedHeartbeats", Required = Required.Always)]
        public int ReceivedHeartbeats { get; set; }
    
        /// <summary>The number of sent messages to the peer.</summary>
        [JsonProperty("sentMessages", Required = Required.Always)]
        public int SentMessages { get; set; }
    
        /// <summary>The number of sent message requests to the peer.</summary>
        [JsonProperty("sentMessageRequests", Required = Required.Always)]
        public int SentMessageRequests { get; set; }
    
        /// <summary>The number of sent milestone requests to the peer.</summary>
        [JsonProperty("sentMilestoneRequests", Required = Required.Always)]
        public int SentMilestoneRequests { get; set; }
    
        /// <summary>The number of sent heartbeats to the peer.</summary>
        [JsonProperty("sentHeartbeats", Required = Required.Always)]
        public int SentHeartbeats { get; set; }
    
        /// <summary>The number of dropped packets.</summary>
        [JsonProperty("droppedPackets", Required = Required.Always)]
        public int DroppedPackets { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Contains a receipt and the index of the milestone which contained the receipt.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReceiptTuple 
    {
        [JsonProperty("receipt", Required = Required.Always)]
        public ReceiptPayload Receipt { get; set; }
    
        [JsonProperty("milestoneIndex", Required = Required.Always)]
        public int MilestoneIndex { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Contains a receipt and the index of the milestone which contained the receipt.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReceiptPayload 
    {
        [JsonProperty("migratedAt", Required = Required.Always)]
        public int MigratedAt { get; set; }
    
        [JsonProperty("final", Required = Required.Always)]
        public bool Final { get; set; }
    
        [JsonProperty("funds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<MigratedFundsEntry> Funds { get; set; } = new System.Collections.ObjectModel.Collection<MigratedFundsEntry>();
    
        [JsonProperty("transaction", Required = Required.Always)]
        public TreasuryTransactionPayload Transaction { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MigratedFundsEntry 
    {
        [JsonProperty("tailTransactionHash", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string TailTransactionHash { get; set; }
    
        [JsonProperty("address", Required = Required.Always)]
        public Ed25519Address Address { get; set; }
    
        [JsonProperty("deposit", Required = Required.Always)]
        public int Deposit { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>The error format.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ErrorResponse 
    {
        [JsonProperty("error", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Error Error { get; set; } = new Error();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns general information about the node.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class InfoResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data Data { get; set; } = new Data();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns tips that are ideal for attaching a message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TipsResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data2 Data { get; set; } = new Data2();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Submits a message to the node.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SubmitMessageRequest 
    {
        /// <summary>Network identifier. This field signifies for which network the message is meant for. It also tells which protocol rules apply to the message. It is computed out of the first 8 bytes of the `BLAKE2b-256` hash of the concatenation of the network type and protocol version string.</summary>
        [JsonProperty("networkId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string NetworkId { get; set; }
    
        /// <summary>The identifiers of the messages this message references.</summary>
        [JsonProperty("parentMessageIds", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> ParentMessageIds { get; set; }
    
        [JsonProperty("payload", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public IndexationPayload Payload { get; set; }
    
        /// <summary>The nonce which lets this message fulfill the Proof-of-Work requirement.</summary>
        [JsonProperty("nonce", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns the message identifier of the submitted message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SubmitMessageResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data3 Data { get; set; } = new Data3();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Searches for messages matching a given indexation key.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MessagesFindResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data4 Data { get; set; } = new Data4();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns the metadata of a given message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MessageMetadataResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data5 Data { get; set; } = new Data5();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns a given message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MessageResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data6 Data { get; set; } = new Data6();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns the children of a given message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MessageChildrenResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data7 Data { get; set; } = new Data7();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns an output.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class OutputResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data8 Data { get; set; } = new Data8();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns the balance of an address.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class BalanceAddressResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data9 Data { get; set; } = new Data9();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class OutputsAddressResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data10 Data { get; set; } = new Data10();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReceiptsResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data11 Data { get; set; } = new Data11();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TreasuryResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data12 Data { get; set; } = new Data12();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns information about a milestone.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class MilestoneResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data13 Data { get; set; } = new Data13();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns all UTXO changes of the given milestone.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class UTXOChangesResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data14 Data { get; set; } = new Data14();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns all peers of the node.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class PeersResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<Peer> Data { get; set; } = new System.Collections.ObjectModel.Collection<Peer>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns a given peer of the node.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class PeerResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        public Peer Data { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Adds a given peer to the node.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class AddPeerRequest 
    {
        [JsonProperty("multiAddress", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MultiAddress { get; set; }
    
        [JsonProperty("alias", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    /// <summary>Returns information about an added peer.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class AddPeerResponse 
    {
        [JsonProperty("data", Required = Required.Always)]
        public Peer Data { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Outputs 
    {
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public enum PeerRelation
    {
        [System.Runtime.Serialization.EnumMember(Value = @"included")]
        Included = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = @"conflicting")]
        Conflicting = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = @"noTransaction")]
        NoTransaction = 2,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Error 
    {
        /// <summary>The application error code.</summary>
        [JsonProperty("code", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Code { get; set; }
    
        /// <summary>The error reason.</summary>
        [JsonProperty("message", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Message { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data 
    {
        /// <summary>The name of the node.</summary>
        [JsonProperty("name", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
    
        /// <summary>The semantic version of the node.</summary>
        [JsonProperty("version", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Version { get; set; }
    
        /// <summary>Tells whether the node is healthy or not.</summary>
        [JsonProperty("isHealthy", Required = Required.Always)]
        public bool IsHealthy { get; set; }
    
        /// <summary>Tells on which network the nodes operates on.</summary>
        [JsonProperty("networkId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string NetworkId { get; set; }
    
        /// <summary>Tells whether the node supports mainnet or testnet addresses. Value `iota` indicates that the node supports mainnet addresses. Value `atoi` indicates that the node supports testnet addresses.</summary>
        [JsonProperty("bech32HRP", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Bech32HRP { get; set; }
    
        /// <summary>The Proof-of-Work difficulty for a message to be sent over the network to mitigate spam.</summary>
        [JsonProperty("minPoWScore", Required = Required.Always)]
        public float MinPoWScore { get; set; }
    
        /// <summary>The current rate of new messages per second.</summary>
        [JsonProperty("messagesPerSecond", Required = Required.Always)]
        public float MessagesPerSecond { get; set; }
    
        /// <summary>The current rate of referenced messages per second.</summary>
        [JsonProperty("referencedMessagesPerSecond", Required = Required.Always)]
        public float ReferencedMessagesPerSecond { get; set; }
    
        /// <summary>The ratio of referenced messages in relation to new messages of the last confirmed milestone.</summary>
        [JsonProperty("referencedRate", Required = Required.Always)]
        public float ReferencedRate { get; set; }
    
        /// <summary>The timestamp of the latest seen milestone.</summary>
        [JsonProperty("latestMilestoneTimestamp", Required = Required.Always)]
        public int LatestMilestoneTimestamp { get; set; }
    
        /// <summary>The most recent milestone known to the node.</summary>
        [JsonProperty("latestMilestoneIndex", Required = Required.Always)]
        public int LatestMilestoneIndex { get; set; }
    
        /// <summary>The most recent milestone that has been confirmed by the node.</summary>
        [JsonProperty("confirmedMilestoneIndex", Required = Required.Always)]
        public int ConfirmedMilestoneIndex { get; set; }
    
        /// <summary>Tells from which starting point the node holds data.</summary>
        [JsonProperty("pruningIndex", Required = Required.Always)]
        public int PruningIndex { get; set; }
    
        /// <summary>The features that are supported by the node. For example, a node could support the Proof-of-Work (PoW) feature, which would allow the PoW to be performed by the node itself.</summary>
        [JsonProperty("features", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> Features { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data2 
    {
        /// <summary>The message identifiers that can be used to a attach a message to.</summary>
        [JsonProperty("tipMessageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> TipMessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data3 
    {
        /// <summary>The message identifier of the submitted message.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data4 
    {
        /// <summary>The provided hex-encoded indexation key that was used to search for.</summary>
        [JsonProperty("index", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Index { get; set; }
    
        /// <summary>The number of results it can return at most.</summary>
        [JsonProperty("maxResults", Required = Required.Always)]
        public int MaxResults { get; set; }
    
        /// <summary>The actual number of found results.</summary>
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }
    
        /// <summary>The identifiers of the found messages that match the given indexation key.</summary>
        [JsonProperty("messageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> MessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data5 
    {
        /// <summary>The identifier of the message.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }
    
        /// <summary>The identifiers of the messages this message references.</summary>
        [JsonProperty("parentMessageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ParentMessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        /// <summary>Tells if the message could get solidified by the node or not.</summary>
        [JsonProperty("isSolid", Required = Required.Always)]
        public bool IsSolid { get; set; }
    
        /// <summary>Tells which milestone references this message. If `null` the message was not referenced by a milestone yet.</summary>
        [JsonProperty("referencedByMilestoneIndex", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? ReferencedByMilestoneIndex { get; set; }
    
        /// <summary>If set, this message can be considered as a valid milestone message. This field therefore describes the milestone index of the involved milestone. A message can be considered as a valid milestone message if the milestone payload is valid and if the referenced parents in the milestone payload do match the referenced parents in the message itself. Note it's possible to have different milestone messages that all represent the same milestone.</summary>
        [JsonProperty("milestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int MilestoneIndex { get; set; }
    
        /// <summary>If `included`, the message contains a transaction that has been included in the ledger. If `conflicitng`, the message contains a transaction that has not been included in the ledger because it conflicts with another transaction. If the message does not contain a transaction, `ledgerInclusionState` is set to `noTransaction`.</summary>
        [JsonProperty("ledgerInclusionState", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Data5LedgerInclusionState LedgerInclusionState { get; set; }
    
        /// <summary>Defines the reason why a message is marked as conflicting. Value `1` denotes that the referenced UTXO was already spent. Value `2`denotes that the referenced UTXO was already spent while confirming this milestone. Value `3` denotes that the referenced UTXO cannot be found. Value `4` denotes that the sum of the inputs and output values does not match. Value `5` denotes that the unlock block signature is invalid. Value `6` denotes that the input or output type used is unsupported. Value `7` denotes that the used address type is unsupported. Value `8` denotes that the dust allowance for the address is invalid. Value `9` denotes that the semantic validation failed.</summary>
        [JsonProperty("conflictReason", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int ConflictReason { get; set; }
    
        /// <summary>Tells if the message should be promoted to get more likely picked up by the Coordinator.</summary>
        [JsonProperty("shouldPromote", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShouldPromote { get; set; }
    
        /// <summary>Tells if the message should be reattached.</summary>
        [JsonProperty("shouldReattach", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShouldReattach { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data6 
    {
        [JsonProperty("allOf", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Message AllOf { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data7 
    {
        /// <summary>The message identifier of the given message that was used to look up its children.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }
    
        /// <summary>The number of results it can return at most.</summary>
        [JsonProperty("maxResults", Required = Required.Always)]
        public int MaxResults { get; set; }
    
        /// <summary>The actual number of found results.</summary>
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }
    
        /// <summary>The message identifiers of the found children.</summary>
        [JsonProperty("childrenMessageIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ChildrenMessageIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data8 
    {
        /// <summary>The message identifier that references the output.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }
    
        /// <summary>The identifier of the transaction.</summary>
        [JsonProperty("transactionId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string TransactionId { get; set; }
    
        /// <summary>The index of the output.</summary>
        [JsonProperty("outputIndex", Required = Required.Always)]
        public int OutputIndex { get; set; }
    
        /// <summary>Tells if the output is spent or not.</summary>
        [JsonProperty("isSpent", Required = Required.Always)]
        public bool IsSpent { get; set; }
    
        [JsonProperty("output", Required = Required.Always)]
        public Output Output { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data9 
    {
        /// <summary>The type of the address. Value `0` denotes a Ed25519 address.</summary>
        [JsonProperty("addressType", Required = Required.Always)]
        public int AddressType { get; set; }
    
        /// <summary>The hex-encoded Ed25519 address.</summary>
        [JsonProperty("address", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Address { get; set; }
    
        /// <summary>The computed balance for the address.</summary>
        [JsonProperty("balance", Required = Required.Always)]
        public int Balance { get; set; }
    
        /// <summary>Tells whether the address can receive dust or not.</summary>
        [JsonProperty("dustAllowed", Required = Required.Always)]
        public bool DustAllowed { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data10 
    {
        /// <summary>The type of the address. Value `0` denotes a Ed25519 address.</summary>
        [JsonProperty("addressType", Required = Required.Always)]
        public int AddressType { get; set; }
    
        /// <summary>The hex-encoded Ed25519 address.</summary>
        [JsonProperty("address", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Address { get; set; }
    
        /// <summary>The number of results it can return at most.</summary>
        [JsonProperty("maxResults", Required = Required.Always)]
        public int MaxResults { get; set; }
    
        /// <summary>The actual number of found results.</summary>
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }
    
        /// <summary>The identifiers of the outputs that use a certain address.</summary>
        [JsonProperty("outputIds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> OutputIds { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data11 
    {
        [JsonProperty("receipts", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<ReceiptTuple> Receipts { get; set; } = new System.Collections.ObjectModel.Collection<ReceiptTuple>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data12 
    {
        [JsonProperty("milestoneId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MilestoneId { get; set; }
    
        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data13 
    {
        /// <summary>The index number of the milestone.</summary>
        [JsonProperty("index", Required = Required.Always)]
        public int Index { get; set; }
    
        /// <summary>The identifier of a message which describes this milestone. Note that different messages could describe the same milestone.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }
    
        /// <summary>The timestamp of when the  milestone was issued.</summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        public int Timestamp { get; set; }
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data14 
    {
        /// <summary>The index number of the milestone.</summary>
        [JsonProperty("index", Required = Required.Always)]
        public int Index { get; set; }
    
        /// <summary>The created outputs of the given milestone.</summary>
        [JsonProperty("createdOutputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> CreatedOutputs { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        /// <summary>The consumed outputs of the given milestone.</summary>
        [JsonProperty("consumedOutputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ConsumedOutputs { get; set; } = new System.Collections.ObjectModel.Collection<string>();
    
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public enum Data5LedgerInclusionState
    {
        [System.Runtime.Serialization.EnumMember(Value = @"included")]
        Included = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = @"conflicting")]
        Conflicting = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = @"noTransaction")]
        NoTransaction = 2,
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Output 
    {
        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();
    
        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }
    
    
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.10.9.0 (NJsonSchema v10.4.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class FileParameter
    {
        public FileParameter(System.IO.Stream data)
            : this (data, null, null)
        {
        }

        public FileParameter(System.IO.Stream data, string fileName)
            : this (data, fileName, null)
        {
        }

        public FileParameter(System.IO.Stream data, string fileName, string contentType)
        {
            Data = data;
            FileName = fileName;
            ContentType = contentType;
        }

        public System.IO.Stream Data { get; private set; }

        public string FileName { get; private set; }

        public string ContentType { get; private set; }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.10.9.0 (NJsonSchema v10.4.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class FileResponse : System.IDisposable
    {
        private System.IDisposable client;
        private System.IDisposable response;

        public int StatusCode { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public System.IO.Stream Stream { get; private set; }

        public bool IsPartial
        {
            get { return StatusCode == 206; }
        }

        public FileResponse(int statusCode, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.IO.Stream stream, System.IDisposable client, System.IDisposable response)
        {
            StatusCode = statusCode; 
            Headers = headers; 
            Stream = stream; 
            client = client; 
            response = response;
        }

        public void Dispose() 
        {
            Stream.Dispose();
            if (response != null)
                response.Dispose();
            if (client != null)
                client.Dispose();
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.10.9.0 (NJsonSchema v10.4.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response; 
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.10.9.0 (NJsonSchema v10.4.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108