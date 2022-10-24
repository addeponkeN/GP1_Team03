using SS = System.SerializableAttribute;
using System.Collections.Generic;
using UnityEngine;

namespace Magnuth
{
    /// <summary>
    /// A custom linked list of nodes
    /// <br>Created by: Magnus Andersson</br>
	/// <br>Date: 2022-10-22</br>
	/// <br>Site: www.magnuth.com</br>
    /// </summary>
    [SS] public class NodeLink<T>
	{
		[SS] public class Node<T> : System.IEquatable<T> {
			public T Data = default;
			public Node<T> Left = null;
			public Node<T> Right = null;

			private EqualityComparer<T> _comparer = 
				EqualityComparer<T>.Default;

            public Node(Node<T> left, Node<T> right, T data){				
				Left = left;
				Right = right;
				Data = data;
            }
            public bool Equals(T other) => _comparer.Equals(Data, other);
            public override string ToString() => $"Node<{typeof(T).Name}>: {Data}";
        }
		
		private int _count = 0;
		private Node<T> _first = null;
		private Node<T> _last = null;

// PROPERTIES

		public int Count => _count;
		public Node<T> First => _first;
		public Node<T> Last => _last;

		private string ErrorHeader => 
			$"NodeLink<{typeof(T).Name}>:";

// GETTING DATA

		/// <summary>
		/// Returns node found at index
		/// </summary>
		public Node<T> GetAt(int index){
			if (index < 0 || index > _count - 1)
				return RangeError(index);
			
			if (index < _count * 0.5f)
				 return GetAtFirst(index);
			else return GetAtLast(index);
		}

        /// <summary>
        /// Returns node found at index, starting from first node
        /// </summary>
        private Node<T> GetAtFirst(int index){
			var node = _first;
				
			for (int i = 0; i < index; i++){
				if (node.Right == null)
					return IndexError(index, i);

				node = node.Right;
			}
			
			return node;
		}

        /// <summary>
        /// Returns node found at index, starting from last node
        /// </summary>
        private Node<T> GetAtLast(int index){
			var node = _last;
				
			for (int i = _count - 1; i > index; i--){
				if (node.Left == null)
					return IndexError(index, i);
				
				node = node.Left;
			}
			
			return node;
		}

// FINDING DATA

		/// <summary>
		/// Returns first node that matches data
		/// </summary>
		public Node<T> FindFirst(T data){
			var node = _first;
			
			for (int i = 0; i < _count; i++){
				if (node.Data.Equals(data))
					return node;
				
				if (node.Right == null)
					return IndexError(i, i);

				node = node.Right;
			}
			
			return MissingDataError(data);
		}

		/// <summary>
		/// Returns last node that matches data
		/// </summary>
		public Node<T> FindLast(T data){
			var node = _last;
			
			for (int i = _count - 1; i >= 0; i--){
				if (node.Data.Equals(data))
					return node;
				
				if (node.Left == null)
					return IndexError(i, i);

				node = node.Left;
			}
			
			return MissingDataError(data);
		}

// ADDING DATA

		/// <summary>
		/// Creates new node to beginning of link
		/// </summary>
		public void AddFirst(T data){
			var node = new Node<T>(
				null, _first, data
			);

            if (_first != null)
				_first.Left = node;

            if (_last == null)
				_last = node;

            _first = node;
			_count++;
		}
		
		/// <summary>
		/// Creates new node at end of link
		/// </summary>
		public void AddLast(T data){
			var node = new Node<T>(
				_last, null, data
			);

            if (_last != null)
				_last.Right = node;

            if (_first == null)
                _first = node;

            _last = node;
			_count++;
		}
		
		/// <summary>
		/// Creates new node to left of index
		/// </summary>
		public void AddAt(T data, int index){
			var node = GetAt(index);
            if (node == null) return;

			var node2 = new Node<T>(
				node.Left, node, data
			);
			
			node.Left = node2;
			_count++;
        }
		
		/// <summary>
		/// Replaces data at node index
		/// </summary>
		public void ReplaceAt(T data, int index){
			var node = GetAt(index);
			if (node == null) return;
			
			node.Data = data;
		}

// REMOVING DATA

		/// <summary>
		/// Removes node at beginning of link 
		/// </summary>
		public void RemoveFirst(){
			if (_first != null){
				var right = _first.Right;

                if (right != null)
					right.Left = null;

				_first.Left = null;
				_first.Right = null;
				_first = right;

				if (--_count > 0) return;
				_last = null;

            } else EmptyError();
        }

		/// <summary>
		/// Removes node at end of link
		/// </summary>
		public void RemoveLast(){
			if (_last != null){
				var left = _last.Left;

				if (left != null)
					left.Right = null;

				_last.Left = null;
				_last.Right = null;
				_last = left;

				if (--_count > 0) return;
                _first = null;

            } else EmptyError();
        }

		/// <summary>
		/// Removes node at index
		/// </summary>
		public void RemoveAt(int index){
            var node = GetAt(index);
            if (node == null) return;

            var left = node.Left;
            var right = node.Right;

            if (left != null)
                left.Right = right;

            if (right != null)
                right.Left = left; 
			
			node.Left  = null;
			node.Right = null;
			
			if (--_count > 0) return;
			_first = null;
			_last  = null;
        }

// ERRORS

        /// <summary>
        /// Outputs empty link error to console
        /// </summary>
        private void EmptyError(){
            var msg = $"The link is empty";
            Debug.LogError($"{ErrorHeader} {msg}");
        }

		/// <summary>
		/// Outputs out of range error to console
		/// </summary>
        private Node<T> RangeError(int index){
			var msg = $"Index {index}/{_count} is out of range.";
			Debug.LogError($"{ErrorHeader} {msg}");
			return null;
		}

        /// <summary>
        /// Outputs index error to console
        /// </summary>
        private Node<T> IndexError(int index, int current){
			var msg1 = $"Couldn't get node at index {index}.";
			var msg2 = $"Stopped at index {current}/{_count}.";
			Debug.LogError($"{ErrorHeader} {msg1} {msg2}");
			return null;
		}

        /// <summary>
        /// Outputs missing data error to console
        /// </summary>
        private Node<T> MissingDataError(T data){
			var msg = $"Couldn't find node containing {data}";
			Debug.LogError($"{ErrorHeader} {msg}");
			return null;
		}
    }
}