namespace He.Iota.Net.Client
{
    using Newtonsoft.Json;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Heartbeat
    {
        /// <summary>The most recent milestone that has been solidified by the node.</summary>
        [JsonProperty("solidMilestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int SolidMilestoneIndex { get; set; }

        /// <summary>Tells from which starting point the node holds data.</summary>
        [JsonProperty("prunedMilestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int PrunedMilestoneIndex { get; set; }

        /// <summary>The most recent milestone known to the node.</summary>
        [JsonProperty("latestMilestoneIndex", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int LatestMilestoneIndex { get; set; }

        /// <summary>Tells how many connected peers the node has.</summary>
        [JsonProperty("connectedNeighbors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int ConnectedNeighbors { get; set; }

        /// <summary>Tells how many synced peers the node has.</summary>
        [JsonProperty("syncedNeighbors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int SyncedNeighbors { get; set; }

        private System.Collections.Generic.IDictionary<string, object> additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return additionalProperties; }
            set { additionalProperties = value; }
        }


    }

}
