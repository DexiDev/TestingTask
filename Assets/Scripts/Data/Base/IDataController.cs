using System;

namespace Game.Data
{
    public interface IDataController
    {
        public event Action<IDataField> OnFieldAdded;
        public event Action<IDataField> OnFieldRemoved;

        public bool HasData<T>() where T : class, IDataField, new();

        public T[] GetDataFields<T>() where T : IDataField;

        public T GetDataField<T>(T dataField = null) where T : class, IDataField;

        public T GetDataField<T>(bool isAutoCreate, T dataField = null) where T : class, IDataField, new();

        public bool TryGetDataField<T>(out T dataField) where T : class, IDataField, new();

        public bool TryAddDataField<T>(T dataField = null) where T : class, IDataField, new();

        public void AddDataField<T>(T dataField) where T : class, IDataField;

        public void RemoveDataField<T>(T dataField) where T : class, IDataField;
    }
}