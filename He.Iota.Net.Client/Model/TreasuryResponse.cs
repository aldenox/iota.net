﻿namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TreasuryResponse
    {
        [JsonProperty("data", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Data12 Data { get; set; } = new Data12();

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
