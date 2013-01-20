/* 
 * Copyright (c) 2003-2005, University of Maryland
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided
 * that the following conditions are met:
 * 
 *		Redistributions of source code must retain the above copyright notice, this list of conditions
 *		and the following disclaimer.
 * 
 *		Redistributions in binary form must reproduce the above copyright notice, this list of conditions
 *		and the following disclaimer in the documentation and/or other materials provided with the
 *		distribution.
 * 
 *		Neither the name of the University of Maryland nor the names of its contributors may be used to
 *		endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * Piccolo was written at the Human-Computer Interaction Laboratory www.cs.umd.edu/hcil by Jesse Grosjean
 * and ported to C# by Aaron Clamage under the supervision of Ben Bederson.  The Piccolo website is
 * www.cs.umd.edu/hcil/piccolo.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using XnaPiccolo;
using XnaPiccolo.Nodes;
using XnaPiccolo.Util;
using XnaPiccoloX.Handles;
using Microsoft.Xna.Framework;

namespace XnaPiccoloFeatures 
{
	public class LayoutExample : XnaPiccoloX.PForm 
    {
		private System.ComponentModel.IContainer components = null;

		public LayoutExample() 
        {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() 
        {
			PNode layoutNode = new LayoutNode();

			layoutNode.Brush = Color.Red;

			// add some children to the layout node.
			for (int i = 0; i < 100; i++) 
            {
				// create child to add to the layout node.
				PNode each = PPath.CreateRectangle(0, 0, 100, 80);

				// add the child to the layout node.		
				layoutNode.AddChild(each);
			}
	
			PBoundsHandle.AddBoundsHandlesTo(layoutNode.GetChild(0));
		
			// add layoutNode to the root so it will be displayed.
			Canvas.Layer.AddChild(layoutNode);
		}

		class LayoutNode : PNode 
        {
			public override void LayoutChildren() 
            {
				float xOffset = 0;
				float yOffset = 0;

				PNodeList children = this.ChildrenReference;
				foreach (PNode each in children) 
                {
					each.SetOffset(xOffset - each.X, yOffset);
					xOffset += each.Width;
				}
			}

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) 
        {
			if( disposing ) 
            {
				if (components != null) 
                {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			// 
			// LayoutExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "LayoutExample";
			this.Text = "LayoutExample";

		}
		#endregion
	}
}