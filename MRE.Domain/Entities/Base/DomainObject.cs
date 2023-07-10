using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Base
{
    public abstract class DomainObject : IDomainObject
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public Guid? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }
        [JsonIgnore]
        public Guid? UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime? ValidUntil { get; set; }

        public Guid? TenantId { get; set; }
        [DefaultValue(true)]
        public bool? IsActive { get; set; }

    }
}
