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
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Determines the indent padding for the TreeControl.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class IndentPaddingEdges
	{
		// Instance fields
		private Edges _edges;

		/// <summary>
		/// Occures when any indent value changes.
		/// </summary>
		public event EventHandler IndentChanged;

		/// <summary>
		/// Initialize a new instance of the IndentPaddingEdges class.
		/// </summary>
		public IndentPaddingEdges()
		{
			// Create the actual storage
			_edges = new Edges();

			// Default all the edges
			ResetLeft();
			ResetTop();
			ResetRight();
			ResetBottom();
		}

		/// <summary>
		/// Gets and sets the indent for the left edge.
		/// </summary>
		[Description("Number of pixels to indent the left edge.")]
		[DefaultValue(0)]
		public int Left
		{
			get { return _edges.Left; }
			
			set 
			{ 
				// Cannot set a negative edge indent
				if (value >= 0)
				{
					if (_edges.Left != value)
					{
						_edges.Left = value; 
						OnIndentChanged();
					}
				}
			}
		}

		/// <summary>
		/// Resets the Left property to its default value.
		/// </summary>
		public void ResetLeft()
		{
			Left = 0;
		}

		/// <summary>
		/// Gets and sets the indent for the top edge.
		/// </summary>
		[Description("Number of pixels to indent the top edge.")]
		[DefaultValue(0)]
		public int Top
		{
			get { return _edges.Top; }
			
			set 
			{ 
				// Cannot set a negative edge indent
				if (value >= 0)
				{
					if (_edges.Top != value)
					{
						_edges.Top = value; 
						OnIndentChanged();
					}
				}
			}
		}

		/// <summary>
		/// Resets the Top property to its default value.
		/// </summary>
		public void ResetTop()
		{
			Top = 0;
		}

		/// <summary>
		/// Gets and sets the indent for the right edge.
		/// </summary>
		[Description("Number of pixels to indent the right edge.")]
		[DefaultValue(0)]
		public int Right
		{
			get { return _edges.Right; }
			
			set 
			{ 
				// Cannot set a negative edge indent
				if (value >= 0)
				{
					if (_edges.Right != value)
					{
						_edges.Right = value; 
						OnIndentChanged();
					}
				}
			}
		}

		/// <summary>
		/// Resets the Right property to its default value.
		/// </summary>
		public void ResetRight()
		{
			Right = 0;
		}

		/// <summary>
		/// Gets and sets the indent for the bottom edge.
		/// </summary>
		[Description("Number of pixels to indent the bottom edge.")]
		[DefaultValue(0)]
		public int Bottom
		{
			get { return _edges.Bottom; }
			
			set 
			{ 
				// Cannot set a negative edge indent
				if (value >= 0)
				{
					if (_edges.Bottom != value)
					{
						_edges.Bottom = value; 
						OnIndentChanged();
					}
				}
			}
		}

		/// <summary>
		/// Resets the Bottom property to its default value.
		/// </summary>
		public void ResetBottom()
		{
			Bottom = 0;
		}

		/// <summary>
		/// Returns a String that represents the current IndentPaddingEdges.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			// Return nothing so it appears as blank in property window
			return string.Empty;
		}

		/// <summary>
		/// Raises the IndentChanged event.
		/// </summary>
		protected virtual void OnIndentChanged()
		{
			if (IndentChanged != null)
				IndentChanged(this, EventArgs.Empty);
		}
	}
}
