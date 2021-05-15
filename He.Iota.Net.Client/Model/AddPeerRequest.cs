namespace He.Iota.Net.Client.Model
{
    using Newtonsoft.Json;

    /// <summary>Adds a given peer to the node.</summary>
    public class AddPeerRequest
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
}
