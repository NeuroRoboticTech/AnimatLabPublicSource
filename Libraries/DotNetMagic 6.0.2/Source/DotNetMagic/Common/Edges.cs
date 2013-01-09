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

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Stores a value for each edge of a rectangle.
	/// </summary>
	public struct Edges
	{	
		/// <summary>
		/// An empty instance of the Edges class.
		/// </summary>
		public static readonly Edges Empty = new Edges(0, 0, 0, 0);

		// Instance fields
		private int _left;
		private int _top;
		private int _right;
		private int _bottom;

		/// <summary>
		/// Initialize a new instance of the Edges structure.
		/// </summary>
		/// <param name="left">Initial value for Left edge.</param>
		/// <param name="top">Initial value for Top edge.</param>
		/// <param name="right">Initial value for Right edge.</param>
		/// <param name="bottom">Initial value for Bottom edge.</param>
		public Edges(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		/// <summary>
		/// Gets and sets the value of the top edge.
		/// </summary>
		[Description("left edge.")]
		[DefaultValue(0)]
		public int Left
		{
			get { return _left; }
			set { _left = value; }
		}

		/// <summary>
		/// Gets and sets the value of the top edge.
		/// </summary>
		[Description("Top edge.")]
		[DefaultValue(0)]
		public int Top
		{
			get { return _top; }
			set { _top = value; }
		}

		/// <summary>
		/// Gets and sets the value of the top edge.
		/// </summary>
		[Description("Right edge.")]
		[DefaultValue(0)]
		public int Right
		{
			get { return _right; }
			set { _right = value; }
		}

		/// <summary>
		/// Gets and sets the value of the top edge.
		/// </summary>
		[Description("Bottom edge.")]
		[DefaultValue(0)]
		public int Bottom
		{
			get { return _bottom; }
			set { _bottom = value; }
		}
	}
}
