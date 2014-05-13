﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class DrawProperties
    {
        public static Pen p_penNormal = new Pen(Color.Black, 1f);
        public static Pen p_penBold = new Pen(Color.Black, 2f);
        public static Pen p_penHighlight = new Pen(Color.Green, 2f);

        public static float p_drawingStep = 16;

        public static Font fontSmallTitles = SystemFonts.SmallCaptionFont;
    }
}
