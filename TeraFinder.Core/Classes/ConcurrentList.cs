using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.ComponentModel;

namespace TeraFinder.Core;

public class ConcurrentList<T> : ConcurrentBag<T>, IBindingList
{
    public bool IsFinalized { get; private set; } = false;

    private ImmutableArray<T> _binding;

    public void FinalizeElements()
    {
        _binding = [.. this];
        IsFinalized = true;
    }

    public new void Clear()
    {
        base.Clear();
        _binding = _binding.Clear();
        IsFinalized = false;
    }

    object? IList.this[int index] 
    {
        get => GetElement(index);
        set => throw new AccessViolationException("The List can not be edited.");
    }

    private T GetElement(int index)
    {
        if (IsFinalized)
            return _binding[index];

        throw new AccessViolationException("The List has not been finalized.");
    }

    bool IBindingList.AllowEdit => IsFinalized;
    bool IBindingList.AllowNew => !IsFinalized;
    bool IBindingList.AllowRemove => false;
    bool IBindingList.IsSorted => false;

    ListSortDirection IBindingList.SortDirection => throw new NotImplementedException();
    PropertyDescriptor? IBindingList.SortProperty => throw new NotImplementedException();
    bool IBindingList.SupportsChangeNotification => false;
    bool IBindingList.SupportsSearching => IsFinalized;
    bool IBindingList.SupportsSorting => false;

    bool IList.IsFixedSize => false;
    bool IList.IsReadOnly => false;

    event ListChangedEventHandler IBindingList.ListChanged
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }

    int IList.Add(object? value)
    {
#pragma warning disable CS8600
#pragma warning disable CS8604
        Add((T)value);
#pragma warning restore CS8600
#pragma warning restore CS8604
        return 0;
    }

    void IBindingList.AddIndex(PropertyDescriptor property)
    {
        throw new NotImplementedException();
    }

    object? IBindingList.AddNew()
    {
        throw new NotImplementedException();
    }

    void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
        throw new NotImplementedException();
    }

    void IList.Clear() => Clear();

    bool IList.Contains(object? value)
    {
        throw new NotImplementedException();
    }

    int IBindingList.Find(PropertyDescriptor property, object key)
    {
        throw new NotImplementedException();
    }

    int IList.IndexOf(object? value)
    {
        throw new NotImplementedException();
    }

    void IList.Insert(int index, object? value)
    {
        throw new NotImplementedException();
    }

    void IList.Remove(object? value)
    {
        throw new NotImplementedException();
    }

    void IList.RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    void IBindingList.RemoveIndex(PropertyDescriptor property)
    {
        throw new NotImplementedException();
    }

    void IBindingList.RemoveSort()
    {
        throw new NotImplementedException();
    }
}
