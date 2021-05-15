namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data11
    {
        [JsonProperty("receipts", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<ReceiptTuple> Receipts { get; set; } = new System.Collections.ObjectModel.Collection<ReceiptTuple>();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
