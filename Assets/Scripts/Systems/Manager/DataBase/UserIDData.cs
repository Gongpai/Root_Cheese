using Postgrest.Attributes;
using Postgrest.Models;

namespace GDD.DataBase
{
    [Table("UserID")]
    public class UserIDData : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("username")] 
        public string username { get; set; }
        
        [Column("password")]
        public string password { get; set; }
    }
}