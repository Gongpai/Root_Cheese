using System;

namespace GDD.DataBase
{
    public interface ITableData
    {
        public string user_id { get; set; }

        public object savedata { get; set; }

        public DateTime created_at { get; set; }
    }
}