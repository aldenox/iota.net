namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>Contains a receipt and the index of the milestone which contained the receipt.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReceiptTuple
    {
        [JsonProperty("receipt", Required = Required.Always)]
        public ReceiptPayload Receipt { get; set; }

        [JsonProperty("milestoneIndex", Required = Required.Always)]
        public int MilestoneIndex { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
