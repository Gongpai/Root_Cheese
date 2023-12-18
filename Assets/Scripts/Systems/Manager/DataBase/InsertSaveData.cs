using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace GDD.DataBase
{
    [Table("SaveData")]
    public class InsertSaveData : BaseModel
    {
        [Column("user_id")] public string user_id { get; set; }

        [Column("savedata")] public object savedata { get; set; }

        [Column("created_at")] public DateTime created_at { get; set; }

    }
}