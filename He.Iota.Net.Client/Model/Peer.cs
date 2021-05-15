namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
