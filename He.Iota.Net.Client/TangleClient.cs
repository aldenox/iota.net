namespace He.Iota.Net.Client
{
    using He.Iota.Net.Client.Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class TangleClient
    {
        public string BaseUrl { get; set; } = "http://127.0.0.1:14265";
        private readonly HttpClient HttpClient;

        public TangleClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns the health of the node.</summary>
        /// <returns>Successful operation: indicates that the node is healthy.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task HealthAsync(CancellationToken cancellationToken = default)
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

                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
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



                        var status = (int)response.StatusCode;
                        if (status == 200)
                        {
                            return;
                        }
                        else
                        if (status == 403)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, responseText, headers, null);
                        }
                        else
                        if (status == 500)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that an unexpected, internal server error happened which prevented the node from fulfilling the request.", status, responseText, headers, null);
                        }
                        else
                        if (status == 503)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns general information about the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<InfoResponse> InfoAsync(CancellationToken cancellationToken = default)
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns tips that are ideal for attaching a message.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<TipsResponse> TipsAsync(CancellationToken cancellationToken = default)
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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
                throw new ArgumentNullException(nameof(submitMessageRequest));
            }

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages");

            submitMessageRequest.Payload.Index = submitMessageRequest.Payload.Index.ToHex();
            submitMessageRequest.Payload.Data = submitMessageRequest.Payload.Data.ToHex();

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(submitMessageRequest);

            // Wrap our JSON inside a StringContent which then can be used by the this.HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var uri = urlBuilder.ToString();

            var response = await this.HttpClient.PostAsync(uri, httpContent);
            var responseText = await response.Content.ReadAsStringAsync();

            try
            {
                var typedBody = JsonConvert.DeserializeObject<SubmitMessageResponse>(responseText);
                return typedBody;
            }
            catch (JsonException exception)
            {
                throw;
            }
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Submit a message.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<SubmitMessageResponse> MessagesAsync(SubmitMessageRequest body, CancellationToken cancellationToken = default)
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
                    var content = new StringContent(JsonConvert.SerializeObject(body));
                    content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request.Content = content;
                    request.Method = new HttpMethod("POST");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Search for messages matching a given indexation key.</summary>
        /// <param name="index">Hex-encoded indexation key that should be searched for.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessagesFindResponse> Messages2Async(string index, CancellationToken cancellationToken = default)
        {
            if (index == null)
                throw new ArgumentNullException("index");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages?");
            urlBuilder.Append(Uri.EscapeDataString("index") + "=").Append(Uri.EscapeDataString(ConvertToString(index, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns message data as JSON by its identifier.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageResponse> Messages3Async(string messageId, CancellationToken cancellationToken = default)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}");
            urlBuilder.Replace("{messageId}", Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Find the metadata of a given message.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageMetadataResponse> MetadataAsync(string messageId, CancellationToken cancellationToken = default)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}/metadata");
            urlBuilder.Replace("{messageId}", Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns message raw bytes by its identifier.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<FileResponse> RawAsync(string messageId, CancellationToken cancellationToken = default)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}/raw");
            urlBuilder.Replace("{messageId}", Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/octet-stream"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns the children of a message.</summary>
        /// <param name="messageId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageChildrenResponse> ChildrenAsync(string messageId, CancellationToken cancellationToken = default)
        {
            if (messageId == null)
                throw new ArgumentNullException("messageId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/messages/{messageId}/children");
            urlBuilder.Replace("{messageId}", Uri.EscapeDataString(ConvertToString(messageId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Find an output by its identifier.</summary>
        /// <param name="outputId">Identifier of the output encoded in hex. An output is identified by the concatenation of `transactionid+outputindex`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<OutputResponse> OutputsAsync(string outputId, CancellationToken cancellationToken = default)
        {
            if (outputId == null)
                throw new ArgumentNullException("outputId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/outputs/{outputId}");
            urlBuilder.Replace("{outputId}", Uri.EscapeDataString(ConvertToString(outputId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get the balance of a bech32-encoded address.</summary>
        /// <param name="address">bech32 encoded address</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<BalanceAddressResponse> AddressesAsync(string address, CancellationToken cancellationToken = default)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/{address}");
            urlBuilder.Replace("{address}", Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get the balance of a hex-encoded Ed25519 address.</summary>
        /// <param name="address">hex-encoded Ed25519 address</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<BalanceAddressResponse> Ed25519Async(string address, CancellationToken cancellationToken = default)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/ed25519/{address}");
            urlBuilder.Replace("{address}", Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get all outputs that use a given bech32-encoded address.</summary>
        /// <param name="address">bech32-encoded address that is referenced by the outputs.</param>
        /// <param name="includespent">Set to true to also include the known spent outputs for the given address.</param>
        /// <param name="type">Allows to filter the results by output type. Set to value `0` to filter outputs of type `SigLockedSingleOutput`. Set to value `1` to filter outputs of type `SigLockedDustAllowanceOutput`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<OutputsAddressResponse> Outputs2Async(string address, bool? includespent, int? type, CancellationToken cancellationToken = default)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/{address}/outputs?");
            urlBuilder.Replace("{address}", Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));
            if (includespent != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("include-spent") + "=").Append(Uri.EscapeDataString(ConvertToString(includespent, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (type != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("type") + "=").Append(Uri.EscapeDataString(ConvertToString(type, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get all outputs that use a given hex-encoded Ed25519 address.</summary>
        /// <param name="address">hex-encoded Ed25519 address that is referenced by the outputs.</param>
        /// <param name="includespent">Set to true to also include the known spent outputs for the given address.</param>
        /// <param name="type">Allows to filter the results by output type. Set to value `0` to filter outputs of type `SigLockedSingleOutput`. Set to value `1` to filter outputs of type `SigLockedDustAllowanceOutput`.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<OutputsAddressResponse> Outputs3Async(string address, bool? includespent, int? type, CancellationToken cancellationToken = default)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/addresses/ed25519/{address}/outputs?");
            urlBuilder.Replace("{address}", Uri.EscapeDataString(ConvertToString(address, System.Globalization.CultureInfo.InvariantCulture)));
            if (includespent != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("include-spent") + "=").Append(Uri.EscapeDataString(ConvertToString(includespent, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (type != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("type") + "=").Append(Uri.EscapeDataString(ConvertToString(type, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns all stored receipts.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ReceiptsResponse> ReceiptsAsync(CancellationToken cancellationToken = default)
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns all stored receipts for a given migration index.</summary>
        /// <param name="migratedAt">Migration index to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ReceiptsResponse> Receipts2Async(double migratedAt, CancellationToken cancellationToken = default)
        {
            if (migratedAt == null)
                throw new ArgumentNullException("migratedAt");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/receipts/{migratedAt}");
            urlBuilder.Replace("{migratedAt}", Uri.EscapeDataString(ConvertToString(migratedAt, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns information about the treasury.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<TreasuryResponse> TreasuryAsync(CancellationToken cancellationToken = default)
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns the included message of a transaction.</summary>
        /// <param name="transactionId">Identifier of the transaction to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MessageResponse> IncludedMessageAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            if (transactionId == null)
                throw new ArgumentNullException("transactionId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/transactions/{transactionId}/included-message");
            urlBuilder.Replace("{transactionId}", Uri.EscapeDataString(ConvertToString(transactionId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Look up a milestone by a given milestone index.</summary>
        /// <param name="index">Index of the milestone to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<MilestoneResponse> MilestonesAsync(double index, CancellationToken cancellationToken = default)
        {
            if (index == null)
                throw new ArgumentNullException("index");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/milestones/{index}");
            urlBuilder.Replace("{index}", Uri.EscapeDataString(ConvertToString(index, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get all UTXO changes of a given milestone.</summary>
        /// <param name="index">Index of the milestone to look up.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<UTXOChangesResponse> UtxoChangesAsync(double index, CancellationToken cancellationToken = default)
        {
            if (index == null)
                throw new ArgumentNullException("index");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/milestones/{index}/utxo-changes");
            urlBuilder.Replace("{index}", Uri.EscapeDataString(ConvertToString(index, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get information about the peers of the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<PeersResponse> PeersAsync(CancellationToken cancellationToken = default)
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



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Add a given peer to the node.</summary>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<AddPeerResponse> Peers2Async(AddPeerRequest body, CancellationToken cancellationToken = default)
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
                    var content = new StringContent(JsonConvert.SerializeObject(body));
                    content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request.Content = content;
                    request.Method = new HttpMethod("POST");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get information about a given peer.</summary>
        /// <param name="peerId">Identifier of the message.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<PeerResponse> Peers3Async(string peerId, CancellationToken cancellationToken = default)
        {
            if (peerId == null)
                throw new ArgumentNullException("peerId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/peers/{peerId}");
            urlBuilder.Replace("{peerId}", Uri.EscapeDataString(ConvertToString(peerId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Remove/disconnect a given peer.</summary>
        /// <param name="peerId">Identifier of the peer.</param>
        /// <returns>Successful operation.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task Peers4Async(string peerId, CancellationToken cancellationToken = default)
        {
            if (peerId == null)
                throw new ArgumentNullException("peerId");

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/peers/{peerId}");
            urlBuilder.Replace("{peerId}", Uri.EscapeDataString(ConvertToString(peerId, System.Globalization.CultureInfo.InvariantCulture)));

            var client = this.HttpClient;
            var disposeClient = false;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("DELETE");



                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);



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



                        var status = (int)response.StatusCode;
                        if (status == 204)
                        {
                            return;
                        }
                        else
                        if (status == 400)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the provided data is invalid.", status, responseText, headers, null);
                        }
                        else
                        if (status == 403)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the endpoint is not available for public use.", status, responseText, headers, null);
                        }
                        else
                        if (status == 404)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("Unsuccessful operation: indicates that the requested data was not found.", status, responseText, headers, null);
                        }
                        else
                        if (status == 500)
                        {
                            string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

        protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, CancellationToken cancellationToken)
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
                    var typedBody = JsonConvert.DeserializeObject<T>(responseText);
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
                        var serializer = JsonSerializer.Create();
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
                return System.Convert.ToBase64String((byte[])value);
            }
            else if (value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            var result = System.Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
        }
    }

}
