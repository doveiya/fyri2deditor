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
using XnaPiccolo.Event;
using XnaPiccolo.Nodes;
using XnaPiccoloX;
using XnaPiccoloX.Nodes;
using Microsoft.Xna.Framework;

namespace XnaPiccoloFeatures 
{
	/// <summary>
	/// This example shows how to create a composite node. A composite node is
	/// a group of nodes that behave as a single node when interacted with.
	/// </summary>
	public class CompositeExample : PForm 
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CompositeExample() 
        {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() 
        {
			PComposite composite = new PComposite();

			PNode circle = PPath.CreateEllipse(0, 0, 100, 100);
			PNode rectangle = PPath.CreateRectangle(50, 50, 100, 100);
			PNode text = new PText("Hello world!");
		
			composite.AddChild(circle);
			composite.AddChild(rectangle);
			composite.AddChild(text);

			rectangle.RotateBy(45);
			rectangle.Brush = Color.Red;

			text.ScaleBy(2.0f);
			text.Brush = Color.Green;
		
			Canvas.Layer.AddChild(composite);
			Canvas.RemoveInputEventListener(Canvas.PanEventHandler);
			Canvas.AddInputEventListener(new PDragEventHandler());
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "CompositeExample";
		}
		#endregion
	}
}