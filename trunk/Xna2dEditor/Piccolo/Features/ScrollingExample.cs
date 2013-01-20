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
using XnaPiccoloX.Components;
using Xna2dEditor;
using Microsoft.Xna.Framework;

namespace XnaPiccoloFeatures 
{
	public class ScrollingExample : XnaPiccoloX.PForm 
    {
		private System.ComponentModel.IContainer components = null;
		private static readonly String WINDOW_LABEL= "Window Scrolling";
		private static readonly String DOCUMENT_LABEL = "Document Scrolling";

		private PScrollDirector windowSD;
		private PScrollDirector documentSD;

		public ScrollingExample() 
        {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() 
        {
			AutoScrollCanvas = true;
			windowSD = ScrollControl.ScrollDirector;
			documentSD = new DocumentScrollDirector();

			ToolBar toolBar = new ToolBar();
			ToolBarButton btnWindow = new ToolBarButton(WINDOW_LABEL);
			ToolBarButton btnDocument = new ToolBarButton(DOCUMENT_LABEL);
			toolBar.Buttons.Add(btnWindow);
			toolBar.Buttons.Add(btnDocument);
			toolBar.ButtonClick += new ToolBarButtonClickEventHandler(toolBar_ButtonClick);
			this.Controls.Add(toolBar);

			ScrollControl.Bounds = new System.Drawing.Rectangle(ClientRectangle.X, toolBar.Bottom, ScrollControl.Width, ScrollControl.Height - toolBar.Height);

			// Make some rectangles on the surface so we can see where we are
			for (int x = 0; x < 20; x++) 
            {
				for (int y = 0; y < 20; y++) 
                {
					if (((x + y) % 2) == 0) 
                    {
						PPath path = PPath.CreateRectangle(50 * x, 50 * y, 40, 40);
						path.Brush = Color.Blue;
                        path.Pen = System.Drawing.Pens.Black;
						Canvas.Layer.AddChild(path);
					}
					else if (((x + y) % 2) == 1) 
                    {
						PPath path = PPath.CreateEllipse(50 * x, 50 * y, 40, 40);
                        path.Brush = Color.Blue;
                        path.Pen = System.Drawing.Pens.Black;
						Canvas.Layer.AddChild(path);
					}
				}
			}
		}

		protected void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e) 
        {
			if (e.Button.Text == WINDOW_LABEL) 
            {
				ScrollControl.ScrollDirector = windowSD;
				ScrollControl.UpdateScrollbars();
			} 
            else 
            {
				ScrollControl.ScrollDirector = documentSD;
				ScrollControl.UpdateScrollbars();
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
			// ScrollingExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "ScrollingExample";
			this.Text = "ScrollingExample";

		}
		#endregion
	}

	public class DocumentScrollDirector : PDefaultScrollDirector 
    {

		public override PointFx GetViewPosition(RectangleFx bounds) 
        {
			PointFx pos = PointFx.Empty;
			if (camera != null) 
            {
				// First we compute the union of all the layers
				RectangleFx layerBounds = camera.UnionOfLayerFullBounds;

				// Then we put the bounds into camera coordinates and
				// union the camera bounds
				layerBounds = camera.ViewToLocal(layerBounds);
                layerBounds = RectangleFxtensions.Union(layerBounds, bounds);

				// Rather than finding the distance from the upper left corner
				// of the window to the upper left corner of the document -
				// we instead find the distance from the lower right corner
				// of the window to the upper left corner of the document
				// THEN we measure the offset from the lower right corner
				// of the document
				int x = (int) (layerBounds.Width - (bounds.X + bounds.Width - layerBounds.X) + 0.5);
				int y = (int) (layerBounds.Height - (bounds.Y + bounds.Height - layerBounds.Y) + 0.5);

				// Make sure the value isn't greater than the maximum
				x = (int)Math.Min(x, layerBounds.Width-1);
				y = (int)Math.Min(y, layerBounds.Height-1);

				pos = new PointFx(x, y);
			}

			return pos;
		}

		public override void SetViewPosition(float x, float y) 
        {
			if (camera != null) 
            {
				// If a scroll is in progress - we ignore new scrolls -
				// if we didn't, since the scrollbars depend on the camera location
				// we can end up with an infinite loo
				if (!scrollInProgress) 
                {
					scrollInProgress = true;

					// Get the union of all the layers' bounds
					RectangleFx layerBounds = camera.UnionOfLayerFullBounds;

					Matrix matrix = camera.ViewMatrix;
					layerBounds = MatrixExtensions.Transform(matrix, layerBounds);

					// Union the camera view bounds
					RectangleFx viewBounds = camera.Bounds;
                    layerBounds = RectangleFxtensions.Union(layerBounds, viewBounds);

					// Now find the new view position in view coordinates -
					// This is basically the distance from the lower right
					// corner of the window to the upper left corner of the
					// document 
					// We then measure the offset from the lower right corner
					// of the document
					PointFx newPoint =
						new PointFx(
						layerBounds.X + layerBounds.Width - (x + viewBounds.Width),
						layerBounds.Y + layerBounds.Height - (y + viewBounds.Height));

					// Now transform the new view position into global coords
					newPoint = camera.LocalToView(newPoint);

					// Compute the new matrix values to put the camera at the
					// correct location
					float[] elements = MatrixExtensions.GetElements(matrix);
					elements[4] = - (elements[0] * newPoint.X + elements[1] * newPoint.Y);
					elements[5] = - (elements[2] * newPoint.X + elements[3] * newPoint.Y);

					matrix = MatrixExtensions.SetElements(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);

					// Now actually set the camera's transform
					camera.ViewMatrix = matrix;
					scrollInProgress = false;
				}
			}
		}
	}
}