namespace Notify.Domain.Entities
{
    public abstract record ModelBase
    {
        public long Id { get; set; }

        public bool Active { get; set; } = true; // padrão de inicialização do campo

        internal List<string> _err;

        public IReadOnlyCollection<string>? Err => _err;

    }
}
