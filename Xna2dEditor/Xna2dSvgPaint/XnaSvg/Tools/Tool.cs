#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       Tool.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Fyri2dEditor
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Base class for all drawing tools
    /// </summary>
    public abstract class Tool
    {
        #region Fields

        /// <summary>
        /// If false the tool is not yet completed
        /// </summary>
        public Boolean IsComplete;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Left nous button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(Xna2dDrawArea drawArea, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseMove(Xna2dDrawArea drawArea, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseUp(Xna2dDrawArea drawArea, MouseEventArgs e)
        {
        }

        public virtual void ToolActionCompleted()
        {
        }

        #endregion Methods
    }
}