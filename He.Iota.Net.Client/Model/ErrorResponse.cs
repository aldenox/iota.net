namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>The error format.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ErrorResponse
    {
        [JsonProperty("error", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Error Error { get; set; } = new Error();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
