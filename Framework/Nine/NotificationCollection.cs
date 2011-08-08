#region Copyright 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
#endregion

namespace Nine
{
    /// <summary>
    ///  Notifies clients that the collection has changed.
    /// </summary>
    public interface INotifyCollectionChanged<T>
    {
        /// <summary>
        /// Raised when a new element is added to the collection.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs<T>> Added;

        /// <summary>
        /// Raised when an element is removed from the collection.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs<T>> Removed;
    }

    /// <summary>
    /// Event args for changed an item.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NotifyCollectionChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the index of the added or removed item.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the new value of the item.
        /// </summary>
        public T Value { get; internal set; }

        public NotifyCollectionChangedEventArgs() { }
        public NotifyCollectionChangedEventArgs(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }

    /// <summary>
    /// A collection that can notify changes.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy("System.Collections.Generic.Mscorlib_CollectionDebugView`1, mscorlib")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NotificationCollection<T> : IList<T>, INotifyCollectionChanged<T>
    {
        private bool isDirty = true;
        private List<T> elements = null;
        private List<T> copy = null;

        /// <summary>
        /// Gets or sets the sender that raise the Added and Removed event.
        /// </summary>
        internal object Sender = null;
        internal bool EnableManipulationWhenEnumerating;
        
        /// <summary>
        /// Creates a new instance of EnumerableCollection.
        /// </summary>
        public NotificationCollection()
        {
            Sender = this;
        }

        /// <summary>
        /// Raised when a new element is added to the collection.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs<T>> Added;

        /// <summary>
        /// Raised when an element is removed from the collection.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs<T>> Removed;

        
        /// <summary>
        /// Gets the enumerator associated with is collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (!EnableManipulationWhenEnumerating)
                return elements != null ? elements.GetEnumerator() : Enumerable.Empty<T>().GetEnumerator();

            // Copy a new list while iterating it
            if (isDirty)
            {
                if (copy == null)
                    copy = new List<T>();
                else
                    copy.Clear();

                if (elements != null)
                {
                    foreach (T e in elements)
                        copy.Add(e);
                }

                isDirty = false;
            }

            return new NotificationCollectionEnumerator<T>() { List = copy, CurrentIndex = -1 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a new item to the collection.
        /// </summary>
        public void Add(T value)
        {
            if (elements == null)
                elements = new List<T>();

            isDirty = true;
            elements.Add(value);

            OnAdded(elements.Count - 1, value);
        }

        /// <summary>
        /// Adds a collection of items to this collection.
        /// </summary>
        public void AddRange(IEnumerable<T> elements)
        {
            foreach (T e in elements)
                Add(e);
        }

        /// <summary>
        /// Removes the first occurrance of an item from the collection.
        /// </summary>
        public bool Remove(T value)
        {
            if (elements == null)
                return false;

            isDirty = true;
            int index = elements.IndexOf(value);
            if (index < 0)
                return false;

            elements.RemoveAt(index);

            OnRemoved(index, value);

            return true;
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public void Clear()
        {
            if (elements != null)
            {
                isDirty = true;
                List<T> temp = elements;
                elements = null;

                if (elements != null)
                    for (int i = 0; i < elements.Count; i++)
                        OnRemoved(i, elements[i]);

                temp.Clear();
            }
        }

        /// <summary>
        /// Gets whether the list is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the count of the list.
        /// </summary>
        public int Count
        {
            get { return elements != null ? elements.Count : 0; }
        }

        /// <summary>
        /// Gets whether the list contains the specifed value.
        /// </summary>
        public bool Contains(T item)
        {
            return elements != null ? elements.Contains(item) : false;
        }

        /// <summary>
        /// Copy the list content to an array at the specified index.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (elements != null)
                elements.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the index of an item.
        /// </summary>
        public int IndexOf(T item)
        {
            return elements != null ?  elements.IndexOf(item) : -1;
        }

        /// <summary>
        /// Inserts an item at the specifed index.
        /// </summary>
        public void Insert(int index, T item)
        {
            if (elements == null)
                elements = new List<T>();

            isDirty = true;
            elements.Insert(index, item);

            OnAdded(index, item);
        }

        /// <summary>
        /// Removes an item at the specified index.
        /// </summary>
        public void RemoveAt(int index)
        {
            if (elements != null)
            {
                isDirty = true;
                T e = elements[index];
                elements.RemoveAt(index);

                OnRemoved(index, e);
            }
        }

        /// <summary>
        /// Removes all items that matches the specified condition.
        /// </summary>
        public int RemoveAll(Predicate<T> match)
        {
            if (elements == null)
                return 0;

            isDirty = true;

            int count = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (match(elements[i]))
                {
                    T e = elements[i];

                    elements.RemoveAt(i);

                    OnRemoved(i, e);

                    i--;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets or sets the value at the given index.
        /// </summary>
        public T this[int index]
        {
            get 
            {
                return elements != null ? elements[index] : default(T); 
            }
            
            set
            {
                if (elements == null)
                    elements = new List<T>();

                T oldValue = elements[index];
                OnRemoved(index, oldValue);
                elements[index] = value;
                isDirty = true;
                OnAdded(index, value);
            }
        }

        /// <summary>
        /// Raised when a new element is added to the collection.
        /// </summary>
        protected virtual void OnAdded(int index, T value)
        {
            if (Added != null)
                Added(Sender, new NotifyCollectionChangedEventArgs<T> { Index = index, Value = value });
        }

        /// <summary>
        /// Raised when an element is removed from the collection.
        /// </summary>
        protected virtual void OnRemoved(int index, T value)
        {
            if (Removed != null)
                Removed(Sender, new NotifyCollectionChangedEventArgs<T> { Index = index, Value = value });
        }
    }

    class NotificationCollectionEnumerator<T> : IEnumerator<T>
    {
        internal IList<T> List;
        internal int CurrentIndex;

        public T Current
        {
            get { return List[CurrentIndex]; }
        }

        public void Dispose()
        {

        }

        object IEnumerator.Current
        {
            get { return List[CurrentIndex]; }
        }

        public bool MoveNext()
        {
            return ++CurrentIndex < List.Count;
        }

        public void Reset()
        {
            CurrentIndex = -1;
        }
    }
}