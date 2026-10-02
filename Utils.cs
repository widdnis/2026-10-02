using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Microsoft.VisualBasic;

namespace Modless
{
    public class Utils
    {
        /// <summary>
        /// XYZ 리스트를 CurveLoop로 반환
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CurveLoop GetCurveLoop(List<XYZ> points)
        {
            CurveLoop cl = new CurveLoop();
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                {
                    Line line = Line.CreateBound(points[i], points[i + 1]);
                    cl.Append(line);
                }
                else if (i == points.Count - 1)
                {
                    Line line = Line.CreateBound(points[i], points[0]);
                    cl.Append(line);
                }
            }
            return cl;
        }

        /// <summary>
        /// FloorType 찾기
        /// </summary>
        /// <param name="name"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static FloorType FindFloorType(string name, Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            col.OfCategory(BuiltInCategory.OST_Floors);
            col.OfClass(typeof(FloorType));

            FloorType ft = null;
            foreach (FloorType item in col)
            {
                if (name == item.Name)
                {
                    ft = item;
                    break;
                }
            }
            return ft;
        }
        /// <summary>
        /// WallType 찾기
        /// </summary>
        /// <param name="name"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static WallType FindWallType(string name, Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            col.OfCategory(BuiltInCategory.OST_Walls);
            col.OfClass(typeof(WallType));

            WallType wt = null;
            foreach (WallType item in col)
            {
                if (name == item.Name)
                {
                    wt = item;
                    break;
                }
            }
            return wt;
        }
        /// <summary>
        /// CeilingType 찾기
        /// </summary>
        /// <param name="name"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static CeilingType FindCeilingType(string name, Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            col.OfCategory(BuiltInCategory.OST_Ceilings);
            col.OfClass(typeof(CeilingType));

            CeilingType ct = null;
            foreach (CeilingType item in col)
            {
                if (name == item.Name)
                {
                    ct = item;
                    break;
                }
            }
            return ct;
        }
        /// <summary>
        /// Level 찾기
        /// </summary>
        /// <param name="name"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Level FindLevel(string name, Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            col.OfClass(typeof(Level));

            Level lv = null;
            foreach (Level item in col)
            {
                if (name == item.Name)
                {
                    lv = item;
                    break;
                }
            }
            return lv;
        }
    }
}
