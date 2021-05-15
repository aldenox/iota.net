namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>Contains a receipt and the index of the milestone which contained the receipt.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReceiptPayload
    {
        [JsonProperty("migratedAt", Required = Required.Always)]
        public int MigratedAt { get; set; }

        [JsonProperty("final", Required = Required.Always)]
        public bool Final { get; set; }

        [JsonProperty("funds", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<MigratedFundsEntry> Funds { get; set; } = new System.Collections.ObjectModel.Collection<MigratedFundsEntry>();

        [JsonProperty("transaction", Required = Required.Always)]
        public TreasuryTransactionPayload Transaction { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
