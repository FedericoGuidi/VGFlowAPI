using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Domain.Entities
{
    public class Social
    {
        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
    }
}
