namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data13
    {
        /// <summary>The index number of the milestone.</summary>
        [JsonProperty("index", Required = Required.Always)]
        public int Index { get; set; }

        /// <summary>The identifier of a message which describes this milestone. Note that different messages could describe the same milestone.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }

        /// <summary>The timestamp of when the  milestone was issued.</summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        public int Timestamp { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
