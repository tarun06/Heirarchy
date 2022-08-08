using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PrismApp.Modules.Users.Models
{
    [DataContract]
    public class SuperviserInfo
    {
        /// <summary>
        /// Gets or sets members of Superviser.
        /// </summary>
        public ICollection<SuperviserInfo>? Members { get; set; }

        [DataMember(Name = "email", IsRequired = true)]
        public string? Email { get; set; }

        [DataMember(Name = "firstName", IsRequired = true)]
        public string? FirstName { get; set; }

        [DataMember(Name = "id", IsRequired = true)]
        public int Id { get; set; } = int.MaxValue;

        [DataMember(Name = "lastName", IsRequired = true)]
        public string? LastName { get; set; }

        [DataMember(Name = "phone", IsRequired = true)]
        public string? Phone { get; set; }

        [DataMember(Name = "supervisorId", IsRequired = true)]
        public int? SupervisorId { get; set; }

        [DataMember(Name = "title", IsRequired = true)]
        public string? Title { get; set; }
    }
}