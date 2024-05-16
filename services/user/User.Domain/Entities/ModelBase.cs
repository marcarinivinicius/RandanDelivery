namespace User.Domain.Entities
{
    public abstract record ModelBase
    {
        public long Id { get; set; }

        internal List<string> _err;

        public IReadOnlyCollection<string>? Err => _err;

        public abstract bool Validate();
    }
}
