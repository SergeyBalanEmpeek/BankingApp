using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BankingApp.UnitTests
{
    /// <summary>
    /// Implementation for IUpdateEntry interface. Used for throwing DbUpdateConcurrencyException
    /// </summary>
    public class UpdateEntryTest: IUpdateEntry
    {
        public IEntityType EntityType { get; }
        public EntityState EntityState { get; }
        public IUpdateEntry SharedIdentityEntry { get; }
        public object GetCurrentValue(IPropertyBase propertyBase) { return null; }
        public TProperty GetCurrentValue<TProperty>(IPropertyBase propertyBase) { return default(TProperty); }
        public object GetOriginalValue(IPropertyBase propertyBase) { return null; }
        public TProperty GetOriginalValue<TProperty>(IProperty property) { return default(TProperty); }
        public bool HasTemporaryValue(IProperty property) { return false; }
        public bool IsModified(IProperty property) { return false; }
        public bool IsStoreGenerated(IProperty property) { return false; }
        public void SetCurrentValue(IPropertyBase propertyBase, object value) { }
        public EntityEntry ToEntityEntry() { return null; }
    }
}
