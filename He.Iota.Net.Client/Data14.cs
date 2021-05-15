namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Data14
    {
        /// <summary>The index number of the milestone.</summary>
        [JsonProperty("index", Required = Required.Always)]
        public int Index { get; set; }

        /// <summary>The created outputs of the given milestone.</summary>
        [JsonProperty("createdOutputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> CreatedOutputs { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        /// <summary>The consumed outputs of the given milestone.</summary>
        [JsonProperty("consumedOutputs", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<string> ConsumedOutputs { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
