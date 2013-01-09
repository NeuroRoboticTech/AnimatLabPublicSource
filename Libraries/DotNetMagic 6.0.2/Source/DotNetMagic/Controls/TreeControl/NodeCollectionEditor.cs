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
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Information for editing the NodeCollection types.
	/// </summary>
    public class NodeCollectionEditor : UITypeEditor
    {
        /// <summary>
        /// Gets the editor style used by the EditValue method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <returns>A UITypeEditorEditStyle enumeration value that indicates the style of editor.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
                return UITypeEditorEditStyle.Modal;
            else
                return base.GetEditStyle(context);
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by GetEditStyle.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <param name="provider">An IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (context.Instance != null) && (provider != null))
            {
                // Convert to the correct class.
                NodeCollection nodes = value as NodeCollection;

                // Create the dialog used to edit the nodes
                NodeCollectionDialog dialog = new NodeCollectionDialog(nodes);

                // Give user the chance to modify the nodes
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Reflect changes back into original copy and generate appropriate
                    // component changes to designer to reflect these changes
                    SynchronizeCollections(nodes, dialog.Nodes, context);

                    // Need to ensure the treecontrol is updated
                    nodes.TreeControl.MarkAllNodeSizesDirty();

                    // Notify container that value has been changed
                    context.OnComponentChanged();
                }
            }

            // Return the original value
            return value;
        }

        private void SynchronizeCollections(NodeCollection orig,
                                            NodeCollection copy,
                                            ITypeDescriptorContext context)
        {
            // Make a note of all original nodes that are still in copy
            Hashtable both = new Hashtable();

            // First pass, scan looking for nodes that are in original and copy
            foreach (Node copyChild in copy)
            {
                // Does this node have an back pointer to its original?
                if (copyChild.Original != null)
                {
                    // Then make a note that it is in both collections
                    both.Add(copyChild.Original, copyChild.Original);

                    // Update the original from the copy
                    copyChild.Original.UpdateInstance(copyChild);
                }
            }

            int origCount = orig.Count;

            // Second pass, remove nodes in the original but not in the copy
            for (int i = 0; i < origCount; i++)
            {
                // Get access to the indexed node from original
                Node origChild = orig[i];

                // If not in the found collection...
                if (!both.ContainsKey(origChild))
                {
                    // ...then it has been removed from the copy, so delete it
                    orig.Remove(origChild);

                    // Must remove from context container so it is removed from designer tray
                    context.Container.Remove(origChild as IComponent);

                    // Now we need to remove all the tree of nodes below it from the component tray
                    RemoveChildComponents(origChild, context);

                    // Reduce the count and index to reflect change in collection contents
                    --i;
                    --origCount;
                }
            }

            int copyCount = copy.Count;

            // Third pass, add new nodes from copy but not in original
            for (int i = 0; i < copyCount; i++)
            {
                // Get access to the indexed node from copy
                Node copyChild = copy[i];

                // If this node is a new one then it will not have an 'original' property
                if (copyChild.Original == null)
                {
                    // Add this node into the original at indexed position
                    orig.Insert(i, copyChild);

                    // Must add into context container so it is added to the designer tray
                    context.Container.Add(copyChild as IComponent);

                    // Now we need to add all the tree of nodes below it to the component tray
                    AddChildComponents(copyChild, context);
                }
            }

            // Fourth pass, process all children
            foreach (Node copyChild in copy)
            {
                // Does this node has an back pointer to its original?
                if (copyChild.Original != null)
                    SynchronizeCollections(copyChild.Original.Nodes, copyChild.Nodes, context);
            }
        }

        private void AddChildComponents(Node parentNode, ITypeDescriptorContext context)
        {
            // Add each child node in turn
            foreach (Node childNode in parentNode.Nodes)
            {
                // Must add into context container so it is added to the designer tray
                context.Container.Add(childNode as IComponent);

                // Now we need to add all the tree of nodes below it to the component tray
                AddChildComponents(childNode, context);
            }
        }

        private void RemoveChildComponents(Node parentNode, ITypeDescriptorContext context)
        {
            // Remove each child node in turn
            foreach (Node childNode in parentNode.Nodes)
            {
                // Must remove from context container so it is removed from designer tray
                context.Container.Remove(childNode as IComponent);

                // Now we need to remove all the tree of nodes below it from the component tray
                RemoveChildComponents(childNode, context);
            }
        }
    }
}
