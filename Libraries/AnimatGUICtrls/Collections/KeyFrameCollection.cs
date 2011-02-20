using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AnimatGuiCtrls.Controls;

namespace AnimatGuiCtrls.Collections
{

	public class KeyFrameCollection : CollectionBase
	{
		protected ArrayList _arySingleFrames = new ArrayList();
		protected ArrayList _aryMulitFrames = new ArrayList();
		protected KeyFrame _CurrentFrame = null;

		TimeRuler _Ruler = null;

		public KeyFrameCollection(TimeRuler ruler)
		{
			// Must provide a valid manager instance
			if (ruler == null)
				throw new ArgumentNullException("TimeRuler");

			// Default the state
			_Ruler = ruler;
		}


		public KeyFrame Add(KeyFrame keyFrame)
		{
			return Add(keyFrame, false);
		}

		public KeyFrame Add(KeyFrame keyFrame, bool bSuspendRedraw)
		{
			if( (keyFrame.StartMillisecond > _Ruler.EndMillisecond) || (keyFrame.EndMillisecond > _Ruler.EndMillisecond) )
				throw new System.Exception("You can not add a keyframe with a start/end time greater than the end time of the ruler.");

			if(keyFrame.KeyFrameType == KeyFrame.enumKeyFrameType.Snapshot)
			{
				//First lets verify that there is not already a single frame at this time slice.
				foreach(KeyFrame frame in _arySingleFrames)
					if(frame.StartMillisecond == keyFrame.StartMillisecond)
						return null;

				//Now lets make sure that it does not overlap with any of the video frames.
				KeyFrame.enumFrameTimeType iTimeType = KeyFrame.enumFrameTimeType.StartTime;
				foreach(KeyFrame frame in _aryMulitFrames)
					if(frame.Overlaps(keyFrame, ref iTimeType))
						return null;

				_arySingleFrames.Add(keyFrame);
			}
			else if(keyFrame.KeyFrameType == KeyFrame.enumKeyFrameType.CurrentFrame)
			{
				if(_CurrentFrame != null)
					throw new System.Exception("There is alread a current frame defined.");

				_CurrentFrame = keyFrame;
			}
			else
			{	
				//First lets verify that there is not already a range frame overlapping this time slice.
				KeyFrame.enumFrameTimeType iTimeType = KeyFrame.enumFrameTimeType.StartTime;
				foreach(KeyFrame frame in _aryMulitFrames)
					if(frame.Overlaps(keyFrame, ref iTimeType))
					{
						//If it overlaps because of the start time then chunk it. If it overlaps because of the end time
						//then lets see if we can come up with an end time that will work.
						if(iTimeType == KeyFrame.enumFrameTimeType.StartTime)
							return null;
						else
							keyFrame.EndMillisecond = frame.StartMillisecond - 1;
					}

				//Now lets verify that there is not a single frame overlapping this video range.
				foreach(KeyFrame frame in _arySingleFrames)
					if(keyFrame.Overlaps(frame, ref iTimeType))
					{
						//Lets find whether the start or end point is closest and then then 
						//add the frame so it does not overlap.
						if(Math.Abs(frame.StartMillisecond-keyFrame.StartMillisecond) < Math.Abs(frame.StartMillisecond-keyFrame.EndMillisecond) )
						{
							//Start millisecond is closer to the single.
							keyFrame.StartMillisecond = frame.StartMillisecond + 1;
						}
						else
						{
							//End millisecond is closer to the single.
							keyFrame.EndMillisecond = frame.StartMillisecond - 1;

						}
					}

				_aryMulitFrames.Add(keyFrame);
			}			

			base.List.Add(keyFrame as object);

			if(!bSuspendRedraw) _Ruler.RedrawBitmap();

			return keyFrame;
		}

		public void Remove(KeyFrame value)
		{
			Remove(value, false);
		}

		public void Remove(KeyFrame value, bool bSuspendRedraw)
		{
			// Use base class to process actual collection operation
			base.List.Remove(value as object);

			if(value.KeyFrameType == KeyFrame.enumKeyFrameType.Snapshot)
				_arySingleFrames.Remove(value);
			else if(value.KeyFrameType == KeyFrame.enumKeyFrameType.CurrentFrame)
				_CurrentFrame = null;
			else
				_aryMulitFrames.Remove(value);

			if(!bSuspendRedraw) _Ruler.RedrawBitmap();
		}

		public bool Contains(KeyFrame value)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(value as object);
		}

		public KeyFrame this[int index]
		{
			// Use base class to process actual collection operation
			get { return (base.List[index] as KeyFrame); }
		}

		public int SetIndex(int newIndex, KeyFrame value)
		{
			base.List.Remove(value);
			base.List.Insert(newIndex, value);

			return newIndex;
		}

		public int IndexOf(KeyFrame value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		public KeyFrameCollection Copy()
		{
			KeyFrameCollection clone = new KeyFrameCollection(_Ruler);

			// Copy each reference across
			foreach(KeyFrame c in base.List)
				clone.Add(c);

			return clone;
		}

		public void Draw(Graphics g)
		{
			if(_CurrentFrame != null)
				_CurrentFrame.Draw(g, _Ruler);

			foreach(KeyFrame frame in _aryMulitFrames)
				frame.Draw(g, _Ruler);

			foreach(KeyFrame frame in _arySingleFrames)
				frame.Draw(g, _Ruler);
		}

		public KeyFrame FindClosest(KeyFrame.enumKeyFrameType iType, long lMillisecond)
		{return FindClosest(iType, lMillisecond, false);}

		public KeyFrame FindClosest(KeyFrame.enumKeyFrameType iType, long lMillisecond, bool bInActualZoneOnly)
		{
			if(iType == KeyFrame.enumKeyFrameType.Snapshot)
			{
				KeyFrame minFrame = null;
				long lMinDist = -1, lDist = 0;

				foreach(KeyFrame frame in _arySingleFrames)
				{
					lDist = Math.Abs(frame.StartMillisecond - lMillisecond);

					if(!bInActualZoneOnly || (bInActualZoneOnly && (frame.StartMillisecond <= _Ruler.ActualMillisecond)))
					{
						if(lDist < lMinDist || lMinDist == -1)
						{
							lMinDist = lDist;
							minFrame = frame;
						}
					}
				}

				if(lMinDist < (long) (lMillisecond*0.05))
					return minFrame;
				else
					return null;
			}
			else
			{
				foreach(KeyFrame frame in _aryMulitFrames)
				{
					if( (lMillisecond >= frame.StartMillisecond) && (lMillisecond <= frame.EndMillisecond) )
						return frame;
				}
			}

			return null;
		}

		public KeyFrame RemoveClosest(KeyFrame.enumKeyFrameType iType, long lMillisecond)
		{
			KeyFrame keyClosest = FindClosest(iType, lMillisecond);

			if(keyClosest != null)
			{
				Remove(keyClosest);
				return keyClosest;
			}

			return null;
		}

		public KeyFrame IsHandleClick(System.Windows.Forms.MouseEventArgs e)
		{
			foreach(KeyFrame frame in this)
				if(frame.IsHandleClick(e, _Ruler))
					return frame;

			return null;
		}

		public bool Overlaps(long lStart, long lEnd, KeyFrame testFrame)
		{
			KeyFrame.enumFrameTimeType iTimeType = KeyFrame.enumFrameTimeType.StartTime;
			foreach(KeyFrame frame in base.List)
			{
				if(frame != testFrame && frame != _CurrentFrame && frame.Overlaps(lStart, lEnd, ref iTimeType))
					return true;
			}

			return false;
		}
	}
}
