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
using XnaPiccoloX.Events;
using Xna2dEditor;
using Microsoft.Xna.Framework;

namespace XnaPiccoloFeatures 
{
	public class GroupExample : XnaPiccoloX.PForm 
    {
		private System.ComponentModel.IContainer components = null;

		public GroupExample() 
        {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() 
        {
			Canvas.RemoveInputEventListener(Canvas.PanEventHandler);

			// Create a decorator group that is NOT volatile		
			DecoratorGroup dg = new DecoratorGroup();
			dg.Brush = Color.Magenta;
		
			// Put some nodes under the group for it to decorate
			PPath p1 = PPath.CreateEllipse(25,25,75,75);
			p1.Brush = Color.Red;
			PPath p2 = PPath.CreateRectangle(125,75,50,50); 
			p2.Brush = Color.Blue;

			// Add everything to the Piccolo hierarchy
			dg.AddChild(p1);
			dg.AddChild(p2);
			Canvas.Layer.AddChild(dg);				

			// Create a decorator group that IS volatile
			VolatileDecoratorGroup vdg = new VolatileDecoratorGroup(Canvas.Camera);
			vdg.Brush = Color.Cyan;
		
			// Put some nodes under the group for it to decorate
			PPath p3 = PPath.CreateEllipse(275,175,50,50);
			p3.Brush = Color.Blue;
			PPath p4 = PPath.CreateRectangle(175,175,75,75); 
			p4.Brush = Color.Green;
		
			// Add everything to the Piccolo hierarchy
			vdg.AddChild(p3);
			vdg.AddChild(p4);
			Canvas.Layer.AddChild(vdg);						

			// Create a selection handler so we can see that the decorator actually works
			PNodeList selectableParents = new PNodeList();
			selectableParents.Add(dg);
			selectableParents.Add(vdg);

			PSelectionEventHandler ps = new PSelectionEventHandler(Canvas.Layer,selectableParents);
			Canvas.AddInputEventListener(ps);
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
			// GroupExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "GroupExample";
			this.Text = "GroupExample";

		}
		#endregion
	}

	class DecoratorGroup : PNode 
    {
		readonly static int INDENT = 10;

		RectangleFx cachedChildBounds = RectangleFx.Empty;
		RectangleFx comparisonBounds = RectangleFx.Empty;

		protected override void Paint(PPaintContext paintContext) 
        {
			if (Brush != Color.Transparent) 
            {
				XnaGraphics g = paintContext.Graphics;

				RectangleFx bounds = UnionOfChildrenBounds;
				bounds = new RectangleFx(bounds.X-INDENT,bounds.Y-INDENT,bounds.Width+2*INDENT,bounds.Height+2*INDENT);
				g.FillRectangle(Brush, bounds);
			}
		}

		public override RectangleFx ComputeFullBounds() 
        {
			RectangleFx result = UnionOfChildrenBounds;
			cachedChildBounds = result;
			result = new RectangleFx(result.X-INDENT,result.Y-INDENT,result.Width+2*INDENT,result.Height+2*INDENT);
			result = LocalToParent(result);
			return result;		
		}

		protected override bool ValidateFullBounds() 
        {
			comparisonBounds = UnionOfChildrenBounds;
	
			if (!cachedChildBounds.Equals(comparisonBounds)) 
            {
				PaintInvalid = true;
			}
			return base.ValidateFullBounds ();
		}
	}

	class VolatileDecoratorGroup : PNode 
    {
		readonly static int INDENT = 10;

		RectangleFx cachedChildBounds = RectangleFx.Empty;
		RectangleFx comparisonBounds = RectangleFx.Empty;
		PCamera renderCamera;

		public VolatileDecoratorGroup(PCamera camera) 
        {
			renderCamera = camera;
		}

		protected override bool BoundsVolatile 
        {
			get 
            {
				return true;
			}
		}

		public override RectangleFx Bounds 
        {
			get 
            {
				RectangleFx bds = base.Bounds;
				bds = UnionOfChildrenBounds;

				cachedChildBounds = bds;
				float scaledIndent = INDENT/renderCamera.ViewScale;
				bds = new RectangleFx(bds.X-scaledIndent,bds.Y-scaledIndent,bds.Width+2*scaledIndent,bds.Height+2*scaledIndent);
		
				return bds;
			}
		}

		protected override bool ValidateFullBounds() 
        {
			comparisonBounds = UnionOfChildrenBounds;
	
			if (!cachedChildBounds.Equals(comparisonBounds)) 
            {
				PaintInvalid = true;
			}
			return base.ValidateFullBounds ();
		}
	}
}