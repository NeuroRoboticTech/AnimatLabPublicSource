// A ComboBox with a TreeView Drop-Down
// Bradley Smith - 2010/11/04

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AnimatGuiCtrls.Controls
{

    /// <summary>
    /// Represents a collection of ComboTreeNode objects contained within a node or a ComboTreeBox control. 
    /// Supports change notification through INotifyCollectionChanged. Implements the non-generic IList to 
    /// provide design-time support.
    /// </summary>
    public class ComboTreeNodeCollection : IList<ComboTreeNode>, IList, INotifyCollectionChanged
    {

        private List<ComboTreeNode> innerList;
        private ComboTreeNode node;

        /// <summary>
        /// Initalises a new instance of ComboTreeNodeCollection and associates it with the specified ComboTreeNode.
        /// </summary>
        /// <param name="node"></param>
        public ComboTreeNodeCollection(ComboTreeNode node)
        {
            innerList = new List<ComboTreeNode>();
            this.node = node;
        }

        /// <summary>
        /// Gets the node with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ComboTreeNode this[string name]
        {
            get
            {
                foreach (ComboTreeNode o in this)
                {
                    if (Object.Equals(o.Name, name)) return o;
                }

                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Creates a node and adds it to the collection.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public ComboTreeNode Add(string text)
        {
            ComboTreeNode item = new ComboTreeNode(text);
            Add(item);
            return item;
        }

        /// <summary>
        /// Creates a node and adds it to the collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public ComboTreeNode Add(string name, string text)
        {
            ComboTreeNode item = new ComboTreeNode(name, text);
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds a range of ComboTreeNode to the collection.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<ComboTreeNode> items)
        {
            foreach (ComboTreeNode item in items)
            {
                innerList.Add(item);
                item.Parent = node;
                item.Nodes.CollectionChanged += CollectionChanged;
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the collection contains a node with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            foreach (ComboTreeNode o in this)
            {
                if (Object.Equals(o.Name, name)) return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the node with the specified name from the collection.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            for (int i = 0; i < innerList.Count; i++)
            {
                if (Object.Equals(innerList[i].Name, name))
                {
                    ComboTreeNode item = innerList[i];
                    item.Nodes.CollectionChanged -= CollectionChanged;
                    innerList.RemoveAt(i);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the index of the node with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int IndexOf(string name)
        {
            for (int i = 0; i < innerList.Count; i++)
            {
                if (Object.Equals(innerList[i].Name, name)) return i;
            }

            return -1;
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null) CollectionChanged(this, e);
        }

        /// <summary>
        /// Sorts the collection and its entire sub-tree using the specified comparer.
        /// </summary>
        /// <param name="comparer"></param>
        internal void Sort(IComparer<ComboTreeNode> comparer)
        {
            if (comparer == null) comparer = Comparer<ComboTreeNode>.Default;
            SortInternal(comparer);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Recursive helper method for Sort(IComparer&lt;ComboTreeNode&gt;).
        /// </summary>
        /// <param name="comparer"></param>
        private void SortInternal(IComparer<ComboTreeNode> comparer)
        {
            innerList.Sort(comparer);
            foreach (ComboTreeNode node in innerList)
            {
                node.Nodes.Sort(comparer);
            }
        }

        #region ICollection<ComboTreeNode> Members

        /// <summary>
        /// Adds a node to the collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(ComboTreeNode item)
        {
            innerList.Add(item);
            item.Parent = node;
            item.Nodes.CollectionChanged += CollectionChanged;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            foreach (ComboTreeNode item in innerList) item.Nodes.CollectionChanged -= CollectionChanged;
            innerList.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the collection contains the specified node.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ComboTreeNode item)
        {
            return innerList.Contains(item);
        }

        /// <summary>
        /// Gets the number of nodes in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return innerList.Count;
            }
        }

        /// <summary>
        /// Copies all the nodes from the collection to a compatible array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(ComboTreeNode[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified node from the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(ComboTreeNode item)
        {
            if (innerList.Remove(item))
            {
                item.Nodes.CollectionChanged -= CollectionChanged;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                return true;
            }

            return false;
        }

        bool ICollection<ComboTreeNode>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<ComboTreeNode> Members

        /// <summary>
        /// Returns an enumerator which can be used to cycle through the nodes in the collection (non-recursive).
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ComboTreeNode> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region IList<ComboTreeNode> Members

        /// <summary>
        /// Gets or sets the node at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ComboTreeNode this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                ComboTreeNode oldItem = innerList[index];
                innerList[index] = value;
                value.Parent = node;
                value.Nodes.CollectionChanged += CollectionChanged;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
            }
        }

        /// <summary>
        /// Returns the index of the specified node.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(ComboTreeNode item)
        {
            return innerList.IndexOf(item);
        }

        /// <summary>
        /// Inserts a node into the collection at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, ComboTreeNode item)
        {
            innerList.Insert(index, item);
            item.Parent = node;
            item.Nodes.CollectionChanged += CollectionChanged;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        /// <summary>
        /// Removes the node at the specified index from the collection.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            ComboTreeNode item = innerList[index];
            item.Nodes.CollectionChanged -= CollectionChanged;
            innerList.RemoveAt(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        #endregion

        #region IEnumerable Members (implemented explicitly)

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region IList Members (implemented explicitly)

        int IList.Add(object value)
        {
            Add((ComboTreeNode)value);
            return Count - 1;
        }

        bool IList.Contains(object value)
        {
            return Contains((ComboTreeNode)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((ComboTreeNode)value);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (ComboTreeNode)value);
        }

        bool System.Collections.IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void IList.Remove(object value)
        {
            Remove((ComboTreeNode)value);
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (ComboTreeNode)value;
            }
        }

        #endregion

        #region ICollection Members (implemented explicitly)

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)innerList).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return ((ICollection)innerList).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return ((ICollection)innerList).SyncRoot;
            }
        }

        #endregion

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Fired when the collection (sub-tree) changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }

}
