#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       CreateCommand.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Draw.Command
{
    using System.Collections;

    class XnaCreateCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _graphicsList;
        private readonly XnaDrawObject _shape;

        #endregion Fields

        #region Constructors

        public XnaCreateCommand(XnaDrawObject shape, ArrayList graphicsList)
        {
            _shape = shape;
            _graphicsList = graphicsList;
        }

        //Disable default constructor
        private XnaCreateCommand()
        {
        }

        #endregion Constructors

        #region Methods

        public void Execute()
        {
            _graphicsList.Insert(0, _shape);
        }

        public void UnExecute()
        {
            _graphicsList.Remove(_shape);
        }

        #endregion Methods
    }
}