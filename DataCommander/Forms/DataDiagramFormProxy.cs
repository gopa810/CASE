using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GPFlowSequenceDiagram;
using CASE.Model;

namespace CASE.Forms
{
    public class DataDiagramFormProxy : DiagramElement
    {
        public DataDiagramFormProxy()
            : base(null)
        {
        }

        public DataDiagramFormProxy(DataDiagramForm diagramView)
            : base(null)
        {
            this.DiagramForm = diagramView;
        }

        public DataDiagramForm DiagramForm
        {
            get;
            set;
        }

        public override string DE_GetUniqueEntityName(string typeElement)
        {
            if (typeElement == "process" && DiagramForm != null && DiagramForm.project != null)
            {
                Project proj = DiagramForm.project;
                return proj.GetNewProcessName();
            }
            return base.DE_GetUniqueEntityName(typeElement);
        }

        public override void DE_DidSelectObject(object obj)
        {
            DiagramForm.PropertyGridView.SelectedObject = obj;
        }

    }
}
