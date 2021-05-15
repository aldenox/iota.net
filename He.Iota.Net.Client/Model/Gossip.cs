namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
