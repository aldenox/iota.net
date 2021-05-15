namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
