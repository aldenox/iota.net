namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>References a previous unlock block in order to substitute the duplication of the same unlock block data for inputs which unlock through the same data.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ReferenceUnlockBlock
    {
        /// <summary>Set to value 1 to denote a Reference Unlock Block.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        /// <summary>Represents the index of a previous unlock block.</summary>
        [JsonProperty("reference", Required = Required.Always)]
        public int Reference { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
