namespace He.Iota.Net.Client.Model
{
    using Newtonsoft.Json;

    /// <summary>Returns information about an added peer.</summary>
    public class AddPeerResponse
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
}
