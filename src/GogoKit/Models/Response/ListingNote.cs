﻿using System.Runtime.Serialization;
using HalKit.Resources;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class ListingNote : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }
    }
}
