namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>Defines an unlock block containing signature(s) unlocking input(s).</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SignatureUnlockBlock
    {
        /// <summary>Denotes a Signature Unlock Block.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty("signature", Required = Required.Always)]
        public Ed25519Signature Signature { get; set; } = new Ed25519Signature();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
