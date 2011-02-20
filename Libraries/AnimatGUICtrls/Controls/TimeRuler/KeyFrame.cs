using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace AnimatGuiCtrls.Controls
{
	/// <summary>
	/// Summary description for KeyFrame.
	/// </summary>
	public class KeyFrame
	{
		#region Enumerations

		public enum enumKeyFrameType
		{
			Snapshot = 0,
			Video = 1, 
			Playback = 2,
			CurrentFrame = 3
		}

		public enum enumFrameTimeType
		{
			StartTime = 0,
			EndTime = 1
		}

		#endregion

		#region Attributes

		protected String _ID = "";
		protected long _StartMillisecond = 0;
		protected long _EndMillisecond = 0;
		protected System.Drawing.Color _Color = Color.FromArgb(255, 100, 100);
		protected System.Drawing.Color _SelectedColor = Color.FromArgb(255, 0, 0);
		protected bool _Selected = false;
		protected int _BarWidth = 3;
		protected bool _Playing = false;

		protected Pen _NonSelPen;
		protected Pen _SelPen;
		protected Pen _DrawingPen;

		protected Brush _NonSelBrush;
		protected Brush _SelBrush;
		protected Brush _DrawingBrush;

		#endregion

		#region Properties

		[Description("Specifies the ID value associated with this Keyframe.")]
		[Category("Ruler")]
		public virtual String ID
		{
			get { return _ID; }
			set 
			{
				_ID = value;
			}
		}

		[Description("This tells the type type of keyframe this is.")]
		[Category("KeyFrame")]
		public virtual enumKeyFrameType KeyFrameType
		{ 
			get { return enumKeyFrameType.Snapshot; }
		}


		[Description("This sets the color of the Keyframe bars.")]
		[Category("Ruler")]
		public virtual System.Drawing.Color Color
		{
			get { return _Color; }
			set 
			{
				_Color = value;
			}
		}

		[Description("This sets the color of selected Keyframe bars.")]
		[Category("Ruler")]
		public virtual System.Drawing.Color SelectedColor
		{
			get { return _SelectedColor; }
			set 
			{
				_SelectedColor = value;
			}
		}

		[Description("This sets the width of the bar that is draw for the keyframes.")]
		[Category("Ruler")]
		public virtual int BarWidth
		{
			get { return _BarWidth; }
			set 
			{
				if(value <= 0)
					throw new System.Exception("You can not set the bar width to be less than or equal to 0.");

				_BarWidth = value;
			}
		}

		[Description("This is the millisecond where this keyframe is located or starts. This depends on the type of keyframe.")]
		[Category("KeyFrame")]
		public virtual long StartMillisecond
		{ 
			get { return _StartMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The start millisecond time for a keyframe can not be less than zero.");

				_StartMillisecond = value;
				_EndMillisecond = value;
			}
		}

		[Description("This is the millisecond where this keyframe is located or starts. This depends on the type of keyframe.")]
		[Category("KeyFrame")]
		public virtual long EndMillisecond
		{ 
			get { return _EndMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The end millisecond time for a keyframe can not be less than zero.");

				_StartMillisecond = value;
				_EndMillisecond = value;
			}
		}

		[Description("This determines if this keyframe is currently selected.")]
		[Category("KeyFrame")]
		public virtual bool Selected
		{ 
			get { return _Selected; }
			set 
			{
				_Selected = value;

				if(_Selected)
				{
					_DrawingBrush = _SelBrush;
					_DrawingPen = _SelPen;
				}
				else
				{
					_DrawingBrush = _NonSelBrush;
					_DrawingPen = _NonSelPen;
				}
			}
		}
		
		[Description("This is used by the simulation controller to keep track of whether a video is playing or not.")]
		[Category("Ruler")]
		public virtual bool Playing
		{
			get { return _Playing; }
			set 
			{
				_Playing = value;
			}
		}

		#endregion

		#region Methods

		public KeyFrame()
		{
			CreatePens();
		}

		public KeyFrame(long lStart)
		{
			SetTimes(lStart, lStart);

			CreatePens();
		}

		protected void CreatePens()
		{
			_NonSelBrush = new SolidBrush(this.Color);
			_SelBrush = new SolidBrush(this.SelectedColor);
			_DrawingBrush = _NonSelBrush;

			_NonSelPen = new Pen(_NonSelBrush, _BarWidth);
			_SelPen = new Pen(_SelBrush, _BarWidth);
			_DrawingPen = _NonSelPen;
		}

		public virtual void SetTimes(long lStart)
		{
			if(lStart < 0)
				throw new System.Exception("The start millisecond time for a keyframe can not be less than zero.");
			
			_StartMillisecond = lStart;
			_EndMillisecond = lStart;
		}

		public virtual void SetTimes(long lStart, long lEnd)
		{
			if(lStart < 0 || lEnd < 0)
				throw new System.Exception("The start/end millisecond time for a keyframe can not be less than zero.");
			
			_StartMillisecond = lStart;
			_EndMillisecond = lStart;
		}

		public virtual void Draw(Graphics g, TimeRuler ruler)
		{
			if (ruler.Orientation == enumOrientation.orHorizontal)
			{
				int x = ruler.ScaleValueToPixel((double) _StartMillisecond) - 1;
				int y1 = ruler.HeaderOffset, y2 = ruler.Height;
				g.DrawLine(_DrawingPen, x, y1, x, y2);

				y1 = ruler.HeaderOffset/2;
				y2 = ruler.HeaderOffset - y1 - 1;
				x = ruler.ScaleValueToPixel((double) _StartMillisecond) - (y2/2) - 1;
				g.DrawEllipse(new Pen(new SolidBrush(ruler.ForeColor)), x, y1, y2, y2);
				g.FillEllipse(_DrawingBrush, x, y1, y2, y2);
			}
			else
			{
				int y = ruler.ScaleValueToPixel((double) _StartMillisecond) - 1;
				int x1 = ruler.HeaderOffset, x2 = ruler.Height;
				g.DrawLine(_DrawingPen, x1, y, x2, y);

				x1 = ruler.HeaderOffset/2;
				x2 = ruler.HeaderOffset - x1 - 1;
				y = ruler.ScaleValueToPixel((double) _StartMillisecond) - (x2/2) - 1;
				g.DrawEllipse(new Pen(new SolidBrush(ruler.ForeColor)), x1, y, x2, x2);
				g.FillEllipse(_DrawingBrush, x1, y, x2, x2);
			}
		}

		public virtual bool Overlaps(KeyFrame keyTest, ref enumFrameTimeType iTimeType)
		{
			if( (keyTest.StartMillisecond >= this.StartMillisecond) && (keyTest.StartMillisecond <= this.EndMillisecond) ||
				  (this.StartMillisecond >= keyTest.StartMillisecond) && (this.StartMillisecond <= keyTest.EndMillisecond) )
			{
				iTimeType = enumFrameTimeType.StartTime;
				return true;
			}

			if( (keyTest.EndMillisecond >= this.StartMillisecond) && (keyTest.EndMillisecond <= this.EndMillisecond) ||
				  (this.EndMillisecond >= keyTest.StartMillisecond) && (this.EndMillisecond <= keyTest.EndMillisecond) )
			{
				iTimeType = enumFrameTimeType.EndTime;
				return true;
			}

			return false;
		}

		public virtual bool Overlaps(long lStart, long lEnd, ref enumFrameTimeType iTimeType)
		{
			if( (lStart >= this.StartMillisecond) && (lStart <= this.EndMillisecond) ||
				  (this.StartMillisecond >= lStart) && (this.StartMillisecond <= lEnd) )
			{
				iTimeType = enumFrameTimeType.StartTime;
				return true;
			}

			if( (lEnd >= this.StartMillisecond) && (lEnd <= this.EndMillisecond) ||
				  (this.StartMillisecond >= lEnd) && (this.EndMillisecond <= lEnd) )
			{
				iTimeType = enumFrameTimeType.EndTime;
				return true;
			}

			return false;
		}

		public virtual bool IsHandleClick(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler, long lMillisecond)
		{
			int y1 = ruler.HeaderOffset/2;
			int y2 = ruler.HeaderOffset;
			int x1 = ruler.ScaleValueToPixel((double) lMillisecond) - (y2/2) - 1;
			int x2 = x1 + y2;
			bool bRetVal = false;

			if (ruler.Orientation == enumOrientation.orHorizontal)
			{
				if( (e.X >= x1) && (e.X <= x2) && (e.Y >= y1) && (e.Y <=y2) )
					bRetVal = true;
			}
			else
			{
				if( (e.Y >= x1) && (e.Y <= x2) && (e.X >= y1) && (e.X <=y2) )
					bRetVal = true;
			}

			return bRetVal;		
		}

		public virtual int DistanceFromHandle(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler, long lMillisecond)
		{
			int iMousePosition = ((ruler.Orientation == enumOrientation.orHorizontal) ? e.X : e.Y);
			int x = ruler.ScaleValueToPixel((double) lMillisecond);
			
			return Math.Abs(x-iMousePosition);		
		}

		public virtual bool IsHandleClick(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{
			if(IsHandleClick(e, ruler, _StartMillisecond)) return true;
			if(IsHandleClick(e, ruler, _EndMillisecond)) return true;
			return false;
		}

		public virtual bool CanBeMoved(TimeRuler ruler) 
		{
			if( (_StartMillisecond <= ruler.ActualMillisecond) || (_EndMillisecond <= ruler.ActualMillisecond) )
				return false;

			return true;
		}

		public virtual void StartMove(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{}

		public virtual void EndMove(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{}

		public virtual void MoveFrame(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{
			int iMousePosition = ((ruler.Orientation == enumOrientation.orHorizontal) ? e.X : e.Y);
			
			long lNewMillisecond = (long) ruler.PixelToScaleValue(iMousePosition);

			if(lNewMillisecond > ruler.ActualMillisecond && 
				 _StartMillisecond > ruler.ActualMillisecond && 
				!ruler.KeyFrames.Overlaps(lNewMillisecond, lNewMillisecond, this))
			{
				_StartMillisecond = lNewMillisecond;
				_EndMillisecond = _StartMillisecond;
	
				ruler.RedrawBitmap();
			}
		}

		public virtual void MoveFrame(long lStart, long lEnd, TimeRuler ruler)
		{

			if(lStart > ruler.ActualMillisecond && 
				_StartMillisecond > ruler.ActualMillisecond && 
				!ruler.KeyFrames.Overlaps(lStart, lStart, this))
			{
				_StartMillisecond = lStart;
				_EndMillisecond = _StartMillisecond;
	
				ruler.RedrawBitmap();
			}
		}

		#endregion

	}
}
