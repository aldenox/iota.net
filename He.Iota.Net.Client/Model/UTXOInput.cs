namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>Describes an input which references an unspent transaction output to consume.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class UTXOInput
    {
        /// <summary>Set to value 0 to denote an UTXO Input.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        /// <summary>The BLAKE2b-256 hash of the transaction from which the UTXO comes from.</summary>
        [JsonProperty("transactionId", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string TransactionId { get; set; }

        /// <summary>The index of the output on the referenced transaction to consume.</summary>
        [JsonProperty("transactionOutputIndex", Required = Required.Always)]
        public int TransactionOutputIndex { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
