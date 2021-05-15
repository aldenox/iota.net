namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
