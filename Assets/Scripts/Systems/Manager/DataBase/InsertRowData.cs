using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace GDD.DataBase
{
    [Table("SaveData")]
    public class InsertRowData : BaseModel, ITableData
    {
        [Column("user_id")] public string user_id { get; set; }
        [Column("playerInfo")] public object playerInfo { get; set; }
        [Column("gameSave")] public object gameSave { get; set; }

        [Column("created_at")] public DateTime created_at { get; set; }

    }
}