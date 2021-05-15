namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
