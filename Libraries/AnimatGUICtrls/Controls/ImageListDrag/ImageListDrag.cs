using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AnimatGuiCtrls.Controls
{
	/// <summary>
	/// Provides the ability to create 32-bit alpha drag images
	/// using the ImageList drag functionality in .NET.
	/// </summary>
	public class ImageListDrag : IDisposable
	{

		#region Unmanaged Code
		[StructLayoutAttribute(LayoutKind.Sequential)]
		private struct POINTAPI
		{
			public int X;
			public int Y;

			public override string ToString()
			{
				return String.Format("{0} X={1},Y={2}",
					this.GetType().FullName, this.X, this.Y);
			}
		}

		/*
		[StructLayoutAttribute(LayoutKind.Sequential)]
			private struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public override string ToString()
			{
				return String.Format("{0} Left={1},Top={2},Right={3},Bottom={4}",
					this.GetType().FullName, this.Left, this.Top, this.Right, this.Bottom);
			}
		}
		*/

		[DllImport("comctl32")]
		private static extern int ImageList_BeginDrag(
			IntPtr himlTrack, 
			int iTrack, 
			int dxHotspot, 
			int dyHotspot);
		[DllImport("comctl32")]
		private static extern void ImageList_EndDrag();
		[DllImport("comctl32")]
		private static extern int ImageList_DragEnter(
			IntPtr hwndLock, 
			int X, 
			int Y);
		[DllImport("comctl32")]
		private static extern int ImageList_DragLeave (
			IntPtr hwndLock );
		[DllImport("comctl32")]
		private static extern int ImageList_DragMove (
			int X, 
			int Y);
		[DllImport("comctl32")]
		private static extern int ImageList_SetDragCursorImage (
			IntPtr himlDrag, 
			int iDrag, 
			int dxHotspot, 
			int dyHotspot);

		[DllImport("comctl32")]
		private static extern int ImageList_DragShowNolock (
			int fShow );
		[DllImport("comctl32")]
		private static extern int ImageList_GetDragImage (
			ref POINTAPI ppt, 
			ref POINTAPI pptHotspot);


		/*
		[DllImport("user32")]
		static extern int GetWindowRect (
			IntPtr hwnd, 
			ref RECT rect);
		Private Declare Function GetCursorPos Lib "user32" (lpPoint As POINTAPI) As Long
		*/

		#endregion

		#region Member Variables
		/// <summary>
		/// The <code>ImageList</code> to use to source the drag-drop image from.
		/// </summary>
		private System.Windows.Forms.ImageList iml = null;
		/// <summary>
		/// The <code>Control</code> which owns this class.
		/// </summary>
		private Control owner = null;
		/// <summary>
		/// The Window handle which we're dragging over.
		/// </summary>
		private IntPtr hWndLast = IntPtr.Zero;
		/// <summary>
		/// Whether dragging is occurring or not.
		/// </summary>
		private bool inDrag = false;
		/// <summary>
		/// Whether we have suspended image
		/// dragging and need to start it again when the
		/// cursor next moves.
		/// </summary>
		private bool startDrag = false;
		/// <summary>
		/// Whether this class has been disposed or not.
		/// </summary>
		private bool disposed = false;
		#endregion

		#region API
		/// <summary>
		/// Gets/sets the ImageList used to source drag images.
		/// </summary>
		public System.Windows.Forms.ImageList Imagelist
		{
			get
			{
				return this.iml;
			}
			set
			{
				this.iml = value;
			}
		}

		/// <summary>
		/// Gets/sets the Owning control or form for this object.
		/// </summary>
		public Control Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		/// <summary>
		/// Starts a dragging operation which will use
		/// an ImageList to create a drag image and defaults
		/// the position of the image to the cursor's drag
		/// point.
		/// </summary>
		/// <param name="imageIndex">The index of the image in
		/// the ImageList to use for the drag image.</param>
		public void StartDrag(
			int imageIndex 
			)
		{
			StartDrag(imageIndex, 0, 0);
		}
		
		/// <summary>
		/// Starts a dragging operation which will use
		/// an ImageList to create a drag image and allows
		/// the offset of the Image from the drag position
		/// to be specified.
		/// </summary>
		/// <param name="imageIndex">The index of the image in
		/// the ImageList to use for the drag image.</param>
		/// <param name="xOffset">The horizontal offset of the drag image
		/// from the drag position.  Negative values move the image
		/// to the right of the cursor, positive values move it
		/// to the left.</param>
		/// <param name="yOffset">The vertical offset of the drag image
		/// from the drag position. Negative values move the image
		/// below the cursor, positive values move it above.</param>
		public void StartDrag(
			int imageIndex,
			int xOffset,
			int yOffset
			)
		{
			int res = 0;
			CompleteDrag();
			res = ImageList_BeginDrag(
				iml.Handle, imageIndex, xOffset, yOffset);
			if (res != 0)
			{
				this.inDrag = true;
				this.startDrag = true;
			}
		}

		/// <summary>
		/// Shows the ImageList drag image at the current
		/// dragging position.
		/// </summary>
		public void DragDrop()
		{
			IntPtr hWndParent = IntPtr.Zero;
			Point dst = new Point();
			
			if (this.inDrag)
			{
				dst = Cursor.Position;
				if (this.owner != null)
				{
					// Position relative to owner:
					dst = this.owner.PointToClient(dst);
				}

				if (this.startDrag)
				{
					this.hWndLast = (this.owner == null ? IntPtr.Zero : this.owner.Handle);
					ImageList_DragEnter(
						hWndLast, 
						dst.X, dst.Y);
					this.startDrag = false;
				}

				ImageList_DragMove(dst.X, dst.Y);
			}     
		}

		/// <summary>
		/// Completes a drag operation.
		/// </summary>
		public void CompleteDrag()
		{
			if (this.inDrag)
			{
				ImageList_EndDrag();
				ImageList_DragLeave(this.hWndLast);
				this.hWndLast = IntPtr.Zero;
				this.inDrag = false;
			}
		}

		/// <summary>
		/// Shows or hides the drag image.  This is used to prevent
		/// painting problems if the area under the drag needs to
		/// be repainted.
		/// </summary>
		/// <param name="state">True to hide the drag image and
		/// allow repainting, False to show the drag image.</param>
		public void HideDragImage(bool state)
		{
			if (this.inDrag)
			{
				if (state)
				{
					ImageList_DragLeave(this.hWndLast);
					this.startDrag = true;
				}
				else
				{
					DragDrop();
				}
			}
		}
		#endregion

		#region Constructor, Dispose, Finalise
		/// <summary>
		/// Constructs a new instance of the ImageListDrag class.
		/// </summary>
		public ImageListDrag()
		{			
			// Intentionally blank
		}

		/// <summary>
		/// Clears up any resources associated with this object.
		/// Note there are only resources associated when there is
		/// a drag operation in effect.
		/// </summary>
		public void Dispose()
		{
			if (!this.disposed)
			{
				CompleteDrag();
			}
			this.disposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Finalize calls Dispose if it hasn't already been called.
		/// </summary>
		~ImageListDrag()
		{
			if (!this.disposed)
			{
				Dispose();
			}
		}
		#endregion
	}
}
