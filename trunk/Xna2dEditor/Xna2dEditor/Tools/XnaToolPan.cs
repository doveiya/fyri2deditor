#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolPan.cs
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
    using System.Reflection;
    using System.Windows.Forms;
    using Xna2dEditor;

    /// <summary>
    /// Rectangle tool
    /// </summary>
    public class ToolPan : ToolObject
    {
        #region Fields

        readonly Cursor _closedHand;
        readonly Cursor _openHand;

        #endregion Fields

        #region Constructors

        public ToolPan()
        {
            _openHand = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.pan.cur"));
            _closedHand = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("Xna2dEditor.Xna2dSvgPaint.XnaSvg.Resources.pan_close.cur"));
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseDown(XnaToolUser drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = _closedHand;
        }

        public override void OnMouseMove(XnaToolUser drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = e.Button == MouseButtons.Left ? _closedHand : _openHand;
        }

        public override void OnMouseUp(XnaToolUser drawArea, MouseEventArgs e)
        {
            Cursor = _openHand;
        }

        #endregion Methods
    }
}