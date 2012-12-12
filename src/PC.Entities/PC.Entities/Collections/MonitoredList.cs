using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Collections
{
    /// <summary>
    /// List class specialisation that fires events when anything is added to or removed from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonitoredList<T> : List<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitoredList() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public MonitoredList(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public MonitoredList(int capacity) : base(capacity) { }

        /// <summary>
        /// Event fired when an item is added or inserted into the list
        /// </summary>
        public event EventHandler<MonitoredListItemEventArgs<T>> ItemAdded;

        /// <summary>
        /// Event fired when several items are added or inserted into the list
        /// </summary>
        public event EventHandler<MonitoredListItemsEventArgs<T>> ItemsAdded;

        /// <summary>
        /// Event fired when an item is removed from the list
        /// </summary>
        public event EventHandler<MonitoredListItemEventArgs<T>> ItemRemoved;

        /// <summary>
        /// Event fired when several items are removed from the list
        /// </summary>
        public event EventHandler<MonitoredListItemsEventArgs<T>> ItemsRemoved;

        /// <summary>
        /// Used internally to fire the item added event
        /// </summary>
        /// <param name="item"></param>
        private void FireItemAdded(T item)
        {
            if (ItemAdded != null)
                ItemAdded(this, new MonitoredListItemEventArgs<T>() { Item = item });
        }

        /// <summary>
        /// Used internally to fire the items added event
        /// </summary>
        /// <param name="items"></param>
        private void FireItemsAdded(IEnumerable<T> items)
        {
            if (ItemsAdded != null)
                ItemsAdded(this, new MonitoredListItemsEventArgs<T>() { Items = items });
        }

        
        /// <summary>
        /// Used internally to fire the item removed event
        /// </summary>
        /// <param name="item"></param>
        private void FireItemRemoved(T item)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, new MonitoredListItemEventArgs<T>() { Item = item });
        }

        /// <summary>
        /// Used internally to fire the items removed event
        /// </summary>
        /// <param name="items"></param>
        private void FireItemsRemoved(IEnumerable<T> items)
        {
            if (ItemsRemoved != null)
                ItemsRemoved(this, new MonitoredListItemsEventArgs<T>() { Items = items });
        }

        /// <summary>
        /// Override indexer to fire events
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new T this[int index]
        {
            get { return base[index]; }
            set
            {
                T removed = base[index];
                base[index] = value;
                FireItemRemoved(removed);
                FireItemAdded(value);
            }
        }

        /// <summary>
        /// Override add to fire events
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            base.Add(item);
            FireItemAdded(item);
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            FireItemsAdded(collection);
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            FireItemAdded(item);
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            FireItemsAdded(collection);
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new void Clear()
        {
            List<T> items = new List<T>(this);
            base.Clear();
            FireItemsRemoved(items);
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new bool Remove(T item)
        {
            bool result = base.Remove(item);
            if (result)
                FireItemRemoved(item);
            return result;
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new int RemoveAll(Predicate<T> match)
        {
            List<T> items = base.FindAll(match);
            int result = base.RemoveAll(match);
            if (result > 0)
                FireItemsRemoved(items);
            return result;
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new void RemoveAt(int index)
        {
            T removed = base[index];
            base.RemoveAt(index);
            FireItemRemoved(removed);
        }

        /// <summary>
        /// Override to fire monitor events
        /// </summary>
        public new void RemoveRange(int index, int count)
        {
            List<T> removed = base.GetRange(index, count);
            base.RemoveRange(index, count);
            FireItemsRemoved(removed);
        }
    }

    /// <summary>
    /// Used in MonitoredList class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonitoredListItemEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }

    /// <summary>
    /// Used in MonitoredList class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonitoredListItemsEventArgs<T> : EventArgs
    {
        public IEnumerable<T> Items { get; set; }
    }
}
