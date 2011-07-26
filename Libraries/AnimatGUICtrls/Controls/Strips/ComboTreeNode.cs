// A ComboBox with a TreeView Drop-Down
// Bradley Smith - 2010/11/04

using System;
using System.ComponentModel;
using System.Drawing;

namespace AnimatGuiCtrls.Controls
{

    /// <summary>
    /// Represents a node in the ComboTreeBox. A node may have a name, text, font style, image and 
    /// may contain child nodes. If so, it can be expanded or collapsed.
    /// </summary>
    [DefaultProperty("Text")]
    public class ComboTreeNode : IComparable<ComboTreeNode>
    {

        private string name;
        private ComboTreeNodeCollection nodes;
        private string text;
        private FontStyle fontStyle;
        private int imageIndex;
        private string imageKey;
        private bool expanded;
        private int expandedImageIndex;
        private string expandedImageKey;
        private ComboTreeNode parent;
        private object tag;

        /// <summary>
        /// Gets or sets the node that owns this node, or null for a top-level node.
        /// </summary>
        [Browsable(false)]
        public ComboTreeNode Parent
        {
            get { return parent; }
            internal set { parent = value; }
        }
        /// <summary>
        /// Gets or sets the text displayed on the node.
        /// </summary>
        [DefaultValue("ComboTreeNode"), Description("The text displayed on the node."), Category("Appearance")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        /// <summary>
        /// Gets or sets the font style to use when painting the node.
        /// </summary>
        [DefaultValue(FontStyle.Regular), Description("The font style to use when painting the node."), Category("Appearance")]
        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set { fontStyle = value; }
        }
        /// <summary>
        /// Gets or sets the index of the image (in the ImageList on the ComboTreeBox control) to use for this node.
        /// </summary>
        [DefaultValue(-1), Description("The index of the image (in the ImageList on the ComboTreeBox control) to use for this node."), Category("Appearance")]
        public int ImageIndex
        {
            get { return imageIndex; }
            set { imageIndex = value; }
        }
        /// <summary>
        /// Gets or sets the name of the image to use for this node.
        /// </summary>
        [DefaultValue(""), Description("The name of the image to use for this node."), Category("Appearance")]
        public string ImageKey
        {
            get { return imageKey; }
            set { imageKey = value; }
        }
        /// <summary>
        /// Gets or sets whether the node is expanded (i.e. its child nodes are visible). Changes are not reflected in the dropdown portion of the 
        /// control until the next time it is opened.
        /// </summary>
        [Browsable(false)]
        public bool Expanded
        {
            get { return expanded; }
            set { expanded = value; }
        }
        /// <summary>
        /// Gets or sets the index of the image to use for this node when expanded.
        /// </summary>
        [DefaultValue(-1), Description("The index of the image to use for this node when expanded."), Category("Appearance")]
        public int ExpandedImageIndex
        {
            get { return expandedImageIndex; }
            set { expandedImageIndex = value; }
        }
        /// <summary>
        /// Gets or sets the name of the image to use for this node when expanded.
        /// </summary>
        [DefaultValue(""), Description("The name of the image to use for this node when expanded."), Category("Appearance")]
        public string ExpandedImageKey
        {
            get { return expandedImageKey; }
            set { expandedImageKey = value; }
        }
        /// <summary>
        /// Gets a collection of the child nodes for this node.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The collection of the child nodes for this node."), Category("Data")]
        public ComboTreeNodeCollection Nodes
        {
            get { return nodes; }
        }
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        [Description("The name of the node."), DefaultValue(""), Category("Design")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Determines the zero-based depth of the node, relative to the ComboTreeBox control.
        /// </summary>
        [Browsable(false)]
        public int Depth
        {
            get
            {
                int depth = 0;
                ComboTreeNode node = this;
                while ((node = node.parent) != null) depth++;
                return depth;
            }
        }
        /// <summary>
        /// Gets or sets a user-defined object associated with this ComboTreeNode.
        /// </summary>
        [Description("User-defined object associated with this ComboTreeNode."), DefaultValue(null), Category("Data")]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Initialises a new instance of ComboTreeNode using default (empty) values.
        /// </summary>
        public ComboTreeNode()
        {
            nodes = new ComboTreeNodeCollection(this);
            name = text = String.Empty;
            fontStyle = FontStyle.Regular;
            expandedImageIndex = imageIndex = -1;
            expandedImageKey = imageKey = String.Empty;
            expanded = false;
        }

        /// <summary>
        /// Initialises a new instance of ComboTreeNode with the specified text.
        /// </summary>
        /// <param name="text"></param>
        public ComboTreeNode(string text)
            : this()
        {
            this.text = text;
        }

        /// <summary>
        /// Initialises a new instance of ComboTreeNode with the specified name and text.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public ComboTreeNode(string name, string text)
            : this()
        {
            this.text = text;
            this.name = name;
        }

        /// <summary>
        /// Initialises a new instance of ComboTreeNode with the specified name, text, and tag.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public ComboTreeNode(string name, string text, object tag)
            : this()
        {
            this.text = text;
            this.name = name;
            this.tag = tag;
            this.expanded = true;
        }

        /// <summary>
        /// Returns a string representation of this ComboTreeNode.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Name=\"{0}\", Text=\"{1}\"", name, text);
        }

        #region IComparable<ComboTreeNode> Members

        /// <summary>
        /// Compares two ComboTreeNode objects using a culture-invariant, case-insensitive comparison of the Text property.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ComboTreeNode other)
        {
            return StringComparer.InvariantCultureIgnoreCase.Compare(this.text, other.text);
        }

        #endregion
    }

}