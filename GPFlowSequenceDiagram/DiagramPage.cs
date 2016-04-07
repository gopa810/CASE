using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class DiagramPage: DiagramElement
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Browsable(false)]
        public DiagramItemCollection Items;

        [Browsable(false)]
        public DiagramDrawingContextTransform TransformMatrices { get; set; }

        public static DiagramDrawingContextTransform DefaultMatrices = null;

        /// <summary>
        /// for control flow items this point is identical with their OriginPoint
        /// for diagram pages this point is center or diagram
        /// </summary>
        public DiagramPoint LastOriginPos { get; set; }

        public string PageType;

        public DiagramPage(DiagramElement elem)
            : base(elem)
        {
            if (DefaultMatrices == null)
                DefaultMatrices = new DiagramDrawingContextTransform(0, 0, 1f);
            TransformMatrices = DefaultMatrices;
            ElementType = ET_GENERAL_PAGE;
            Items = new DiagramItemCollection(this);
            LastOriginPos = new DiagramPoint();
        }

        public override string ToString()
        {
            return string.Format("Page {0}", ElementId);
        }

        public override void DE_InsertNewItem(DiagramItem newItem, DiagramPoint location)
        {
            newItem.Parent = Items;
            newItem.OriginPoint.Point = location;

            Items.Add(newItem);
        }

        /// <summary>
        /// This implementation fo FindElements makes some changes to context
        /// so this should be called from parent object only if mouse pointer
        /// is really in the area of this page
        /// </summary>
        /// <param name="context"></param>
        public override void DE_FindElements(DiagramContext context)
        {
            // assumption is, that when FindElements of DiagramPage is called
            // then it should be added to found elements
            if (context.FoundElement != null)
                return;
            context.PagePoint = ClientToPagePoint(context.ClientLocation.X, context.ClientLocation.Y);
            Items.DE_FindElements(context);
    
            if (context.FoundElement == null)
                context.InsertElement(this);
        }

        public DiagramPoint ClientToPagePoint(float X, float Y)
        {
            if (TransformMatrices == null)
            {
                return new DiagramPoint(X, Y);
            }
            else
            {
                return new DiagramPoint(TransformMatrices.ClientToPagePoint(new PointF(X, Y)));
            }
        }

        public RectangleF PageToClientRect(RectangleF rect)
        {
            return TransformMatrices.PageToClientRect(rect);
        }

        public PointF PageToClientPoint(PointF pt)
        {
            return TransformMatrices.PageToClientPoint(pt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx">Drawing context</param>
        /// <param name="highlighType"></param>
        /// <param name="relativeRect">Rectangle in coordinates valid on page that is parent to this page</param>
        public void PG_DrawPageInRect(DiagramDrawingContext ctx, HighlightType highType, RectangleF relativeRect)
        {
            // convert relative rect to client coordinates
            RectangleF target = ctx.LastPageRectToClient(relativeRect);

            // create matrix
            ctx.SetTransformation(target, UsedRectangle);
            TransformMatrices = ctx.LastTransform;

            DE_DrawShape(ctx, highType);
            
            ctx.PopTransform();
        }

    }
}
