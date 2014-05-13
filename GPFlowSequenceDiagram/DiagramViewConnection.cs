using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class DiagramViewConnection
    {
        public DiagramConnectionCollection Collection = null;
 
        private int p_id = 0;
        private int p_source_id = 0;
        private int p_destination_id = 0;

        public PointF SourcePoint;
        public PointF DestinationPoint;
        public bool PointsValid = false;

        public int Id
        {
            get { return p_id; }
            set { p_id = value; }
        }
        public int SourceId
        {
            get { return p_source_id; }
            set { p_source_id = value; }
        }
        public int DestinationId
        {
            get { return p_destination_id; }
            set { p_destination_id = value; }
        }

    }
}
