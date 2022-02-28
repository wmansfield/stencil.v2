using System;

namespace Placeholder.Domain
{
    public class DerivedField<TEntity>
        where TEntity : class
    {
        public DerivedField()
        {
            this.Value = default(TEntity);
        }
        public DerivedField(TEntity value)
        {
            this.Value = value;
        }

        public virtual TEntity Value { get; set; }
    }

    public static class _DerivedField
    {
        public static bool HasValue<TEntity>(this DerivedField<TEntity> entity)
            where TEntity : class
        {
            if (entity != null && entity.Value != null)
            {
                return true;
            }
            return false;
        }
        public static TEntity GetValueOrDefault<TEntity>(this DerivedField<TEntity> entity)
            where TEntity : class
        {
            if (entity != null && entity.Value != null)
            {
                return entity.Value;
            }
            return null;
        }
    }
}
