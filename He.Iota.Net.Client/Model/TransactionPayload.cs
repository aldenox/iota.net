namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>The Transaction Payload to be embedded into a message.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TransactionPayload
    {
        /// <summary>Set to value 0 to denote a Transaction Payload.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty("essence", Required = Required.Always)]
        public TransactionEssence Essence { get; set; } = new TransactionEssence();

        [JsonProperty("unlockBlocks", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<SignatureUnlockBlock> UnlockBlocks { get; set; } = new System.Collections.ObjectModel.Collection<SignatureUnlockBlock>();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
