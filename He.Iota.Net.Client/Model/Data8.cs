namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data8
    {
        /// <summary>The message identifier that references the output.</summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MessageId { get; set; }

        /// <summary>The identifier of the transaction.</summary>
        [JsonProperty("transactionId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string TransactionId { get; set; }

        /// <summary>The index of the output.</summary>
        [JsonProperty("outputIndex", Required = Required.Always)]
        public int OutputIndex { get; set; }

        /// <summary>Tells if the output is spent or not.</summary>
        [JsonProperty("isSpent", Required = Required.Always)]
        public bool IsSpent { get; set; }

        [JsonProperty("output", Required = Required.Always)]
        public Output Output { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
