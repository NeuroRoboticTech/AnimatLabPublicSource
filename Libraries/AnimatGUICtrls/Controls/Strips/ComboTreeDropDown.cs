// A ComboBox with a TreeView Drop-Down
// Bradley Smith - 2010/11/04

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.IO;

namespace AnimatGuiCtrls.Controls
{

    /// <summary>
    /// Represents the dropdown portion of the ComboTreeBox control. The nodes are displayed in a 
    /// manner similar to the TreeView control.
    /// </summary>
    [ToolboxItem(false)]
    public class ComboTreeDropDown : ToolStripDropDown
    {

        const int GLYPH_SIZE = 16;
        const int INDENT_WIDTH = 16;
        const int MIN_ITEM_HEIGHT = 16;
        const int MIN_THUMB_HEIGHT = 20;
        const int SCROLLBAR_WIDTH = 17;
        readonly Size SCROLLBUTTON_SIZE = new Size(SCROLLBAR_WIDTH, SCROLLBAR_WIDTH);
        const TextFormatFlags TEXT_FORMAT_FLAGS = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;

        private static Bitmap collapsed;
        private static Bitmap expanded;

        private Dictionary<BitmapInfo, Image> bitmaps;
        private int dropDownHeight;
        private int highlightedItemIndex;
        private Rectangle interior;
        private int itemHeight;
        private int numItemsDisplayed;
        private bool scrollBarVisible;
        private ScrollBarInfo scrollBar;
        private bool scrollDragging;
        private int scrollOffset;
        private Timer scrollRepeater;
        private ComboTreeBox sourceControl;
        private List<NodeInfo> visibleItems;

        /// <summary>
        /// Gets the collapsed (+) glyph to paint on the dropdown.
        /// </summary>
        private Image Collapsed
        {
            get
            {
                if (collapsed == null)
                {
                    collapsed = new Bitmap(16, 16);
                    Graphics g = Graphics.FromImage(collapsed);
                    Rectangle r = new Rectangle(4, 4, 8, 8);
                    g.FillRectangle(Brushes.White, r);
                    g.DrawRectangle(Pens.Gray, r);
                    g.DrawLine(Pens.Black, Point.Add(r.Location, new Size(2, 4)), Point.Add(r.Location, new Size(6, 4)));
                    g.DrawLine(Pens.Black, Point.Add(r.Location, new Size(4, 2)), Point.Add(r.Location, new Size(4, 6)));
                }
                return collapsed;
            }
        }
        /// <summary>
        /// Removes extraneous default padding from the dropdown.
        /// </summary>
        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(0, 1, 0, 1);
            }
        }
        /// <summary>
        /// Gets or sets the maximum height of the dropdown.
        /// </summary>
        public int DropDownHeight
        {
            get { return dropDownHeight; }
            set
            {
                dropDownHeight = value;
                UpdateVisibleItems();
            }
        }
        /// <summary>
        /// Gets the expanded (-) glyph to paint on the dropdown.
        /// </summary>
        private Image Expanded
        {
            get
            {
                if (expanded == null)
                {
                    expanded = new Bitmap(16, 16);
                    Graphics g = Graphics.FromImage(expanded);
                    Rectangle r = new Rectangle(4, 4, 8, 8);
                    g.FillRectangle(Brushes.White, r);
                    g.DrawRectangle(Pens.Gray, r);
                    g.DrawLine(Pens.Black, Point.Add(r.Location, new Size(2, 4)), Point.Add(r.Location, new Size(6, 4)));
                }
                return expanded;
            }
        }
        /// <summary>
        /// Gets or sets the first visible ComboTreeNode in the drop-down portion of the control.
        /// </summary>
        public ComboTreeNode TopNode
        {
            get
            {
                return visibleItems[scrollOffset].Node;
            }
            set
            {
                for (int i = 0; i < visibleItems.Count; i++)
                {
                    if (visibleItems[i].Node == value)
                    {
                        if ((i < scrollOffset) || (i >= (scrollOffset + numItemsDisplayed)))
                        {
                            scrollOffset = Math.Min(Math.Max(0, i - numItemsDisplayed + 1), visibleItems.Count - numItemsDisplayed);
                            UpdateScrolling();
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Gets the number of ComboTreeNodes visible in the drop-down portion of the control.
        /// </summary>
        public int VisibleCount
        {
            get
            {
                return numItemsDisplayed;
            }
        }


        /// <summary>
        /// Initialises a new instance of ComboTreeDropDown and associates it with its parent ComboTreeBox.
        /// </summary>
        /// <param name="sourceControl"></param>
        public ComboTreeDropDown(ComboTreeBox sourceControl)
        {
            visibleItems = new List<NodeInfo>();
            bitmaps = new Dictionary<BitmapInfo, Image>();
            scrollBar = new ScrollBarInfo();
            AutoSize = false;
            this.sourceControl = sourceControl;
            RenderMode = ToolStripRenderMode.System;
            BackColor = Color.White;
            dropDownHeight = 150;
            itemHeight = MIN_ITEM_HEIGHT;
            Items.Add("");

            scrollRepeater = new Timer();
            scrollRepeater.Tick += new EventHandler(scrollRepeater_Tick);
        }

        /// <summary>
        /// Generates a bitmap to display beside the ToolStripItem representation of the specified node.
        /// </summary>
        /// <param name="bitmapInfo"></param>
        /// <param name="nodeImage"></param>
        /// <returns></returns>
        private Image GenerateBitmap(BitmapInfo bitmapInfo, Image nodeImage)
        {
            int indentation = INDENT_WIDTH * bitmapInfo.NodeDepth;
            int halfIndent = INDENT_WIDTH / 2;
            int halfHeight = itemHeight / 2;

            // create a bitmap that will be composed of the node's image and the glyphs/lines/indentation
            Bitmap composite = new Bitmap(INDENT_WIDTH + indentation + ((nodeImage != null) ? nodeImage.Width : 0), itemHeight);
            Graphics g = Graphics.FromImage(composite);

            Pen dotted = new Pen(Color.Gray);
            dotted.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            // horizontal dotted line
            g.DrawLine(dotted, indentation + halfIndent, halfHeight, indentation + INDENT_WIDTH, halfHeight);

            // vertical dotted line to peers
            g.DrawLine(dotted, indentation + halfIndent, bitmapInfo.IsFirst ? halfHeight : 0, indentation + halfIndent, bitmapInfo.IsLastPeer ? halfHeight : itemHeight);

            // vertical dotted line to subtree
            if (bitmapInfo.NodeExpanded) g.DrawLine(dotted, INDENT_WIDTH + indentation + halfIndent, halfHeight, INDENT_WIDTH + indentation + halfIndent, itemHeight);

            // outer vertical dotted lines
            for (int i = 0; i < bitmapInfo.VerticalLines.Length; i++)
            {
                if (bitmapInfo.VerticalLines[i])
                {
                    int parentIndent = (INDENT_WIDTH * (bitmapInfo.NodeDepth - (i + 1)));
                    g.DrawLine(dotted, parentIndent + halfIndent, 0, parentIndent + halfIndent, itemHeight);
                }
            }

            // composite the image associated with node (appears at far right)
            if (nodeImage != null) g.DrawImage(nodeImage, new Rectangle(
                INDENT_WIDTH + indentation,
                composite.Height / 2 - nodeImage.Height / 2,
                nodeImage.Width,
                nodeImage.Height
            ));

            // render plus/minus glyphs
            if (bitmapInfo.HasChildren)
            {
                Rectangle glyphBounds = new Rectangle(indentation, composite.Height / 2 - GLYPH_SIZE / 2, GLYPH_SIZE, GLYPH_SIZE);
                VisualStyleElement elem = bitmapInfo.NodeExpanded ? VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;

                if (sourceControl.DrawWithVisualStyles && VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(elem))
                {
                    // visual style support, render using visual styles
                    VisualStyleRenderer r = new VisualStyleRenderer(elem);
                    r.DrawBackground(g, glyphBounds);
                }
                else
                {
                    // no visual style support, render using bitmap
                    Image glyph = bitmapInfo.NodeExpanded ? Expanded : Collapsed;
                    g.DrawImage(glyph, glyphBounds);
                }
            }

            return composite;
        }

        /// <summary>
        /// Determines how to draw a scrollbar button.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private ButtonState GetButtonState(Rectangle bounds)
        {
            ButtonState state = ButtonState.Normal;
            if (bounds.Contains(PointToClient(Cursor.Position)) && !scrollDragging)
            {
                if ((MouseButtons & MouseButtons.Left) == MouseButtons.Left) state = ButtonState.Pushed;
            }
            return state;
        }

        /// <summary>
        /// Returns the ComboTreeNodeCollection to which the specified node belongs.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private ComboTreeNodeCollection GetCollectionContainingNode(ComboTreeNode node)
        {
            return (node.Parent != null) ? node.Parent.Nodes : sourceControl.Nodes;
        }

        /// <summary>
        /// Determines all of the parameters for drawing the bitmap beside the 
        /// specified node. If they represent a unique combination, the bitmap is 
        /// generated and returned. Otherwise, the appropriate cached bitmap is 
        /// returned.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Image GetItemBitmap(ComboTreeNode node)
        {
            BitmapInfo bitmapInfo = new BitmapInfo();

            // the following factors determine the bitmap drawn:
            ComboTreeNodeCollection collection = GetCollectionContainingNode(node);
            bitmapInfo.HasChildren = (node.Nodes.Count > 0);
            bitmapInfo.IsLastPeer = (collection.IndexOf(node) == (collection.Count - 1));
            bitmapInfo.IsFirst = (node == sourceControl.Nodes[0]);
            bitmapInfo.NodeDepth = node.Depth;
            bitmapInfo.NodeExpanded = node.Expanded && bitmapInfo.HasChildren;
            bitmapInfo.ImageIndex = bitmapInfo.NodeExpanded ? node.ExpandedImageIndex : node.ImageIndex;
            bitmapInfo.ImageKey = bitmapInfo.NodeExpanded ? node.ExpandedImageKey : node.ImageKey;

            bitmapInfo.VerticalLines = new bool[bitmapInfo.NodeDepth];
            ComboTreeNode parent = node;
            int i = 0;
            while ((parent = parent.Parent) != null)
            {
                // vertical line required if parent is expanded (and not last peer)
                ComboTreeNodeCollection parentCollection = GetCollectionContainingNode(parent);
                bitmapInfo.VerticalLines[i] = (parent.Expanded && (parentCollection.IndexOf(parent) != (parentCollection.Count - 1)));
                i++;
            }

            if (bitmaps.ContainsKey(bitmapInfo))
                return bitmaps[bitmapInfo];
            else
                return (bitmaps[bitmapInfo] = GenerateBitmap(bitmapInfo, sourceControl.GetNodeImage(node)));
        }

        /// <summary>
        /// Determines how to draw the main part of the scrollbar.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private ScrollBarState GetScrollBarState(Rectangle bounds)
        {
            ScrollBarState state = ScrollBarState.Normal;
            Point local = PointToClient(Cursor.Position);
            if (bounds.Contains(local)
                && !scrollDragging
                && !scrollBar.DownArrow.Contains(local)
                && !scrollBar.UpArrow.Contains(local)
                && !scrollBar.Thumb.Contains(local))
            {

                if ((MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                    state = ScrollBarState.Pressed;
                else
                    state = ScrollBarState.Hot;
            }
            return state;
        }

        /// <summary>
        /// Determines how to draw the down arrow on the scrollbar.
        /// </summary>
        /// <returns></returns>
        private ScrollBarArrowButtonState GetScrollBarStateDown()
        {
            ScrollBarArrowButtonState state = ScrollBarArrowButtonState.DownNormal;
            if (scrollBar.DownArrow.Contains(PointToClient(Cursor.Position)) && !scrollDragging)
            {
                if ((MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                    state = ScrollBarArrowButtonState.DownPressed;
                else
                    state = ScrollBarArrowButtonState.DownHot;
            }
            return state;
        }

        /// <summary>
        /// Determines how to draw the up arrow on the scrollbar.
        /// </summary>
        /// <returns></returns>
        private ScrollBarArrowButtonState GetScrollBarStateUp()
        {
            ScrollBarArrowButtonState state = ScrollBarArrowButtonState.UpNormal;
            if (scrollBar.UpArrow.Contains(PointToClient(Cursor.Position)) && !scrollDragging)
            {
                if ((MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                    state = ScrollBarArrowButtonState.UpPressed;
                else
                    state = ScrollBarArrowButtonState.UpHot;
            }
            return state;
        }

        /// <summary>
        /// Determines how to draw the 'thumb' button on the scrollbar.
        /// </summary>
        /// <returns></returns>
        private ScrollBarState GetScrollBarThumbState()
        {
            ScrollBarState state = ScrollBarState.Normal;
            if (scrollBar.Thumb.Contains(PointToClient(Cursor.Position)))
            {
                if ((MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                    state = ScrollBarState.Pressed;
                else
                    state = ScrollBarState.Hot;
            }
            return state;
        }

        /// <summary>
        /// Registers the arrow keys as input keys.
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Home:
                case Keys.End:
                case Keys.Enter:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        /// <summary>
        /// Updates the status of the dropdown on the owning ComboTreeBox control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            base.OnClosed(e);

            // update DroppedDown on ComboTreeBox after close
            sourceControl.SetDroppedDown(false, false);
        }

        /// <summary>
        /// Prevents the clicking of items from closing the dropdown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                if (sourceControl.ClientRectangle.Contains(sourceControl.PointToClient(Cursor.Position))) e.Cancel = true;
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Updates the font on the items when the drop-down's font changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            itemHeight = Math.Max(MIN_ITEM_HEIGHT, Font.Height);
        }

        #region Keyboard Events

        /// <summary>
        /// Handles keyboard shortcuts.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = true;

            if (e.KeyChar == '+')
            {
                NodeInfo info = visibleItems[highlightedItemIndex];
                if (info.Node.Nodes.Count > 0)
                {
                    info.Node.Expanded = true;
                    UpdateVisibleItems();
                }
            }
            else if (e.KeyChar == '-')
            {
                NodeInfo info = visibleItems[highlightedItemIndex];
                if (info.Node.Nodes.Count > 0)
                {
                    info.Node.Expanded = false;
                    UpdateVisibleItems();
                }
            }
            if (e.KeyChar == '*')
            {
                sourceControl.ExpandAll();
                UpdateVisibleItems();
            }
            else if (e.KeyChar == '/')
            {
                sourceControl.CollapseAll();
                UpdateVisibleItems();
            }
            else
            {
                e.Handled = false;
            }

            base.OnKeyPress(e);
        }

        /// <summary>
        /// Handles keyboard shortcuts.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = e.SuppressKeyPress = true;

            if ((e.KeyCode == Keys.Enter) || (e.Alt && (e.KeyCode == Keys.Up)))
            {
                sourceControl.SelectedNode = visibleItems[highlightedItemIndex].Node;
                Close();
            }
            else if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left))
            {
                highlightedItemIndex = Math.Max(0, highlightedItemIndex - 1);
                sourceControl.SelectedNode = visibleItems[highlightedItemIndex].Node;
                ScrollToHighlighted(true);
                Refresh();
            }
            else if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Right))
            {
                highlightedItemIndex = Math.Min(highlightedItemIndex + 1, visibleItems.Count - 1);
                sourceControl.SelectedNode = visibleItems[highlightedItemIndex].Node;
                ScrollToHighlighted(false);
                Refresh();
            }
            else if (e.KeyCode == Keys.Home)
            {
                highlightedItemIndex = scrollOffset = 0;
                UpdateScrolling();
                Invalidate();
            }
            else if (e.KeyCode == Keys.End)
            {
                scrollOffset = visibleItems.Count - numItemsDisplayed;
                highlightedItemIndex = visibleItems.Count - 1;
                UpdateScrolling();
                Invalidate();
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                scrollOffset = Math.Min(scrollOffset + numItemsDisplayed, visibleItems.Count - numItemsDisplayed);
                highlightedItemIndex = Math.Min(scrollOffset + numItemsDisplayed - 1, visibleItems.Count - 1);
                UpdateScrolling();
                Refresh();
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                highlightedItemIndex = scrollOffset = Math.Max(scrollOffset - numItemsDisplayed, 0);
                UpdateScrolling();
                Refresh();
            }
            else
            {
                e.Handled = e.SuppressKeyPress = false;
            }

            base.OnKeyDown(e);
        }

        #endregion

        #region Mouse Events

        /// <summary>
        /// Handles dragging of the scrollbar and hot-tracking in response to movement of the mouse.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // dragging with the scrollbar's 'thumb' button
            if (scrollDragging)
            {
                double availableHeight = scrollBar.DisplayRectangle.Height - (2 * SCROLLBUTTON_SIZE.Height) - scrollBar.Thumb.Height;
                double position = Math.Min(e.Location.Y - scrollBar.DisplayRectangle.Top - SCROLLBUTTON_SIZE.Height - (scrollBar.Thumb.Height / 2), availableHeight);

                // measure the scroll offset based on the location of the mouse pointer, relative to the scrollbar's bounds
                scrollOffset = Math.Max(0, Math.Min(
                    (int)((position / availableHeight) * (double)(visibleItems.Count - numItemsDisplayed)),
                    (visibleItems.Count - numItemsDisplayed)
                ));

                UpdateScrolling();
                Refresh();
                return;
            }

            // moving the mouse over the scrollbar
            if (scrollBarVisible && scrollBar.DisplayRectangle.Contains(e.Location))
            {
                Invalidate();
                return;
            }

            // not within scrollbar's bounds, end auto-repeat behaviour
            scrollRepeater.Stop();

            // hit-test each displayed item's bounds to determine the highlighted item
            for (int i = scrollOffset; i < (scrollOffset + numItemsDisplayed); i++)
            {
                if (visibleItems[i].DisplayRectangle.Contains(e.Location))
                {
                    highlightedItemIndex = i;
                    Invalidate();
                    break;
                }
            }
        }

        /// <summary>
        /// Handles scrolling in response to the left mouse button being clicked.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

            // mouse pointer within the scrollbar's bounds
            if (scrollBarVisible && scrollBar.DisplayRectangle.Contains(e.Location))
            {
                if (e.Y > scrollBar.Thumb.Bottom)
                {
                    // any point below the thumb button requires scrolling - on bar = pagedown, on button = next
                    int step = (scrollBar.DownArrow.Contains(e.Location)) ? 1 : numItemsDisplayed;
                    ScrollDropDown(step);

                    // if the button is held, start auto-repeat behaviour
                    if (!scrollRepeater.Enabled)
                    {
                        scrollRepeater.Interval = 250;
                        scrollRepeater.Start();
                    }
                    return;
                }
                else if (e.Y < scrollBar.Thumb.Top)
                {
                    // any point above the thumb button requires scrolling - on bar = pagedown, on button = next
                    int step = (scrollBar.UpArrow.Contains(e.Location)) ? 1 : numItemsDisplayed;
                    ScrollDropDown(-step);

                    // if the button is held, start auto-repeat behaviour
                    if (!scrollRepeater.Enabled)
                    {
                        scrollRepeater.Interval = 250;
                        scrollRepeater.Start();
                    }
                    return;
                }
                else if (scrollBar.Thumb.Contains(e.Location))
                {
                    // assume the thumb button is being dragged
                    scrollDragging = true;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Disengages dragging of the scrollbar and handles hot-tracking in 
        /// response to the mouse button being released.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            scrollRepeater.Stop();
            scrollDragging = false;

            if (scrollBarVisible && scrollBar.DisplayRectangle.Contains(e.Location))
            {
                Invalidate();
                return;
            }
        }

        /// <summary>
        /// Handles the expand/collapse of nodes and selection in response to the 
        /// mouse being clicked.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (scrollDragging) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                for (int i = scrollOffset; i < (scrollOffset + numItemsDisplayed); i++)
                {
                    NodeInfo info = visibleItems[i];

                    if (info.DisplayRectangle.Contains(e.Location))
                    {
                        if (info.GlyphRectangle.Contains(e.Location))
                        {
                            info.Node.Expanded = !info.Node.Expanded;
                            UpdateVisibleItems();
                        }
                        else
                        {
                            sourceControl.SelectedNode = visibleItems[i].Node;
                            Close();
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Terminates dragging of the scrollbar in response to the mouse 
        /// returning to the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if ((MouseButtons & MouseButtons.Left) != MouseButtons.Left) scrollDragging = false;
        }

        /// <summary>
        /// Terminates dragging of the scrollbar in response to the mouse leaving 
        /// the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if ((MouseButtons & MouseButtons.Left) != MouseButtons.Left) scrollDragging = false;
        }

        #endregion

        /// <summary>
        /// Paints the drop-down, including all items within the scrolled region 
        /// and, if appropriate, the scrollbar.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (scrollBarVisible)
            {
                Rectangle upper = new Rectangle(scrollBar.DisplayRectangle.Left, scrollBar.DisplayRectangle.Top, scrollBar.DisplayRectangle.Width, scrollBar.Thumb.Top - scrollBar.DisplayRectangle.Top);
                Rectangle lower = new Rectangle(scrollBar.DisplayRectangle.Left, scrollBar.Thumb.Bottom, scrollBar.DisplayRectangle.Width, scrollBar.DisplayRectangle.Bottom - scrollBar.Thumb.Bottom);

                if (sourceControl.DrawWithVisualStyles && ScrollBarRenderer.IsSupported)
                {
                    ScrollBarRenderer.DrawUpperVerticalTrack(e.Graphics, upper, GetScrollBarState(upper));
                    ScrollBarRenderer.DrawLowerVerticalTrack(e.Graphics, lower, GetScrollBarState(lower));
                    ScrollBarRenderer.DrawArrowButton(e.Graphics, scrollBar.UpArrow, GetScrollBarStateUp());
                    ScrollBarRenderer.DrawArrowButton(e.Graphics, scrollBar.DownArrow, GetScrollBarStateDown());
                    ScrollBarRenderer.DrawVerticalThumb(e.Graphics, scrollBar.Thumb, GetScrollBarThumbState());
                    ScrollBarRenderer.DrawVerticalThumbGrip(e.Graphics, scrollBar.Thumb, GetScrollBarThumbState());
                }
                else
                {
                    Rectangle bounds = scrollBar.DisplayRectangle;
                    bounds.Offset(1, 0);
                    Rectangle up = scrollBar.UpArrow;
                    up.Offset(1, 0);
                    Rectangle down = scrollBar.DownArrow;
                    down.Offset(1, 0);
                    Rectangle thumb = scrollBar.Thumb;
                    thumb.Offset(1, 0);

                    System.Drawing.Drawing2D.HatchBrush brush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent50, SystemColors.ControlLightLight, SystemColors.Control);

                    e.Graphics.FillRectangle(brush, bounds);
                    ControlPaint.DrawScrollButton(e.Graphics, up, ScrollButton.Up, GetButtonState(scrollBar.UpArrow));
                    ControlPaint.DrawScrollButton(e.Graphics, down, ScrollButton.Down, GetButtonState(scrollBar.DownArrow));
                    ControlPaint.DrawButton(e.Graphics, thumb, ButtonState.Normal);
                }
            }

            for (int i = scrollOffset; i < (scrollOffset + numItemsDisplayed); i++)
            {
                bool highlighted = (highlightedItemIndex == i);
                NodeInfo item = visibleItems[i];

                // background
                if (highlighted) e.Graphics.FillRectangle(SystemBrushes.Highlight, item.DisplayRectangle);

                // image and glyphs
                if (item.Image != null) e.Graphics.DrawImage(item.Image, new Rectangle(item.DisplayRectangle.Location, item.Image.Size));

                Font font = new Font(Font, visibleItems[i].Node.FontStyle);

                Rectangle textBounds = new Rectangle(item.DisplayRectangle.X + item.Image.Width + 2, item.DisplayRectangle.Y, item.DisplayRectangle.Width - item.Image.Width - 4, itemHeight);
                TextRenderer.DrawText(e.Graphics, item.Node.Text, font, textBounds, highlighted ? SystemColors.HighlightText : ForeColor, TEXT_FORMAT_FLAGS);
            }
        }

        /// <summary>
        /// Displays the dropdown beneath its owning ComboTreeBox control.
        /// </summary>
        public void Open()
        {
            if (sourceControl.SelectedNode != null)
            {
                ComboTreeNode node = sourceControl.SelectedNode;

                // the selected node must have a clear path (i.e. all parents expanded)
                while ((node = node.Parent) != null) node.Expanded = true;
            }

            UpdateVisibleItems();

            // highlight and scroll to the selected node
            if (sourceControl.SelectedNode != null)
            {
                for (int i = 0; i < visibleItems.Count; i++)
                {
                    if (visibleItems[i].Node == sourceControl.SelectedNode)
                    {
                        highlightedItemIndex = i;
                        if ((highlightedItemIndex < scrollOffset) || (highlightedItemIndex >= (scrollOffset + numItemsDisplayed)))
                        {
                            scrollOffset = Math.Min(Math.Max(0, highlightedItemIndex - numItemsDisplayed + 1), visibleItems.Count - numItemsDisplayed);
                            UpdateScrolling();
                        }
                        break;
                    }
                }
            }

            // show below the source control
            Show(sourceControl, new Point(0, sourceControl.ClientRectangle.Height - 1));
        }

        /// <summary>
        /// Scrolls the drop-down up or down by the specified number of items.
        /// </summary>
        /// <param name="offset"></param>
        public void ScrollDropDown(int offset)
        {
            if (offset < 0)
            {
                // up/left
                scrollOffset = Math.Max(scrollOffset + offset, 0);
                UpdateScrolling();
                Invalidate();
            }
            else if (offset > 0)
            {
                // down/right
                scrollOffset = Math.Min(scrollOffset + offset, visibleItems.Count - numItemsDisplayed);
                UpdateScrolling();
                Invalidate();
            }
        }

        /// <summary>
        /// Scrolls the drop-down so as to ensure that the highlighted item is at 
        /// either the top or bottom of the scrolled region.
        /// </summary>
        /// <param name="highlightedAtTop"></param>
        private void ScrollToHighlighted(bool highlightedAtTop)
        {
            if ((highlightedItemIndex < scrollOffset) || (highlightedItemIndex >= (scrollOffset + numItemsDisplayed)))
            {
                if (highlightedAtTop)
                    scrollOffset = Math.Min(highlightedItemIndex, visibleItems.Count - numItemsDisplayed);
                else
                    scrollOffset = Math.Min(Math.Max(0, highlightedItemIndex - numItemsDisplayed + 1), visibleItems.Count - numItemsDisplayed);

                UpdateScrolling();
            }
        }

        /// <summary>
        /// Updates the items in the scrolled region. This method is called 
        /// whenever the scroll offset is changed.
        /// </summary>
        private void UpdateScrolling()
        {
            if (scrollBarVisible)
            {
                // calculate the bounds of the scrollbar's 'thumb' button
                int availableHeight = scrollBar.DisplayRectangle.Height - (2 * SCROLLBUTTON_SIZE.Height);

                double percentSize = (double)numItemsDisplayed / (double)visibleItems.Count;
                int size = Math.Max((int)(percentSize * (double)availableHeight), MIN_THUMB_HEIGHT);
                int diff = Math.Max(0, MIN_THUMB_HEIGHT - (int)(percentSize * (double)availableHeight));

                double percentStart = (double)scrollOffset / (double)visibleItems.Count;
                int start = Math.Min((int)Math.Ceiling(percentStart * (double)(availableHeight - diff)), availableHeight - MIN_THUMB_HEIGHT);

                scrollBar.Thumb = new Rectangle(new Point(scrollBar.DisplayRectangle.X, scrollBar.DisplayRectangle.Top + SCROLLBUTTON_SIZE.Height + start), new Size(SCROLLBAR_WIDTH, size));
            }

            // calculate display rectangles and assign images for each item in the scroll range
            for (int i = scrollOffset; i < (scrollOffset + numItemsDisplayed); i++)
            {
                NodeInfo info = visibleItems[i];
                if (info.Image == null) info.Image = GetItemBitmap(info.Node);
                info.DisplayRectangle = new Rectangle(interior.X, interior.Y + (itemHeight * (i - scrollOffset)), interior.Width, itemHeight);
                int identation = (info.Node.Depth * INDENT_WIDTH);
                info.GlyphRectangle = new Rectangle(identation, info.DisplayRectangle.Top, info.Image.Width - identation, info.Image.Height);
            }
        }

        /// <summary>
        /// Regenerates the items on the dropdown. This method is called whenever 
        /// a significant change occurs to the dropdown, such as a change in the 
        /// tree or changes to the layout of the owning control.
        /// </summary>
        internal void UpdateVisibleItems()
        {
            SuspendLayout();

            // clear bitmap cache
            bitmaps.Clear();

            // populate the collection with the displayable items only
            visibleItems.Clear();
            foreach (ComboTreeNode node in sourceControl.AllNodes)
            {
                if (sourceControl.IsNodeVisible(node)) visibleItems.Add(new NodeInfo(node));
            }

            highlightedItemIndex = Math.Max(0, Math.Min(highlightedItemIndex, visibleItems.Count - 1));

            numItemsDisplayed = Math.Min((dropDownHeight / itemHeight) + 1, visibleItems.Count);
            int maxHeight = ((((dropDownHeight - 2) / itemHeight) + 1) * itemHeight) + 2;

            Size = new Size(sourceControl.ClientRectangle.Width, Math.Min(maxHeight, (visibleItems.Count * itemHeight) + 2));

            // represents the entire paintable area
            interior = ClientRectangle;
            interior.Inflate(-1, -1);

            scrollBarVisible = (numItemsDisplayed < visibleItems.Count);
            if (scrollBarVisible)
            {
                scrollOffset = Math.Max(0, Math.Min(scrollOffset, (visibleItems.Count - numItemsDisplayed)));
                interior.Width -= 17;
                scrollBar.DisplayRectangle = new Rectangle(interior.Right, interior.Top, 17, interior.Height);
                scrollBar.UpArrow = new Rectangle(scrollBar.DisplayRectangle.Location, SCROLLBUTTON_SIZE);
                scrollBar.DownArrow = new Rectangle(new Point(scrollBar.DisplayRectangle.X, scrollBar.DisplayRectangle.Bottom - 17), SCROLLBUTTON_SIZE);
            }

            UpdateScrolling();

            ResumeLayout();
            Invalidate();
        }

        void scrollRepeater_Tick(object sender, EventArgs e)
        {
            // reduce the interval and simulate another click
            scrollRepeater.Interval = 50;
            Point local = PointToClient(Cursor.Position);
            OnMouseDown(new MouseEventArgs(MouseButtons, 1, local.X, local.Y, 0));
        }

        #region Inner Classes

        /// <summary>
        /// Represents the information needed to draw and interact with a node in the drop-down.
        /// </summary>
        private class NodeInfo
        {

            /// <summary>
            /// Gets the node represented by this item.
            /// </summary>
            public ComboTreeNode Node
            {
                get;
                private set;
            }
            /// <summary>
            /// Gets or sets a reference to the bitmap shown beside this item, 
            /// containing the node's image, plus/minus glyph and lines.
            /// </summary>
            public Image Image
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the current bounds of the item in the drop-down.
            /// </summary>
            public Rectangle DisplayRectangle
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the current bounds of the glyph section of the 
            /// item, which is clickable.
            /// </summary>
            public Rectangle GlyphRectangle
            {
                get;
                set;
            }

            /// <summary>
            /// Creates a new instance of the NodeInfo class to represent the 
            /// specified ComboTreeNode.
            /// </summary>
            /// <param name="node"></param>
            public NodeInfo(ComboTreeNode node)
            {
                Node = node;
            }
        }

        /// <summary>
        /// Represents the information needed to draw and interact with the scroll 
        /// bar.
        /// </summary>
        private class ScrollBarInfo
        {

            /// <summary>
            /// Gets or sets the bounds of the entire scrollbar.
            /// </summary>
            public Rectangle DisplayRectangle
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the bounds of the up arrow.
            /// </summary>
            public Rectangle UpArrow
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the bounds of the down arrow.
            /// </summary>
            public Rectangle DownArrow
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the bounds of the 'thumb' button.
            /// </summary>
            public Rectangle Thumb
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Represents the variables which determine the bitmap to draw beside an 
        /// item. In a drop-down with a large number of items, there may be only a
        /// small number of distinct bitmaps. This structure serves as a key to 
        /// aid in identifying the bitmap to use.
        /// </summary>
        private struct BitmapInfo : IEquatable<BitmapInfo>
        {

            /// <summary>
            /// Gets or sets whether the node has children. This is used to 
            /// determine if the plus/minus glyph is drawn.
            /// </summary>
            public bool HasChildren
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets whether the node is the last peer in its branch of 
            /// the tree. These nodes do not draw a connector to their successor.
            /// </summary>
            public bool IsLastPeer
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets whether the node is the first in the entire tree. The 
            /// very first node does not draw a connector to its predecessor.
            /// </summary>
            public bool IsFirst
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the index of the image in the ComboTreeNode's 
            /// ImageList component to draw beside this node.
            /// </summary>
            public int ImageIndex
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the name of the image in the ComboTreeNode's 
            /// ImageList component to draw beside this node.
            /// </summary>
            public string ImageKey
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets the zero-based depth of the node in the tree. This is 
            /// used to calculate indents.
            /// </summary>
            public int NodeDepth
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets whether the node has children and is expanded. This 
            /// will cause a connector to be drawn to the sub-tree.
            /// </summary>
            public bool NodeExpanded
            {
                get;
                set;
            }
            /// <summary>
            /// Gets or sets whether outer vertical connectors are to be drawn for 
            /// each successive parent of the node.
            /// </summary>
            public bool[] VerticalLines
            {
                get;
                set;
            }

            #region IEquatable<BitmapInfo> Members

            /// <summary>
            /// Used as the comparison function in the bitmap cache; ensures that 
            /// bitmaps are only created for distinct combinations of these 
            /// variables.
            /// </summary>
            /// <param name="that"></param>
            /// <returns></returns>
            public bool Equals(BitmapInfo that)
            {
                if (this.HasChildren != that.HasChildren)
                    return false;
                if (this.IsLastPeer != that.IsLastPeer)
                    return false;
                if (this.IsFirst != that.IsFirst)
                    return false;
                if (this.NodeDepth != that.NodeDepth)
                    return false;
                if (this.NodeExpanded != that.NodeExpanded)
                    return false;
                if (this.VerticalLines.Length != that.VerticalLines.Length)
                    return false;
                if (this.ImageIndex != that.ImageIndex)
                    return false;
                if (this.ImageKey != that.ImageKey)
                    return false;

                for (int i = 0; i < VerticalLines.Length; i++)
                {
                    if (this.VerticalLines[i] != that.VerticalLines[i]) return false;
                }

                return true;
            }

            #endregion
        }

        #endregion
    }

}