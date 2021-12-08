namespace ClassLibrary1.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = DefaultValues.Undefined;

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; } = DefaultValues.Undefined;
    }
}
