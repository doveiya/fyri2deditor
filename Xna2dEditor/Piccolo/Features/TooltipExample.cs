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
using System.Drawing;
using System.Windows.Forms;

using XnaPiccolo;
using XnaPiccolo.Event;
using XnaPiccolo.Nodes;
using Xna2dEditor;

namespace XnaPiccoloFeatures {
	public class TooltipExample : XnaPiccoloX.PForm {
		private System.ComponentModel.IContainer components = null;
		private PText tooltipNode;

		public TooltipExample() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() {
			PNode n1 = PPath.CreateEllipse(0, 0, 100, 100);
			PNode n2 = PPath.CreateRectangle(300, 200, 100, 100);
		
			n1.Tag = "node 1";
			n2.Tag = "node 2";
			Canvas.Layer.AddChild(n1);
			Canvas.Layer.AddChild(n2);
		
			PCamera camera = Canvas.Camera;
			tooltipNode = new PText();
		
			tooltipNode.Pickable = false;
			camera.AddChild(tooltipNode);

			PBasicInputEventHandler tipEventHandler = new PBasicInputEventHandler();
			tipEventHandler.MouseMove = new MouseMoveDelegate(MouseMoveHandler);
			tipEventHandler.MouseDrag = new MouseDragDelegate(MouseDragHandler);
			camera.AddInputEventListener(tipEventHandler);
		}

		public void MouseMoveHandler(object sender, PInputEventArgs e) {
			UpdateToolTip(e);
		}

		public void MouseDragHandler(object sender, PInputEventArgs e) {
			UpdateToolTip(e);
		}

		public void UpdateToolTip(PInputEventArgs e) {
			PNode n = e.InputManager.MouseOver.PickedNode;
			String tooltipString = (String) n.Tag;
			PointFx p = e.CanvasPosition;

			p = e.Path.CanvasToLocal(p, Canvas.Camera);
				
			tooltipNode.Text = tooltipString;
			tooltipNode.SetOffset(p.X + 8, p.Y - 8);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			// TooltipExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "TooltipExample";
			this.Text = "ToolTipExample";

		}
		#endregion
	}
}