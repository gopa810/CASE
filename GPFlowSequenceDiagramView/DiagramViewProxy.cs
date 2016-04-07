using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using GPFlowSequenceDiagram;

namespace GPFlowSequenceDiagramView
{
    public class DiagramViewProxy : DiagramElement
    {
        public DiagramDrawingContext Context { get; set; }

        public DiagramViewProxy(DiagramElement parent)
            : base(parent)
        {
            Context = new DiagramDrawingContext();
        }

        public DiagramViewProxy(DiagramElement parent, View diagramView)
            : base(parent)
        {
            this.DiagramView = diagramView;
        }

        public override string ToString()
        {
            return string.Format("ViewProxy {0}", ElementId);
        }

        public View DiagramView
        {
            get;
            set;
        }

        public override void DE_OnCollectionChanged()
        {
            DiagramView.RedrawClientScreen();

            base.DE_OnCollectionChanged();
        }

        public override void DE_OnItemSelected(DiagramItem selectedItem)
        {
            DiagramView.RedrawClientScreen();

            base.DE_OnItemSelected(selectedItem);
        }
    }

}
