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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using XnaPiccolo;
using XnaPiccolo.Event;
using XnaPiccolo.Nodes;
using XnaPiccolo.Util;
using XnaPiccoloX.Handles;
using XnaPiccoloX.Util;
using Xna2dEditor;

namespace XnaPiccoloFeatures {
	public class AngleNodeExample : XnaPiccoloX.PForm {
		private System.ComponentModel.IContainer components = null;

		public AngleNodeExample() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() {
			Canvas.Layer.AddChild(new AngleNode());
		}

		public class AngleNode : PPath {
			protected PointFx pointOne = PointFx.Empty;
			protected PointFx pointTwo = PointFx.Empty;

			public AngleNode() {
				PointOne = new PointFx(100, 10);
				PointTwo = new PointFx(10, 100);
				AddHandles();
			}

			public PointFx PointOne {
				set {
					pointOne = value;
					UpdatePath();
				}
				get { return pointOne; }
			}

			public PointFx PointTwo {
				set {
					pointTwo = value;
					UpdatePath();
				}
				get { return pointTwo; }
			}

			public void AddHandles() {
				PHandle h1 = new PHandle(new AngleHandleLocator(this, AngleHandleLocator.HandleNum.HandleOne));
				h1.HandleDrag = new HandleDragDelegate(DragHandleOneHandler);
				AddChild(h1);

				PHandle h2 = new PHandle(new AngleHandleLocator(this, AngleHandleLocator.HandleNum.HandleTwo));
				h2.HandleDrag = new HandleDragDelegate(DragHandleTwoHandler);
				AddChild(h2);
			}

			public void DragHandleOneHandler(object sender, SizeFx localDelta, PInputEventArgs e) {
				SizeFx parentDelta = LocalToParent(localDelta);
				PointOne = new PointFx(PointOne.X + parentDelta.Width, PointOne.Y + parentDelta.Height);
				((PHandle)sender).RelocateHandle();
			}

			public void DragHandleTwoHandler(object sender, SizeFx localDelta, PInputEventArgs e) {
				SizeFx parentDelta = LocalToParent(localDelta);
				PointTwo = new PointFx(PointTwo.X + parentDelta.Width, PointTwo.Y + parentDelta.Height);
				((PHandle)sender).RelocateHandle();
			}

			class AngleHandleLocator : PLocator {
				public enum HandleNum {HandleOne, HandleTwo};
				AngleNode target;
				HandleNum handleNum;

				public AngleHandleLocator(AngleNode target, HandleNum handleNum) {
					this.target = target;
					this.handleNum = handleNum;
				}
				public override float LocateX {
					get {
						if (handleNum == HandleNum.HandleOne) {
							return target.pointOne.X;
						} else {
							return target.pointTwo.X;
						}
					}
				}
				public override float LocateY {
					get {
						if (handleNum == HandleNum.HandleOne) {
							return target.pointOne.Y;
						} else {
							return target.pointTwo.Y;
						}
					}
				}
			}

			protected void UpdatePath() {
				Reset();
				AddLine(pointOne.X, pointOne.Y, 0, 0);
				AddLine(0, 0, pointTwo.X, pointTwo.Y);
			}
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
			// AngleNodeExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "AngleNodeExample";
			this.Text = "AngleNodeExample";

		}
		#endregion
	}
}