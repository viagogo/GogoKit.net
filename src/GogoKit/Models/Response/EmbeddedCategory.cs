﻿using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A Category represents a grouping of events or other categories. Examples are “Concerts”, “Rock and Pop” and “Lady Gaga”.
    /// </summary>
    [DataContract]
    public class EmbeddedCategory : Resource
    {
        /// <summary>
        /// The category identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The external mappings for this category.
        /// </summary>
        [Embedded("external_mappings")]
        public EmbeddedExternalMappingResource[] ExternalMappings { get; set; }
    }
}
