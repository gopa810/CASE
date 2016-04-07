using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GPFlowSequenceDiagram
{
    public class DiagramElement
    {
        /// <summary>
        /// 0x000? - part of object
        /// 0x00?? - object
        /// 0x0??? - page (diagram), group of objects
        /// </summary>
        public const int ET_GENERAL_PART = 0x0100;
        public const int ET_GENERAL_OBJECT = 0x0200;
        public const int ET_GENERAL_PAGE = 0x0400;

        public const int ET_NONE = 0x0000;
        public const int ET_ORIGIN_POINT = 0x0101;
        public const int ET_ENDING_POINT = 0x0102;
        public const int ET_DATA_IN = 0x0105;
        public const int ET_DATA_OUT = 0x0106;
        public const int ET_DATA_INOUT = 0x0107;
        public const int ET_PRIMARY_AREA = 0x0111;
        public const int ET_SECONDARY_AREA = 0x0112;
        public const int ET_TERCIARY_AREA = 0x0113;


        [ReadOnly(true)]
        public DiagramElement Parent { get; set; }

        [ReadOnly(true)]
        public int ElementType { get; set; }

        [ReadOnly(true)]
        public int ElementId { get; set; }

        public Dictionary<Keys, Cursor> DefinedCursors = null;
        public RectangleF UsedRectangle = RectangleF.Empty;

        public DiagramElement(DiagramElement parent)
        {
            Parent = parent;
            ElementType = 0;
        }

        public virtual Cursor DE_GetCursor(Keys k)
        {
            if (DefinedCursors == null || DefinedCursors.ContainsKey(k) == false)
                return null;
            return DefinedCursors[k];
        }

        public void DE_SetCursor(Keys k, Cursor cr)
        {
            if (DefinedCursors == null)
                DefinedCursors = new Dictionary<Keys, Cursor>();
            DefinedCursors[k] = cr;
        }

        public virtual void DE_OnCollectionChanged()
        {
            if (Parent != null)
                Parent.DE_OnCollectionChanged();
        }

        public virtual int DE_GetUniqueId()
        {
            if (Parent != null)
                return Parent.DE_GetUniqueId();
            return 0;
        }

        public virtual void DE_RemoveConnectionWithItem(int itemId)
        {
            if (Parent != null)
                DE_RemoveConnectionWithItem(itemId);
        }

        public virtual Graphics DE_GetGraphics()
        {
            return (Parent != null ? Parent.DE_GetGraphics() : null);
        }

        public virtual void DE_OnItemSelected(DiagramItem selectedItem)
        {
            if (Parent != null)
                Parent.DE_OnItemSelected(selectedItem);
        }

        public virtual void DE_InsertNewItem(DiagramItem newItem, DiagramPoint absolutePos)
        {
            if (Parent != null)
                Parent.DE_InsertNewItem(newItem, absolutePos);
        }

        public virtual void DE_PopulateContextMenu(ContextMenuStrip cms, DiagramContext context)
        {
            if (Parent != null)
            {
                Parent.DE_PopulateContextMenu(cms, context);
            }
        }

        /// <summary>
        /// Retrieves new unique name for given entity type
        /// Usually this is implemented in the project.
        /// </summary>
        /// <param name="typeEntity">Entity type</param>
        /// <returns></returns>
        public virtual string DE_GetUniqueEntityName(string typeEntity)
        {
            if (Parent != null)
                return Parent.DE_GetUniqueEntityName(typeEntity);
            return typeEntity;
        }

        /// <summary>
        /// Informs parent that object was selected by user in user interface
        /// </summary>
        /// <param name="obj"></param>
        public virtual void DE_DidSelectObject(object obj)
        {
            if (Parent != null)
                Parent.DE_DidSelectObject(obj);
        }

        /// <summary>
        /// Find all elements that are placed at given diagram position.
        /// Diagram position is initialized in the context structure.
        /// </summary>
        /// <param name="context"></param>
        public virtual void DE_FindElements(DiagramContext context)
        {
        }

        public virtual RectangleF DE_GetRectangle()
        {
            return UsedRectangle;
        }

        public virtual SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            return SizeF.Empty;
        }

        public static RectangleF MergeRectangles(RectangleF result, RectangleF partial)
        {
            if (!partial.IsEmpty)
            {
                if (result.IsEmpty)
                {
                    return partial;
                }
                else
                {
                    float l = Math.Min(result.Left, partial.Left);
                    float t = Math.Min(result.Top, partial.Top);
                    float r = Math.Max(result.Right, partial.Right);
                    float b = Math.Max(result.Bottom, partial.Bottom);
                    return new RectangleF(l, t, r - l, b - t);
                }
            }

            return RectangleF.Empty;
        }

        public virtual DiagramElement DE_FindPredecessorOfType(int nType)
        {
            DiagramElement elem = this.Parent;
            while (elem != null)
            {
                if ((elem.ElementType & nType) != 0)
                    return elem;
                elem = elem.Parent;
            }

            return null;
        }
    }
}
