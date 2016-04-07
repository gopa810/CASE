using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace GPFlowSequenceDiagram
{
    /// <summary>
    /// Context in the diagram page
    /// 
    /// When user click with mouse button on some position in the view
    /// program needs to determine what is "under" the cursor, so
    /// everything what is found, is placed in this object.
    /// </summary>
    public class DiagramContext
    {
        /// <summary>
        /// Coordinates in View (graphical)
        /// </summary>
        public Point ClientLocation = new Point(0, 0);

        /// <summary>
        /// Coordinates in Screen (graphical)
        /// </summary>
        public Point ScreenLocation = new Point(0, 0);

        /// <summary>
        /// This type of diagram element will be searched for
        /// value is ET_...
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// Elements found "under" cursor
        /// </summary>
        public DiagramElement FoundElement { get; set; }

        /// <summary>
        /// Page where coordinates are located
        /// </summary>
        public DiagramPage Page { get; set; }

        /// <summary>
        /// Coordinates in logical coordinate system
        /// (these coordinates are used in diagram to place objects)
        /// </summary>
        public DiagramPoint PagePoint = new DiagramPoint(0, 0);


        public void InsertElement(DiagramElement diagramELement)
        {
            if (SearchType == 0 || (SearchType & diagramELement.ElementType) != 0x0000)
            {
                FoundElement = diagramELement;
            }
        }

        public void InitSearchOfCounterpartFor(int elementType)
        {
            if (elementType == 0)
                SearchType = 0;
            if (elementType == DiagramElement.ET_ORIGIN_POINT)
                SearchType = DiagramElement.ET_ENDING_POINT;
            else if (elementType == DiagramElement.ET_ENDING_POINT)
                SearchType = DiagramElement.ET_ORIGIN_POINT;
        }
    }
}
