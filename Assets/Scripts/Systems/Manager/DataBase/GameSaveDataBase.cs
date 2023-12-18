using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace GDD.DataBase
{
    [Table("SaveData")]
    public class GameSaveDataBase : BaseModel
    {
        [PrimaryKey("user_id", false)]
        public string user_id { get; set; }
        
        [Column("savedata")]
        public object savedata { get; set; }
        
        [Column("created_at")]
        public DateTime created_at { get; set; }
    }
}