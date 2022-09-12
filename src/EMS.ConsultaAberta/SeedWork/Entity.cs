namespace EMS.ConsultaAberta.SeedWork;

public abstract class Entity<TKey>  where TKey : IComparable
    {
        protected Entity(TKey id)
        {
            Id = id;
        }

        public virtual TKey Id { get; protected set; }
        
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<TKey>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;
            if (ReferenceEquals(null, Id)) return false;

            return $"{GetType().Name}.{Id}".Equals($"{compareTo.GetType().Name}.{compareTo.Id}");
        }

        public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
            => !(a == b);

        public override int GetHashCode()
            => (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public override string ToString()
            => $"{GetType().Name} [Id={Id}]";

        public virtual int CompareTo(Entity<TKey> other)
            => Id.CompareTo(other.Id);
    }