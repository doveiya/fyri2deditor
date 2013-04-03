using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xna2dEditor
{
    public class TreeViewHelper
    {
        public static void Copy(TreeView treeview1, TreeView treeview2)
        {
            TreeNode newTn;
            foreach (TreeNode tn in treeview1.Nodes)
            {
                newTn = new TreeNode(tn.Text);
                newTn.Tag = tn.Tag;
                CopyChilds(newTn, tn);
                treeview2.Nodes.Add(newTn);
            }
        }

        public static void CopyChilds(TreeNode parent, TreeNode willCopied)
        {
            TreeNode newTn;
            foreach (TreeNode tn in willCopied.Nodes)
            {
                newTn = new TreeNode(tn.Text);
                newTn.Tag = tn.Tag;
                CopyChilds(newTn, tn);
                parent.Nodes.Add(newTn);
            }
        } 
    }
}
