using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fyri2dEditor.Xna2dDrawingLibrary
{
    public enum CommandType
    {
        DrawLine,
        DrawTriangle,
        DrawRectangle,
        DrawEllipse,
        DrawPolyline,
        
        DrawFilledRectangle,
        DrawFilledTriangle,
        DrawFilledEllipse,
        
        SetPixel,
        GetPixel,

        DrawPoints,
        DrawColors,

        DrawText,
        DrawTexture,
    }
}
