namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

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

}
