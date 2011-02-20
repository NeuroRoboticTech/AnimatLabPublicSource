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
	/// Summary description for KeyFrameVideoRange.
	/// </summary>
	public class KeyFrameVideoRange : KeyFrame
	{

		#region Properties

		public override enumKeyFrameType KeyFrameType
		{ 
			get { return enumKeyFrameType.Video; }
		}

		public override long StartMillisecond
		{ 
			get { return _StartMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The start millisecond time for a keyframe can not be less than zero.");

				if(value > _EndMillisecond)
					throw new System.Exception("The start millisecond time for a keyframe can not be greater than the end time.");

				_StartMillisecond = value;
			}
		}

		public override long EndMillisecond
		{ 
			get { return _EndMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The end millisecond time for a keyframe can not be less than zero.");

				if(value < _StartMillisecond)
					throw new System.Exception("The end millisecond time for a keyframe can not be less than the start time.");

				_EndMillisecond = value;
			}
		}

		#endregion
	
		#region Methods
	
		public KeyFrameVideoRange()
		{
			_Color = Color.FromArgb(100, 255, 100);
			_SelectedColor = Color.FromArgb(0, 255, 0);
			CreatePens();
		}

		public KeyFrameVideoRange(long lStart, long lEnd)
		{
			SetTimes(lStart, lEnd);
			_Color = Color.FromArgb(100, 255, 100);
			_SelectedColor = Color.FromArgb(0, 255, 0);
			CreatePens();
		}

		public override void SetTimes(long lStart, long lEnd)
		{
			if(lStart < 0 || lEnd < 0)
				throw new System.Exception("The start/end millisecond time for a keyframe can not be less than zero.");
			
			long lTemp;
			//If the start and end times are swapped then swap them back correctly.
			if(lEnd < lStart)
			{
				lTemp = lStart;
				lStart = lEnd;
				lEnd = lTemp;
			}

			_StartMillisecond = lStart;
			_EndMillisecond = lEnd;
		}


		public override void Draw(Graphics g, TimeRuler ruler)
		{
			if (ruler.Orientation == enumOrientation.orHorizontal)
			{
				int x1 = ruler.ScaleValueToPixel((double) _StartMillisecond) - 1;
				int y1 = ruler.HeaderOffset, y2 = ruler.Height;
				g.DrawLine(_DrawingPen, x1, y1, x1, y2);

				x1 = ruler.ScaleValueToPixel((double) _EndMillisecond) - 1;
				y1 = ruler.HeaderOffset;
				y2 = ruler.Height;
				g.DrawLine(_DrawingPen, x1, y1, x1, y2);

				y1 = ruler.HeaderOffset/2;
				y2 = ruler.HeaderOffset - y1;
				x1 = ruler.ScaleValueToPixel((double) _StartMillisecond) - 2;
				int x2 = ruler.ScaleValueToPixel((double) _EndMillisecond) ;
				x2 = x2 - x1;
				g.DrawRectangle(new Pen(new SolidBrush(ruler.ForeColor)), x1, y1, x2, y2);
				g.FillRectangle(_DrawingBrush, x1, y1, x2, y2);

				y1 = ruler.HeaderOffset/2;
				y2 = ruler.HeaderOffset - y1;
				x1 = ruler.ScaleValueToPixel((double) _StartMillisecond) - (y2/2) - 1;
				g.FillRectangle(new SolidBrush(ruler.ForeColor), x1, y1, y2, y2);

				y1 = ruler.HeaderOffset/2;
				y2 = ruler.HeaderOffset - y1;
				x1 = ruler.ScaleValueToPixel((double) _EndMillisecond) - (y2/2) - 1;
				g.FillRectangle(new SolidBrush(ruler.ForeColor), x1, y1, y2, y2);
			}
			else
			{
				int y1 = ruler.ScaleValueToPixel((double) _StartMillisecond) - 1;
				int x1 = ruler.HeaderOffset, x2 = ruler.Width;
				g.DrawLine(_DrawingPen, x1, y1, x2, y1);

				y1 = ruler.ScaleValueToPixel((double) _EndMillisecond) - 1;
				x1 = ruler.HeaderOffset;
				x2 = ruler.Height;
				g.DrawLine(_DrawingPen, x1, y1, x2, y1);

				x1 = ruler.HeaderOffset/2;
				x2 = ruler.HeaderOffset - x1;
				y1 = ruler.ScaleValueToPixel((double) _StartMillisecond) - 2;
				int y2 = ruler.ScaleValueToPixel((double) _EndMillisecond) ;
				y2 = y2 - y1;
				g.DrawRectangle(new Pen(new SolidBrush(ruler.ForeColor)), x1, y1, x2, y2);
				g.FillRectangle(_DrawingBrush, x1, y1, x2, y2);

				x1 = ruler.HeaderOffset/2;
				x2 = ruler.HeaderOffset - x1;
				y1 = ruler.ScaleValueToPixel((double) _StartMillisecond) - (x2/2) - 1;
				g.FillRectangle(new SolidBrush(ruler.ForeColor), x1, y1, x2, x2);

				x1 = ruler.HeaderOffset/2;
				y2 = ruler.HeaderOffset - y1;
				y1 = ruler.ScaleValueToPixel((double) _EndMillisecond) - (x2/2) - 1;
				g.FillRectangle(new SolidBrush(ruler.ForeColor), x1, y1, x2, x2);
			}

		}

		public override void MoveFrame(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{
			int iMousePosition = ((ruler.Orientation == enumOrientation.orHorizontal) ? e.X : e.Y);
			long lNewMillisecond = (long) ruler.PixelToScaleValue(iMousePosition);

			if(lNewMillisecond > ruler.CurrentMillisecond)
			{
				int iStartDist = DistanceFromHandle(e, ruler, _StartMillisecond);
				int iEndDist = DistanceFromHandle(e, ruler, _EndMillisecond);
			
				if(iStartDist < iEndDist)
				{
					if(_StartMillisecond > ruler.CurrentMillisecond && 
						!ruler.KeyFrames.Overlaps(lNewMillisecond, this._EndMillisecond, this))
						_StartMillisecond = lNewMillisecond;
				}
				else
				{
					if(_EndMillisecond > ruler.CurrentMillisecond && 
						!ruler.KeyFrames.Overlaps(this._StartMillisecond, lNewMillisecond, this))
						_EndMillisecond = lNewMillisecond;
				}

				ruler.RedrawBitmap();
			}
		}

		public override void MoveFrame(long lStart, long lEnd, TimeRuler ruler)
		{
			if(lStart > ruler.CurrentMillisecond && lEnd > ruler.CurrentMillisecond)
			{
				if(_StartMillisecond > ruler.CurrentMillisecond && 
					!ruler.KeyFrames.Overlaps(lStart, this._EndMillisecond, this))
					_StartMillisecond = lStart;
			
				if(_EndMillisecond > ruler.CurrentMillisecond && 
					!ruler.KeyFrames.Overlaps(this._StartMillisecond, lEnd, this))
					_EndMillisecond = lEnd;

				ruler.RedrawBitmap();
			}
		}

		#endregion

	}
}
