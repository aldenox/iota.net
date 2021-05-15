namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data9
    {
        /// <summary>The type of the address. Value `0` denotes a Ed25519 address.</summary>
        [JsonProperty("addressType", Required = Required.Always)]
        public int AddressType { get; set; }

        /// <summary>The hex-encoded Ed25519 address.</summary>
        [JsonProperty("address", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Address { get; set; }

        /// <summary>The computed balance for the address.</summary>
        [JsonProperty("balance", Required = Required.Always)]
        public int Balance { get; set; }

        /// <summary>Tells whether the address can receive dust or not.</summary>
        [JsonProperty("dustAllowed", Required = Required.Always)]
        public bool DustAllowed { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
