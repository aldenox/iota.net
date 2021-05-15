namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
