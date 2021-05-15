namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>The Ed25519 signature.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Ed25519Signature
    {
        /// <summary>Set to value 0 to denote an Ed25519 Signature.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        /// <summary>The public key of the Ed25519 keypair which is used to verify the signature.</summary>
        [JsonProperty("publicKey", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string PublicKey { get; set; }

        /// <summary>The signature signing the serialized Transaction Essence.</summary>
        [JsonProperty("signature", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Signature { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
