namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>Describes the essence data making up a transaction by defining its inputs and outputs and an optional payload.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TransactionEssence
    {
        /// <summary>Set to value 0 to denote a Transaction Essence.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty("inputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<UTXOInput> Inputs { get; set; } = new System.Collections.ObjectModel.Collection<UTXOInput>();

        [JsonProperty("outputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<Outputs> Outputs { get; set; } = new System.Collections.ObjectModel.Collection<Outputs>();

        [JsonProperty("payload", Required = Required.AllowNull)]
        public IndexationPayload Payload { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
