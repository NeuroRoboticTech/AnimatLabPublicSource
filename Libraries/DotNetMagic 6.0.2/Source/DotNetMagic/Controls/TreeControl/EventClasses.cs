// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Crownwood.DotNetMagic.Controls
{	
	/// <summary>
	/// Provide TextBox data for an event.
	/// </summary>
	public class LabelControlEventArgs : EventArgs
	{
		// Instance fields
		private TextBox _textBox;
		
		/// <summary>
		/// Initialize a new instance of the LabelControlEventArgs class.
		/// </summary>
		/// <param name="textBox">Instance about to be edited.</param>
		public LabelControlEventArgs(TextBox textBox)
		{
			_textBox = textBox;
		}
		
		/// <summary>
		/// Gets access to the TextBox used for label editing.
		/// </summary>
		public TextBox TextBox
		{
			get { return _textBox; }
		}
	}
	
	/// <summary>
	/// Provides Node data for event.
	/// </summary>
	public class NodeEventArgs : EventArgs	
	{
		// Instance fields
		private Node _node;

		/// <summary>
		/// Initialize a new instance of the =NodeEventArgs class.
		/// </summary>
		/// <param name="node">Node instance associated with event.</param>
		public NodeEventArgs(Node node)
		{
			_node = node;
		}
		
		/// <summary>
		/// Gets the Node instance associated with event.
		/// </summary>
		public Node Node
		{
			get { return _node; }
		}
	}

	/// <summary>
	/// Provides Node data for a cancelable event.
	/// </summary>
	public class CancelNodeEventArgs : CancelEventArgs	
	{
		// Instance fields
		private Node _node;

		/// <summary>
		/// Initialize a new instance of the CancelNodeEventArgs class.
		/// </summary>
		/// <param name="node">Node instance associated with event.</param>
		public CancelNodeEventArgs(Node node)
		{
			_node = node;
		}
		
		/// <summary>
		/// Gets the Node instance associated with event.
		/// </summary>
		public Node Node
		{
			get { return _node; }
		}
	}
	
	/// <summary>
	/// Provides Node and Label text data for a cancelable event.
	/// </summary>
	public class LabelEditEventArgs : CancelNodeEventArgs
	{
		// Instance fields
		private string _label;
		
		/// <summary>
		/// Initialize a new instance of the LabelEditEventArgs class.
		/// </summary>
		/// <param name="node">Node instance associated with event.</param>
		/// <param name="label">Initial label to use when editing.</param>
		public LabelEditEventArgs(Node node, string label)
			: base(node)
		{
			_label = label;
		}
		
		/// <summary>
		/// Gets and sets the initial label text for editing.
		/// </summary>
		public string Label
		{
			get { return _label; }
			set { _label = value; }
		}
	}

	/// <summary>
	/// Provides Node and drag effects values for a cancelable event.
	/// </summary>
	public class StartDragEventArgs : CancelNodeEventArgs
	{
		// Instance fields
		private object _object;
		private DragDropEffects _effect;
		
		/// <summary>
		/// Initialize a new instance of the LabelEditEventArgs class.
		/// </summary>
		/// <param name="node">Node instance associated with event.</param>
		/// <param name="effect">DragDropEffects for operation.</param>
		public StartDragEventArgs(Node node, DragDropEffects effect)
			: base(node)
		{
			_object = node;
			_effect = effect;
		}
		
		/// <summary>
		/// Gets and sets the initial label text for editing.
		/// </summary>
		public DragDropEffects Effect
		{
			get { return _effect; }
			set { _effect = value; }
		}

		/// <summary>
		/// Gets and sets the object to be drag and dropped.
		/// </summary>
		public object Object
		{
			get { return _object; }
			set { _object = value; }
		}
	}

	/// <summary>
	/// Signature of handler used for label editing.
	/// </summary>
	public delegate void LabelControlEventHandler(TreeControl tc, LabelControlEventArgs e);

	/// <summary>
	/// Signature of handler used for node event.
	/// </summary>
	public delegate void NodeEventHandler(TreeControl tc, NodeEventArgs e);

	/// <summary>
	/// Signature of handler used for cancelable node event.
	/// </summary>
	public delegate void CancelNodeEventHandler(TreeControl tc, CancelNodeEventArgs e);

	/// <summary>
	/// Signature of handler used for cancelable label editing event.
	/// </summary>
	public delegate void LabelEditEventHandler(TreeControl tc, LabelEditEventArgs e);
	
	/// <summary>
	/// Signature of handler used for cancelable attempt to start dragging a node.
	/// </summary>
	public delegate void StartDragEventHandler(TreeControl tc, StartDragEventArgs e);

	/// <summary>
	/// Signature of handler for Node specific drag and drop events.
	/// </summary>
	public delegate void NodeDragDropEventHandler(TreeControl tc, Node n, DragEventArgs e);
}
