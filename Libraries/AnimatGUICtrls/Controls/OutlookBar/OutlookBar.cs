using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace AnimatGuiCtrls.Controls
{
//	namespace OutlookBar
//	{
		internal class BandTagInfo
		{
			public OutlookBar outlookBar;
			public int index;

			public BandTagInfo(OutlookBar ob, int index)
			{
				outlookBar=ob;
				this.index=index;
			}
		}

		public class OutlookBar : Panel
		{
			private int buttonHeight;
			private int selectedBand;
			private int selectedBandHeight;

			public int ButtonHeight
			{
				get
				{
					return buttonHeight;
				}

				set
				{
					buttonHeight=value;
					// do recalc layout for entire bar
				}
			}

			public int SelectedBand
			{
				get
				{
					return selectedBand;
				}
				set
				{
					SelectBand(value);
				}
			}

			public OutlookBar()
			{
				buttonHeight=25;
				selectedBand=0;
				selectedBandHeight=0;
				AllowDrop = true;
			}

			public void Initialize()
			{
				// parent must exist!
				this.SizeChanged+=new EventHandler(SizeChangedEvent);
			}

			public void AddBand(string caption, ContentPanel content)
			{
				content.outlookBar=this;
				int index=Controls.Count;
				BandTagInfo bti=new BandTagInfo(this, index);
				BandPanel bandPanel=new BandPanel(caption, content, bti);
				Controls.Add(bandPanel);
				UpdateBarInfo();
				RecalcLayout(bandPanel, index);
			}

			public void SelectBand(int index)
			{
				selectedBand=index;
				RedrawBands();
			}

//			public override void Refresh()
//			{
//				RecalcLayout();
//				base.Refresh ();
//			}

			protected override void OnResize(EventArgs e)
			{
				try
				{
					RedrawBands();
					base.OnResize (e);
				}
				catch(System.Exception ex)
				{string strMsg = ex.Message;}
			}

			private void RedrawBands()
			{
				UpdateBarInfo();
				for (int i=0; i<Controls.Count; i++)
				{
					BandPanel bp=Controls[i] as BandPanel;
					RecalcLayout(bp, i);
				}
			}

			private void UpdateBarInfo()
			{
				selectedBandHeight=ClientRectangle.Height-(Controls.Count * buttonHeight);
			}

			private void RecalcLayout(BandPanel bandPanel, int index)
			{
				int vPos=(index <= selectedBand) ? buttonHeight*index : buttonHeight*index+selectedBandHeight;
				int height=selectedBand==index ? selectedBandHeight+buttonHeight : buttonHeight;

				// the band dimensions
				bandPanel.Location=new Point(0, vPos);
				bandPanel.Size=new Size(ClientRectangle.Width, height);

				// the contained button dimensions
				bandPanel.Controls[0].Location=new Point(0, 0);
				bandPanel.Controls[0].Size=new Size(ClientRectangle.Width, buttonHeight);

				// the contained content panel dimensions
				bandPanel.Controls[1].Location=new Point(0, buttonHeight);
				bandPanel.Controls[1].Size=new Size(ClientRectangle.Width-2, height-25);

				foreach(System.Windows.Forms.Control ctrlIcon in bandPanel.Content.Controls)
				{
					int iX = bandPanel.Content.Size.Width/2 - ctrlIcon.Width/2;
					ctrlIcon.Location = new Point(iX, ctrlIcon.Location.Y);
				}
			}

			private void SizeChangedEvent(object sender, EventArgs e)
			{
				try
				{
					//Size=new Size(Size.Width, ((Control)sender).ClientRectangle.Size.Height);
					UpdateBarInfo();
					RedrawBands();
				}
				catch(System.Exception ex)
				{string strMsg = ex.Message;}
			}
		}

		internal class BandPanel : Panel
		{
			private ContentPanel m_Content;

			public BandPanel(string caption, ContentPanel content, BandTagInfo bti)
			{
				BandButton bandButton=new BandButton(caption, bti);
				Controls.Add(bandButton);
				Controls.Add(content);
				m_Content = content;
			}

			public ContentPanel Content
			{
				get
				{
					return m_Content;
				}
			}
		}

		internal class BandButton : Button
		{
			private BandTagInfo bti;

			public BandButton(string caption, BandTagInfo bti)
			{
				Text=caption;
				FlatStyle=FlatStyle.Standard;
				Visible=true;
				this.bti=bti;
				Click+=new EventHandler(SelectBand);
			}

			private void SelectBand(object sender, EventArgs e)
			{
				bti.outlookBar.SelectBand(bti.index);
			}
		}

		public abstract class ContentPanel : Panel
		{
			public OutlookBar outlookBar;

			public ContentPanel()
			{
				// initial state
				Visible=true;
			}
		}

		public class IconPanel : ContentPanel
		{
			protected int iconHeight;
			protected int iconSpacing;
			protected int margin;

			public int IconSpacing
			{
				get
				{
					return iconSpacing;
				}
			}

			public int PanelMargin
			{
				get
				{
					return margin;
				}
			}

			public int IconHeight
			{
				get
				{
					return iconHeight;
				}
				set
				{
					if(value > 0)
						iconHeight = value;
					iconSpacing=iconHeight+15+margin;	// icon height + text height + margin
				}
			}

			public IconPanel()
			{
				margin=10;
				iconHeight=35;
				iconSpacing=iconHeight+15+margin;	// icon height + text height + margin
				BackColor=Color.LightBlue;
				AutoScroll=true;
				AllowDrop = true;
			}

			public PanelIcon AddIcon(string caption, Image imgPanel, Image imgDrag, Object IconData)
			{
				return AddIcon(caption, imgPanel, imgDrag, IconData, null, null);
			}

			public PanelIcon AddIcon(string caption, Image imgPanel, Image imgDrag, Object IconData, EventHandler onClickEvent)
			{
				return AddIcon(caption, imgPanel, imgDrag, IconData, onClickEvent, null);
			}

			public PanelIcon AddIcon(string caption, Image imgPanel, Image imgDrag, Object IconData, EventHandler onClickEvent, PanelIcon.DoubleClickIconEvent onDoubleClickEvent)
			{
				int index=Controls.Count/2;	// two entries per icon

				if(imgPanel == null)
					throw new System.Exception("The image associated with the icon '" + caption + "' is not defined.");

				if(imgDrag == null)
					throw new System.Exception("The drag image associated with the icon '" + caption + "' is not defined.");

				if(IconData == null)
					throw new System.Exception("The IconData associated with the icon '" + caption + "' is not defined.");

				PanelIcon panelIcon=new PanelIcon(this, imgPanel, imgDrag, index, IconData, onClickEvent, onDoubleClickEvent);
				Controls.Add(panelIcon);

				Label label=new Label();
				label.Text=caption;
				label.Visible=true;
				label.Location=new Point(0, margin+imgPanel.Size.Height+index*iconSpacing);
				label.Size=new Size(Size.Width, 15);
				label.TextAlign=ContentAlignment.TopCenter;
				label.Click+=onClickEvent;
				label.Tag=panelIcon;
				Controls.Add(label);

				return panelIcon;
			}

		}

		public class PanelIcon : PictureBox
		{
			public int index;
			public IconPanel iconPanel;
			
			protected ImageList  m_imageList;
			protected ImageListDrag  m_imageDrag;
			protected bool m_bDraggingIcon;
			protected System.Drawing.Image m_imgDrag;

			public delegate void DoubleClickIconEvent(PanelIcon Icon);
			public event DoubleClickIconEvent DoubleClickIcon;

			private Color bckgColor;
			//private bool mouseEnter;
			private Object m_oIconData;

			public int Index
			{
				get
				{
					return index;
				}
			}

			public Object Data
			{
				get
				{
					return m_oIconData;
				}
				set
				{
					m_oIconData = value;
				}
			}

			public bool DraggingIcon
			{
				get
				{
					return m_bDraggingIcon;
				}
				set
				{
					m_bDraggingIcon = value;
				}
			}

			public System.Drawing.Image DragImage
			{
				get
				{
					return m_imgDrag;
				}
				set
				{
					m_imgDrag = value;
				}
			}

			public PanelIcon(IconPanel parent, Image imgPanel, Image imgDrag, int index, Object IconData)
			{
				Initialize(parent, imgPanel, imgDrag, index, IconData, null, null);
			}
 
			public PanelIcon(IconPanel parent, Image imgPanel, Image imgDrag, int index, Object IconData, EventHandler onClickEvent)
			{
				Initialize(parent, imgPanel, imgDrag, index, IconData, onClickEvent, null);
			}
				
			public PanelIcon(IconPanel parent, Image imgPanel, Image imgDrag, int index, Object IconData, EventHandler onClickEvent, DoubleClickIconEvent onDoubleClickEvent)
			{
				Initialize(parent, imgPanel, imgDrag, index, IconData, onClickEvent, onDoubleClickEvent);
			}

			private void Initialize(IconPanel parent, Image imgPanel, Image imgDrag, int index, Object IconData, EventHandler onClickEvent, DoubleClickIconEvent onDoubleClickEvent)
			{
				this.index=index;
				this.iconPanel=parent;
				this.m_oIconData = IconData;
				m_imgDrag = imgDrag;

				if(IconData == null)
					throw new Exception("IconData is not set for this panel icon.");

				Image=imgPanel;
				Visible=true;
				Location=new Point(iconPanel.outlookBar.Size.Width/2-imgPanel.Size.Width/2,
					iconPanel.PanelMargin + index*iconPanel.IconSpacing);
				Size=imgPanel.Size;

				if(onClickEvent != null)
					Click+=onClickEvent;

				if(onDoubleClickEvent != null)
					DoubleClickIcon+=onDoubleClickEvent;

				Tag=this;

				MouseEnter+=new EventHandler(OnMouseEnter);
				MouseLeave+=new EventHandler(OnMouseLeave);
				MouseMove+=new MouseEventHandler(OnMouseMove);
				MouseDown+=new MouseEventHandler(OnMouseDown);
				GiveFeedback+=new GiveFeedbackEventHandler(OnGiveFeedback);

				m_imageDrag = new ImageListDrag();
				m_imageList = new ImageList();
				m_imageList.ImageSize = new Size(imgDrag.Width, imgDrag.Height);
				m_imageDrag.Imagelist = m_imageList;
				m_imageList.Images.Add(imgDrag, System.Drawing.Color.Transparent);
				m_bDraggingIcon = false;

				bckgColor=iconPanel.BackColor;
				//mouseEnter=false;
				AllowDrop = true;
			}

			private void OnMouseMove(object sender, MouseEventArgs args)
			{
				try
				{
					if(args.Button == MouseButtons.Left)
					{
						//if they are moving the mouse while the left mouse button is down then
						//they are attempting to drag the item.
						m_bDraggingIcon = true;

						m_imageDrag.StartDrag(0, (int) (m_imgDrag.Width/2), (int) (m_imgDrag.Height/2));

						this.DoDragDrop(this, DragDropEffects.Copy);

						m_imageDrag.CompleteDrag();
					}
				}
				catch(System.Exception ex)
				{string strMsg = ex.Message;}
			}

			private void OnMouseEnter(object sender, EventArgs e)
			{
			}

			private void OnMouseLeave(object sender, EventArgs e)
			{
//				if (mouseEnter)
//				{
//					BackColor=bckgColor;
//					BorderStyle=BorderStyle.None;
//					Location=Location+new Size(1, 1);
//					mouseEnter=false;
//				}
			}

			private void OnMouseDown(object sender, MouseEventArgs args)
			{
//				m_bDraggingIcon = true;
//
//				m_imageDrag.StartDrag(0, (int) (m_imgDrag.Width/2), (int) (m_imgDrag.Height/2));
//
//				this.DoDragDrop(this, DragDropEffects.Copy);
//
//				m_imageDrag.CompleteDrag();
			}

			protected override void OnDoubleClick(EventArgs e)
			{
				try
				{
					base.OnDoubleClick (e);
					DoubleClickIcon(this);
				}
				catch(System.Exception ex)
				{string strMsg = ex.Message;}
			}

			protected override void OnDragEnter(DragEventArgs drgevent)
			{
				try
				{
					base.OnDragEnter (drgevent);
					drgevent.Effect = DragDropEffects.Copy;
				}
				catch(System.Exception ex)
				{string strMsg = ex.Message;}
			}

			private void OnGiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs args)
			{
				try
				{
					args.UseDefaultCursors = false;

					// Draw the drag image:
					// Debug.WriteLine("OnGiveFeedback: DraggingIcon: " + m_bDraggingIcon);
					if(m_bDraggingIcon)
						m_imageDrag.DragDrop();
				}
				catch(System.Exception ex)
				{string strMsg = ex.Message;}
			}

		}
//	}
}