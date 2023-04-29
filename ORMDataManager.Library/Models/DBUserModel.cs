using System;

namespace ORMDataManager.Library.DataAccess.Models
{
    /// <summary>
    /// Capturing infomation from the database
    /// </summary>
    public class DBUserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}