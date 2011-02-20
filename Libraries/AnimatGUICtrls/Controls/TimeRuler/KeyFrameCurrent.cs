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
	/// Summary description for KeyFrameCurrent.
	/// </summary>
	public class KeyFrameCurrent : KeyFrame
	{
		#region Properties

		public override enumKeyFrameType KeyFrameType
		{ 
			get { return enumKeyFrameType.CurrentFrame; }
		}

		#endregion

		#region Methods

		public KeyFrameCurrent()
		{
			_Color = Color.Blue;
		}

		public KeyFrameCurrent(long lStart)
		{
			SetTimes(lStart, lStart);
			_Color = Color.Blue;
		}

		public override void Draw(Graphics g, TimeRuler ruler)
		{
			if (ruler.Orientation == enumOrientation.orHorizontal)
			{
				int iSecondPos = ruler.ScaleValueToPixel((double) _StartMillisecond);
				int x = iSecondPos - 1;
				int y1 = ruler.HeaderOffset/2, y2 = ruler.Height;
				g.DrawLine(new Pen(new SolidBrush(this.Color), 3), x, y1, x, y2);

				Point left = new Point(iSecondPos - y1, 1);
				Point right = new Point(iSecondPos + y1, 1);
				Point bottom = new Point(iSecondPos, ruler.HeaderOffset/2);
				Point[] trianglePoints = {left, right, bottom};
				g.FillPolygon(new SolidBrush(this.Color), trianglePoints, System.Drawing.Drawing2D.FillMode.Winding);

				g.DrawPolygon(new Pen(new SolidBrush(ruler.ForeColor)), trianglePoints);
			}
			else
			{
				int iSecondPos = ruler.ScaleValueToPixel((double) _StartMillisecond);
				int y = iSecondPos - 1;
				int x1 = ruler.HeaderOffset/2, x2 = ruler.Height;
				g.DrawLine(new Pen(new SolidBrush(this.Color), 3), x1, y, x2, y);

				Point left = new Point(1, iSecondPos - x1);
				Point right = new Point(1, iSecondPos + x1);
				Point bottom = new Point(ruler.HeaderOffset/2, iSecondPos);
				Point[] trianglePoints = {left, right, bottom};
				g.FillPolygon(new SolidBrush(this.Color), trianglePoints, System.Drawing.Drawing2D.FillMode.Winding);

				g.DrawPolygon(new Pen(new SolidBrush(ruler.ForeColor)), trianglePoints);
			}
		}


		public override bool IsHandleClick(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler, long lMillisecond)
		{
			int y1 = 1;
			int y2 = ruler.HeaderOffset/2;
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

		public override bool CanBeMoved(TimeRuler ruler) 
		{return true;}

		public override void MoveFrame(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{
			int iMousePosition = ((ruler.Orientation == enumOrientation.orHorizontal) ? e.X : e.Y);			
			long lNewMillisecond = (long) ruler.PixelToScaleValue(iMousePosition);

			if(lNewMillisecond < ruler.ActualMillisecond)
			{
				_StartMillisecond = lNewMillisecond;
				_EndMillisecond = _StartMillisecond;
	
				ruler.RedrawBitmap();
			}
		}

		public override void EndMove(System.Windows.Forms.MouseEventArgs e, TimeRuler ruler)
		{
			int iMousePosition = ((ruler.Orientation == enumOrientation.orHorizontal) ? e.X : e.Y);
			long lNewMillisecond = (long) ruler.PixelToScaleValue(iMousePosition);

			KeyFrame keyClosest = ruler.KeyFrames.FindClosest(KeyFrame.enumKeyFrameType.Snapshot, lNewMillisecond, true);

			//If we can not find a single frame close to the current end position that is within the current time zone then move
			//it back to the currenttime. Otherwise move it to the closest key frame.
			if(keyClosest != null)
			{
				SetTimes(keyClosest.StartMillisecond);
				ruler.CurrentMillisecond = keyClosest.StartMillisecond;
				ruler.OnCurrentFrameMoved(keyClosest);
			}
			else
			{
				SetTimes(ruler.ActualMillisecond);
				ruler.CurrentMillisecond = ruler.ActualMillisecond;
				ruler.OnCurrentFrameMoved(null);
			}

			ruler.RedrawBitmap();
		}

		#endregion

	}
}
