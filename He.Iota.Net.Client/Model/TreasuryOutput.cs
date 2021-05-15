namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TreasuryOutput
    {
        /// <summary>Set to value 2 to denote a TreasuryOutput.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
