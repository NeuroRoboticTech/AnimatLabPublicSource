// A ComboBox with a TreeView Drop-Down
// Bradley Smith - 2010/11/04

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnimatGuiCtrls.Controls
{

    /// <summary>
    /// Represents a control which provides ComboBox-like functionality, displaying its 
    /// dropdown items (nodes) in a manner similar to a TreeView control.
    /// </summary>
    [ToolboxItem(true)]
    public class ComboTreeBox : DropDownControlBase {

	    const TextFormatFlags TEXT_FORMAT_FLAGS = TextFormatFlags.TextBoxControl | TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.PathEllipsis;

        private ComboTreeDropDown dropDown;
        private int expandedImageIndex;
        private string expandedImageKey;
        private int imageIndex;
        private string imageKey;
        private ImageList images;
        private bool isUpdating;
        private ComboTreeNodeCollection nodes;
        private string nullValue;
        private string pathSeparator;
        private ComboTreeNode selectedNode;
	    private bool showPath;
        private bool useNodeNamesForPath;

        /// <summary>
        /// Gets the (recursive) superset of the entire tree of nodes contained within the control.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<ComboTreeNode> AllNodes {
            get {
                IEnumerator<ComboTreeNode> e = GetNodesRecursive(nodes, false);
                while (e.MoveNext()) yield return e.Current;
            }
        }
        /// <summary>
        /// Gets or sets the height of the dropdown portion of the control.
        /// </summary>
        [DefaultValue(150), Description("The height of the dropdown portion of the control."), Category("Behavior")]
        public int DropDownHeight {
            get {
                return dropDown.DropDownHeight;
            }
            set {
                dropDown.DropDownHeight = value;
            }
        }
        /// <summary>
        /// Gets or sets whether the dropdown portion of the control is displayed.
        /// </summary>
        [Browsable(false)]
        public override bool DroppedDown {
            get {
                return base.DroppedDown;
            }
            set {
                SetDroppedDown(value, true);
            }
        }
        /// <summary>
        /// Gets or sets the index of the default image to use for nodes when expanded.
        /// </summary>
        [DefaultValue(0), Description("The index of the default image to use for nodes when expanded."), Category("Appearance")]
        public int ExpandedImageIndex {
            get { return expandedImageIndex; }
		    set {
			    expandedImageIndex = value;
			    dropDown.UpdateVisibleItems();
		    }
        }
        /// <summary>
        /// Gets or sets the name of the default image to use for nodes when expanded.
        /// </summary>
        [DefaultValue(""), Description("The name of the default image to use for nodes when expanded."), Category("Appearance")]
        public string ExpandedImageKey {
            get { return expandedImageKey; }
		    set {
			    expandedImageKey = value;
			    dropDown.UpdateVisibleItems();
		    }
        }
        /// <summary>
        /// Gets or sets the index of the default image to use for nodes.
        /// </summary>
        [DefaultValue(0), Description("The index of the default image to use for nodes."), Category("Appearance")]
        public int ImageIndex {
            get { return imageIndex; }
		    set {
			    imageIndex = value;
			    dropDown.UpdateVisibleItems();
		    }
        }
        /// <summary>
        /// Gets or sets the name of the default image to use for nodes.
        /// </summary>
        [DefaultValue(""), Description("The name of the default image to use for nodes."), Category("Appearance")]
        public string ImageKey {
            get { return imageKey; }
		    set {
			    imageKey = value;
			    dropDown.UpdateVisibleItems();
		    }
        }
        /// <summary>
        /// Gets or sets an ImageList component which provides the images displayed beside nodes in the control.
        /// </summary>
        [DefaultValue(null), Description("An ImageList component which provides the images displayed beside nodes in the control."), Category("Appearance")]
        public ImageList Images {
            get { return images; }
		    set {
			    images = value;
			    dropDown.UpdateVisibleItems();
		    }
        }
        /// <summary>
        /// Gets the collection of top-level nodes contained by the control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The collection of top-level nodes contained by the control."), Category("Data")]
        public ComboTreeNodeCollection Nodes {
            get { return nodes; }
        }
        /// <summary>
        /// Gets or sets the text displayed in the editable portion of the control if the SelectedNode property is null.
        /// </summary>
        [DefaultValue(""), Description("The text displayed in the editable portion of the control if the SelectedNode property is null."), Category("Appearance")]
        public string NullValue {
            get { return nullValue; }
            set {
                nullValue = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the path to the selected node.
        /// </summary>
        [DefaultValue(""), Description("The path to the selected node."), Category("Behavior")]
        public string Path {
            get {
                StringBuilder s = new StringBuilder();

                if (selectedNode != null) {
                    s.Append(useNodeNamesForPath ? selectedNode.Name : selectedNode.Text);

                    ComboTreeNode node = selectedNode;
                    while ((node = node.Parent) != null) {
                        s.Insert(0, pathSeparator);
                        s.Insert(0, useNodeNamesForPath ? node.Name : node.Text);
                    }
                }

                return s.ToString();
            }
            set {
                ComboTreeNode select = null;

                string[] parts = value.Split(new string[] { pathSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length; i++) {
                    ComboTreeNodeCollection collection = ((select == null) ? nodes : select.Nodes);
                    if (useNodeNamesForPath) {
                        try {
                            select = collection[parts[i]];
                        }
                        catch (KeyNotFoundException ex) {
                            throw new ArgumentException("Invalid path string.", "value", ex);
                        }
                    }
                    else {
                        bool found = false;
                        foreach (ComboTreeNode node in collection) {
                            if (node.Text.Equals(parts[i], StringComparison.InvariantCultureIgnoreCase)) {
                                select = node;
                                found = true;
                                break;
                            }
                        }
                        if (!found) throw new ArgumentException("Invalid path string.", "value");
                    }
                }

                SelectedNode = select;
            }
        }
        /// <summary>
        /// Gets or sets the string used to separate nodes in the Path property.
        /// </summary>
        [DefaultValue(@"\"), Description("The string used to separate nodes in the path string."), Category("Behavior")]
        public string PathSeparator {
            get { return pathSeparator; }
            set {
                pathSeparator = value;
                if (showPath) Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the node selected in the control.
        /// </summary>
        [Browsable(false)]
        public ComboTreeNode SelectedNode {
            get { return selectedNode; }
            set {
			    if (!OwnsNode(value)) throw new ArgumentException("Node does not belong to this control.", "value");
			    SetSelectedNode(value);
            }
        }
        /// <summary>
	    /// Determines whether the full path to the selected node is displayed in the editable portion of the control.
	    /// </summary>
	    [DefaultValue(false), Description("Determines whether the path to the selected node is displayed in the editable portion of the control."), Category("Appearance")]
	    public bool ShowPath {
		    get { return showPath; }
		    set {
			    showPath = value;
			    Invalidate();
		    }
	    }
	    /// <summary>
	    /// Hides the Text property from the designer.
	    /// </summary>
	    [Browsable(false)]
	    public override string Text {
		    get {
			    return String.Empty;
		    }
		    set {
			    base.Text = String.Empty;
		    }
	    }
	    /// <summary>
	    /// Gets or sets the first visible ComboTreeNode in the drop-down portion of the control.
	    /// </summary>
	    [Browsable(false)]
	    public ComboTreeNode TopNode {
		    get {
			    return dropDown.TopNode;
		    }
		    set {
			    dropDown.TopNode = value;
		    }
	    }
        /// <summary>
        /// Determines whether the Name property of the nodes is used to construct the path string. 
        /// The default behaviour is to use the Text property.
        /// </summary>
        [DefaultValue(false), Description("Determines whether the Name property of the nodes is used to construct the path string. The default behaviour is to use the Text property."), Category("Behavior")]
        public bool UseNodeNamesForPath {
            get { return useNodeNamesForPath; }
            set {
                useNodeNamesForPath = value;
                if (showPath) Invalidate();
            }
        }
	    /// <summary>
	    /// Gets the number of ComboTreeNodes visible in the drop-down portion of the control.
	    /// </summary>
	    [Browsable(false)]
	    public int VisibleCount {
		    get {
			    return dropDown.VisibleCount;
		    }
	    }

	    /// <summary>
	    /// Fired when the SelectedNode property changes.
	    /// </summary>
	    [Description("Occurs when the SelectedNode property changes.")]
	    public event EventHandler SelectedNodeChanged;

	    /// <summary>
	    /// Initalises a new instance of ComboTreeBox.
	    /// </summary>
        public ComboTreeBox() {
		    // default property values
		    nullValue = String.Empty;
		    pathSeparator = @"\";
		    expandedImageIndex = imageIndex = 0;
		    expandedImageKey = imageKey = String.Empty;

		    // nodes collection
            nodes = new ComboTreeNodeCollection(null);
		    nodes.CollectionChanged += new NotifyCollectionChangedEventHandler(nodes_CollectionChanged);
        
		    // dropdown portion
		    dropDown = new ComboTreeDropDown(this);
		    dropDown.Opened += new EventHandler(dropDown_Opened);
		    dropDown.Closed += new ToolStripDropDownClosedEventHandler(dropDown_Closed);
		    dropDown.UpdateVisibleItems();
        }

	    /// <summary>
	    /// Prevents the dropdown portion of the control from being updated until the EndUpdate method is called.
	    /// </summary>
	    public void BeginUpdate() {
		    isUpdating = true;
	    }

        /// <summary>
        /// Collapses all nodes in the tree for when the dropdown portion of the control is reopened.
        /// </summary>
        public void CollapseAll() {
            foreach (ComboTreeNode node in AllNodes) node.Expanded = false;
        }

        /// <summary>
        /// Disposes of the control and its dropdown.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing) {
            if (disposing) dropDown.Dispose();
            base.Dispose(disposing);
        }

	    /// <summary>
	    /// Updates the dropdown portion of the control after being suspended by the BeginUpdate method.
	    /// </summary>
	    public void EndUpdate() {
		    isUpdating = false;
		    if (!OwnsNode(selectedNode)) SetSelectedNode(null);
		    dropDown.UpdateVisibleItems();
	    }

	    /// <summary>
	    /// Expands all nodes in the tree for when the dropdown portion of the control is reopened.
	    /// </summary>
	    public void ExpandAll() {
		    foreach (ComboTreeNode node in AllNodes) if (node.Nodes.Count > 0) node.Expanded = true;
	    }

	    /// <summary>
	    /// Returns the next displayable node, relative to the selected node.
	    /// </summary>
	    /// <returns></returns>
	    private ComboTreeNode GetNextDisplayedNode() {
		    bool started = false;
		    IEnumerator<ComboTreeNode> e = GetNodesRecursive(nodes, false);
		    while (e.MoveNext()) {
			    if (started || (selectedNode == null)) {
				    if (IsNodeVisible(e.Current)) return e.Current;
			    }
			    else if (e.Current == selectedNode) {
				    started = true;
			    }
		    }

		    return null;
	    }

	    /// <summary>
	    /// Returns the previous displayable node, relative to the selected node.
	    /// </summary>
	    /// <returns></returns>
	    private ComboTreeNode GetPrevDisplayedNode() {
		    bool started = false;
		    IEnumerator<ComboTreeNode> e = GetNodesRecursive(nodes, true);
		    while (e.MoveNext()) {
			    if (started || (selectedNode == null)) {
				    if (IsNodeVisible(e.Current)) return e.Current;
			    }
			    else if (e.Current == selectedNode) {
				    started = true;
			    }
		    }

		    return null;
	    }

        /// <summary>
        /// Returns the image referenced by the specified node in the ImageList component associated with this control.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal Image GetNodeImage(ComboTreeNode node) {
            if ((images != null) && (node != null)) {
                if (node.Expanded) {
                    if (images.Images.ContainsKey(node.ExpandedImageKey))
                        return images.Images[node.ExpandedImageKey];		// node's key
                    else if (node.ExpandedImageIndex >= 0)
                        return images.Images[node.ExpandedImageIndex];		// node's index
                    else if (images.Images.ContainsKey(expandedImageKey))
                        return images.Images[expandedImageKey];				// default key
                    else if (expandedImageIndex >= 0)
                        return images.Images[expandedImageIndex];			// default index
                }
                else {
                    if (images.Images.ContainsKey(node.ImageKey))
                        return images.Images[node.ImageKey];		// node's key
                    else if (node.ImageIndex >= 0)
                        return images.Images[node.ImageIndex];		// node's index
                    else if (images.Images.ContainsKey(imageKey))
                        return images.Images[imageKey];				// default key
                    else if (imageIndex >= 0)
                        return images.Images[imageIndex];			// default index
                }
            }

            return null;
        }

        /// <summary>
        /// Helper method for the AllNodes property.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private IEnumerator<ComboTreeNode> GetNodesRecursive(ComboTreeNodeCollection collection, bool reverse) {
		    if (!reverse) {
			    for (int i = 0; i < collection.Count; i++) {
				    yield return collection[i];
				    IEnumerator<ComboTreeNode> e = GetNodesRecursive(collection[i].Nodes, reverse);
				    while (e.MoveNext()) yield return e.Current;
			    }
		    }
		    else {
			    for (int i = (collection.Count - 1); i >= 0; i--) {
				    IEnumerator<ComboTreeNode> e = GetNodesRecursive(collection[i].Nodes, reverse);
				    while (e.MoveNext()) yield return e.Current;
				    yield return collection[i];
			    }
		    }
        }

	    /// <summary>
	    /// Determines whether the specified node should be displayed.
	    /// </summary>
	    /// <param name="node"></param>
	    /// <returns></returns>
	    internal bool IsNodeVisible(ComboTreeNode node) {
		    bool displayed = true;
		    ComboTreeNode parent = node;
		    while ((parent = parent.Parent) != null) {
			    if (!parent.Expanded) {
				    displayed = false;
				    break;
			    }
		    }
		    return displayed;
	    }

        /// <summary>
        /// Toggles the visibility of the dropdown portion of the control.
        /// </summary>
        /// <param name="e"></param>
	    protected override void OnMouseClick(MouseEventArgs e) {
		    base.OnMouseClick(e);
		    if (e.Button == System.Windows.Forms.MouseButtons.Left) DroppedDown = !DroppedDown;
	    }

	    /// <summary>
	    /// Scrolls between adjacent nodes, or scrolls the drop-down portion of 
	    /// the control in response to the mouse wheel.
	    /// </summary>
	    /// <param name="e"></param>
	    protected override void OnMouseWheel(MouseEventArgs e) {
		    base.OnMouseWheel(e);
		    if (DroppedDown)
			    dropDown.ScrollDropDown(-(e.Delta / 120) * SystemInformation.MouseWheelScrollLines);
		    else if (e.Delta > 0) {
			    ComboTreeNode prev = GetPrevDisplayedNode();
			    if (prev != null) SetSelectedNode(prev);
		    }
		    else if (e.Delta < 0) {
			    ComboTreeNode next = GetNextDisplayedNode();
			    if (next != null) SetSelectedNode(next);
		    }
	    }

	    /// <summary>
	    /// Updates the dropdown's font when the control's font changes.
	    /// </summary>
	    /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            dropDown.Font = Font;
        }

	    /// <summary>
	    /// Handles keyboard shortcuts.
	    /// </summary>
	    /// <param name="e"></param>
	    protected override void OnKeyDown(KeyEventArgs e) {
		    e.Handled = e.SuppressKeyPress = true;

		    if (e.Alt && (e.KeyCode == Keys.Down)) {
			    DroppedDown = true;
		    }
		    else if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left)) {
			    ComboTreeNode prev = GetPrevDisplayedNode();
			    if (prev != null) SetSelectedNode(prev);
		    }
		    else if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Right)) {
			    ComboTreeNode next = GetNextDisplayedNode();
			    if (next != null) SetSelectedNode(next);
		    }
		    else {
			    e.Handled = e.SuppressKeyPress = false;
		    }
		
		    base.OnKeyDown(e);		
	    }

        /// <summary>
        /// Closes the dropdown portion of the control when it loses focus.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            if (!dropDown.Focused) dropDown.Close();
        }

        /// <summary>
        /// Paints the selected node in the control.
        /// </summary>
        /// <param name="e"></param>
	    protected override void OnPaintContent(DropDownPaintEventArgs e) {
		    base.OnPaintContent(e);

            Image img = GetNodeImage(selectedNode);
            string text = (selectedNode != null) ? ((showPath) ? Path : selectedNode.Text) : nullValue;

            Rectangle imgBounds = (img == null) ? new Rectangle(1,0,0,0) : new Rectangle(4, e.Bounds.Height / 2 - img.Height / 2, img.Width, img.Height);
		    Rectangle txtBounds = new Rectangle(imgBounds.Right, 0, e.Bounds.Width - imgBounds.Right - 3, e.Bounds.Height);

            if (img != null) e.Graphics.DrawImage(img, imgBounds);

            TextRenderer.DrawText(e.Graphics, text, Font, txtBounds, ForeColor, TEXT_FORMAT_FLAGS);

		    // focus rectangle
		    if (Focused && ShowFocusCues && !DroppedDown) e.DrawFocusRectangle();
        }

        /// <summary>
        /// Raises the SelectedNodeChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedNodeChanged(EventArgs e) {
            if (SelectedNodeChanged != null) SelectedNodeChanged(this, e);
        }

	    /// <summary>
	    /// Determines whether the specified node belongs to this ComboTreeBox, and 
	    /// hence is a valid selection. For the purposes of this method, a null 
	    /// value is always a valid selection.
	    /// </summary>
	    /// <param name="node"></param>
	    /// <returns></returns>
	    private bool OwnsNode(ComboTreeNode node) {
		    if (node == null) return true;

		    ComboTreeNode parent = node;
		    while (parent.Parent != null) parent = parent.Parent;
		    return nodes.Contains(parent);
	    }

        /// <summary>
        /// Sets the value of the DroppedDown property, optionally without raising any events.
        /// </summary>
        /// <param name="droppedDown"></param>
        /// <param name="raiseEvents"></param>
        internal void SetDroppedDown(bool droppedDown, bool raiseEvents) {
            base.DroppedDown = droppedDown;

            if (raiseEvents) {
                if (droppedDown)
                    dropDown.Open();
                else
                    dropDown.Close();
            }
        }

	    /// <summary>
	    /// Sets the value of the SelectedNode property and raises the SelectedNodeChanged event.
	    /// </summary>
	    /// <param name="node"></param>
	    private void SetSelectedNode(ComboTreeNode node) {
		    if (selectedNode != node) {
			    selectedNode = node;
			    Invalidate();
			    OnSelectedNodeChanged(EventArgs.Empty);
		    }
	    }

        /// <summary>
	    /// Sorts the contents of the tree using the default comparer.
	    /// </summary>
	    public void Sort() {
		    Sort(null);
	    }

	    /// <summary>
	    /// Sorts the contents of the tree using the specified comparer.
	    /// </summary>
	    /// <param name="comparer"></param>
	    public void Sort(IComparer<ComboTreeNode> comparer) {
		    bool oldIsUpdating = isUpdating;
		    isUpdating = true;
		    nodes.Sort(comparer);
		    if (!oldIsUpdating) EndUpdate();
	    }

	    void dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
		    OnDropDownClosed(EventArgs.Empty);
	    }

	    void dropDown_Opened(object sender, EventArgs e) {
		    OnDropDown(EventArgs.Empty);
	    }

	    void nodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
		    if (!isUpdating) {
			    // verify that selected node still belongs to the tree
			    if (!OwnsNode(selectedNode)) SetSelectedNode(null);

			    // rebuild the view
			    dropDown.UpdateVisibleItems();
		    }
	    }
    }

}