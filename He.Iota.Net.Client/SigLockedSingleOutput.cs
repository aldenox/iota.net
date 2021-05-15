﻿namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    /// <summary>Describes a deposit to a single address which is unlocked via a signature.</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SigLockedSingleOutput
    {
        /// <summary>Set to value 0 to denote a SigLockedSingleOutput.</summary>
        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty("address", Required = Required.Always)]
        public Ed25519Address Address { get; set; }

        /// <summary>The amount of tokens to deposit with this SigLockedSingleOutput output.</summary>
        [JsonProperty("amount", Required = Required.Always)]
        public int Amount { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
