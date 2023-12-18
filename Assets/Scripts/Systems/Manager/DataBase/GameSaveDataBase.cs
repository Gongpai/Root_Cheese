using System;
using Newtonsoft.Json.Bson;
using Postgrest.Attributes;
using Postgrest.Models;

namespace GDD.DataBase
{
    [Table("SaveData")]
    public class GameSaveDataBase : BaseModel
    {
        [Column("user_id")] 
        public string user_id { get; set; }
        
        [Column("savedata")] 
        public string savedata { get; set; }
        
        [Column("created_at")]
        public DateTime created_at { get; set; }
    }
}