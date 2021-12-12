namespace ClassLibrary1
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = Constant.Undefined;

        public DateTime? Modified { get; set; }

        public string ModifiedBy { get; set; } = Constant.Undefined;
    }
}
