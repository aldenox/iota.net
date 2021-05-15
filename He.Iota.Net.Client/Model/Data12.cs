namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data12
    {
        [JsonProperty("milestoneId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string MilestoneId { get; set; }

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
