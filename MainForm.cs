using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
// WinForms 와 Revit API 에 같은 이름이 있는 클래스는 어느 쪽을 쓸지 지정합니다.
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Modless
{
    // ═══════════════════════════════════════════════════════════════
    //  ExternalEvent 란?
    // ═══════════════════════════════════════════════════════════════
    //
    //  ■ 왜 필요한가?
    //    Revit 은 "지금은 애드인 차례" 라고 허락한 동안에만 Revit API 를 쓸 수 있게 합니다.
    //    (이 "애드인 차례" 를 API 컨텍스트 라고 부릅니다.)
    //    명령(Command)의 Execute() 가 실행되는 동안이 바로 "애드인 차례" 입니다.
    //
    //    [모달 폼 - ShowDialog()]
    //      명령 시작 → 폼 열림 → 버튼 클릭 → 폼 닫힘 → 명령 끝
    //      버튼을 누를 때 명령이 아직 안 끝났음 → "애드인 차례" → API 사용 O
    //      (대신 폼이 열려 있는 동안 Revit 을 조작할 수 없습니다)
    //
    //    [모드리스 폼 - Show()]
    //      명령 시작 → 폼 열림 → 명령 끝 (폼은 계속 떠 있음) → 버튼 클릭
    //      버튼을 누를 때 명령이 이미 끝났음 → "애드인 차례" 아님 → API 사용 X
    //      (억지로 사용하면 "... outside of API context is not allowed." 예외 발생)
    //
    //    비유 : 은행 창구
    //      창구 직원(Revit)은 번호표를 뽑고 내 차례가 된 손님만 업무를 봐 줍니다.
    //      ExternalEvent 가 바로 이 "번호표" 입니다.
    //
    //  ■ 해결 방법 : ExternalEvent (번호표)
    //    버튼을 누르면 번호표를 뽑아 두고, 내 차례가 되면 Revit 이 할 일을 실행해 줍니다.
    //
    //    1) ExternalEvent.Create(this)    → 번호표 기계 설치 (폼을 만들 때 한 번)
    //    2) exEvent.Raise()               → 번호표 뽑기 (버튼 클릭 시)
    //    3) Execute(UIApplication app)    → 내 차례! Revit 이 호출해 줌 (API 사용 O)
    //    4) exEvent.Dispose()             → 번호표 기계 철거 (폼을 닫을 때)
    //
    //    ※ 번호표를 뽑는다고 바로 실행되지 않습니다. 차례가 올 때까지 기다립니다.
    //
    //  ■ 흐름
    //    [버튼 클릭] → 번호표 뽑기 → (차례 기다림) → 내 차례 → { } 안의 코드 실행
    //
    //  ■ 사용 방법
    //    버튼 안에 아래 모양을 그대로 쓰고, { } 안에 할 일을 작성하면 됩니다.
    //
    //        RunRevit((uidoc, doc) =>
    //        {
    //            // 할 일
    //        });
    //
    // ═══════════════════════════════════════════════════════════════
    public partial class MainForm : System.Windows.Forms.Form, IExternalEventHandler
    {
        // 번호표 기계
        private readonly ExternalEvent _exEvent;
        // 번호표에 적어 둔 할 일 (내 차례가 되면 실행됨)
        private Action<UIDocument, Document> _action;
        //
        public MainForm()
        {
            InitializeComponent();
            // 번호표 기계 설치
            // (폼은 Command.Execute() 안, 즉 "애드인 차례" 에 만들어지므로 여기서 설치할 수 있습니다)
            _exEvent = ExternalEvent.Create(this);
        }

        // ───────────── 버튼 ─────────────

        private void button1_Click(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                // 할 일
                TaskDialog.Show("Modless", "버튼 클릭! 내 차례!");
            });
        }


        // ───────────── 아래는 수정할 필요 없습니다 ─────────────

        /// <summary>
        /// 번호표를 뽑습니다. { } 안의 할 일은 내 차례가 되면 실행됩니다.
        /// </summary>
        private void RunRevit(Action<UIDocument, Document> action)
        {
            // 번호표에 할 일을 적고
            _action = action;
            // 번호표 뽑기 (바로 실행 X → 차례가 되면 Revit 이 Execute() 호출)
            // ※ 차례가 오기 전에 다시 누르면, 마지막에 누른 버튼의 할 일만 실행됩니다.
            _exEvent.Raise();
        }

        /// <summary>
        /// 내 차례! Revit 이 호출해 줍니다. 번호표에 적어 둔 할 일을 실행합니다.
        /// 이 안에서는 Revit API 를 자유롭게 사용할 수 있습니다.
        /// </summary>
        public void Execute(UIApplication app)
        {
            if (_action == null) return;

            UIDocument uidoc = app.ActiveUIDocument;
            if (uidoc == null)
            {
                TaskDialog.Show("Modless", "열려 있는 문서가 없습니다.");
                return;
            }

            try
            {
                _action(uidoc, uidoc.Document);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("오류", ex.Message);
            }
            finally
            {
                _action = null;
            }
        }

        /// <summary>
        /// 핸들러 이름. (IExternalEventHandler)
        /// Revit 이 내부적으로 이벤트를 구분할 때 사용합니다. 아무 이름이나 괜찮습니다.
        /// </summary>
        public string GetName()
        {
            return "Modless";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 번호표 기계 철거
            _exEvent.Dispose();
            base.OnFormClosed(e);
        }



        // 전역변수
        // 유형
        public static string _floorTypeSelect = "";
        public static string _wallTypeSelect = "";
        public static string _ceilingTypeSelect = "";
        // 높이
        public static string _wallHeightInput = "";
        public static string _ceilingHeightInput = "";
        // 레벨
        public static string _bottomLvStr = "";
        public static string _topLvStr = "";

        // Select Item 채우기
        private void MainForm_Load(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                FilteredElementCollector colFloor = new FilteredElementCollector(doc);
                colFloor.OfCategory(BuiltInCategory.OST_Floors);
                colFloor.OfClass(typeof(FloorType));

                foreach (FloorType item in colFloor)
                {
                    string name = item.Name;
                    comboBox1.Items.Add(name);
                }

                FilteredElementCollector colWall = new FilteredElementCollector(doc);
                colWall.OfCategory(BuiltInCategory.OST_Walls);
                colWall.OfClass(typeof(WallType));

                foreach (WallType item in colWall)
                {
                    string name = item.Name;
                    comboBox2.Items.Add(name);
                }

                FilteredElementCollector colCeiling = new FilteredElementCollector(doc);
                colCeiling.OfCategory(BuiltInCategory.OST_Ceilings);
                colCeiling.OfClass(typeof(CeilingType));

                foreach (CeilingType item in colCeiling)
                {
                    string name = item.Name;
                    comboBox3.Items.Add(name);
                }

                FilteredElementCollector colLevel = new FilteredElementCollector(doc);
                colLevel.OfClass(typeof(Level));

                foreach (Level item in colLevel)
                {
                    string name = item.Name;
                    comboBox7.Items.Add(name);
                    comboBox6.Items.Add(name);
                }
            });
        }

        // 1 - 바닥 생성
        private void button1_Click_1(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                XYZ ppp1 = new XYZ(6000, 0, 0) / 304.8;
                XYZ ppp2 = new XYZ(10000, 0, 0) / 304.8;
                XYZ ppp3 = new XYZ(10000, 4000, 0) / 304.8;
                XYZ ppp4 = new XYZ(6000, 4000, 0) / 304.8;

                List<XYZ> points2 = new List<XYZ>();
                points2.Add(ppp1);
                points2.Add(ppp2);
                points2.Add(ppp3);
                points2.Add(ppp4);

                IList<CurveLoop> cls1 = new List<CurveLoop>();

                CurveLoop lp2 = Utils.GetCurveLoop(points2);
                cls1.Add(lp2);

                FilteredElementCollector col = new FilteredElementCollector(doc);
                col.OfCategory(BuiltInCategory.OST_Floors);
                col.OfClass(typeof(FloorType));
                FloorType ft = col.FirstElement() as FloorType;

                ElementId floorid = ft.Id;

                Level level = doc.ActiveView.GenLevel;
                ElementId levelid = level.Id;

                using (Transaction trans = new Transaction(doc, "Generate Floor"))
                {
                    trans.Start();
                    Floor f = Floor.Create(doc, cls1, floorid, levelid);
                    trans.Commit();
                }
            });

        }

        // 1 - 벽 생성
        private void button2_Click(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                XYZ ppp1 = new XYZ(6000, 0, 0) / 304.8;
                XYZ ppp2 = new XYZ(10000, 0, 0) / 304.8;
                XYZ ppp3 = new XYZ(10000, 4000, 0) / 304.8;
                XYZ ppp4 = new XYZ(6000, 4000, 0) / 304.8;

                List<XYZ> points2 = new List<XYZ>();
                points2.Add(ppp1);
                points2.Add(ppp2);
                points2.Add(ppp3);
                points2.Add(ppp4);

                IList<CurveLoop> cls1 = new List<CurveLoop>();

                CurveLoop lp2 = Utils.GetCurveLoop(points2);
                cls1.Add(lp2);

                FilteredElementCollector col = new FilteredElementCollector(doc);
                col.OfCategory(BuiltInCategory.OST_Walls);
                col.OfClass(typeof(WallType));
                WallType wt = col.FirstElement() as WallType;

                ElementId wallid = wt.Id;

                Level level = doc.ActiveView.GenLevel;
                ElementId levelid = level.Id;

                double wallheight = 3000;

                using (Transaction trans = new Transaction(doc, "CreateWall"))
                {
                    trans.Start();
                    foreach (CurveLoop loop in cls1)
                    {
                        foreach (Curve c in loop)
                        {
                            Wall wall = Wall.Create(doc, c, wt.Id, level.Id, wallheight / 304.8, 0, false, true);

                        }
                    }
                    trans.Commit();
                }
            });
        }

        // 1 - 천장 생성
        private void button3_Click_(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                XYZ ppp1 = new XYZ(6000, 0, 0) / 304.8;
                XYZ ppp2 = new XYZ(10000, 0, 0) / 304.8;
                XYZ ppp3 = new XYZ(10000, 4000, 0) / 304.8;
                XYZ ppp4 = new XYZ(6000, 4000, 0) / 304.8;

                List<XYZ> points2 = new List<XYZ>();
                points2.Add(ppp1);
                points2.Add(ppp2);
                points2.Add(ppp3);
                points2.Add(ppp4);

                IList<CurveLoop> cls1 = new List<CurveLoop>();

                CurveLoop lp2 = Utils.GetCurveLoop(points2);
                cls1.Add(lp2);

                FilteredElementCollector col = new FilteredElementCollector(doc);
                col.OfCategory(BuiltInCategory.OST_Ceilings);
                col.OfClass(typeof(CeilingType));
                CeilingType ct = col.FirstElement() as CeilingType;

                ElementId ceilingid = ct.Id;

                Level level = doc.ActiveView.GenLevel;
                ElementId levelid = level.Id;

                double ceilingheight = 3000;

                using (Transaction trans = new Transaction(doc, "Generate Ceiling"))
                {
                    trans.Start();
                    Ceiling c = Ceiling.Create(doc, cls1, ceilingid, levelid);
                    Parameter param = c.get_Parameter(BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM);
                    param.Set(ceilingheight / 304.8);

                    trans.Commit();
                }
            });

        }


        // 선택 유형을 string으로 반환
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _floorTypeSelect = comboBox1.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _wallTypeSelect = comboBox2.SelectedItem.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ceilingTypeSelect = comboBox3.SelectedItem.ToString();
        }

        // 입력 높이를 저장
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _wallHeightInput = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            _ceilingHeightInput = textBox2.Text;
        }

        // 선택 레벨을 string으로 반환
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            _bottomLvStr = comboBox7.SelectedItem.ToString();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            _topLvStr = comboBox6.SelectedItem.ToString();
        }

        // 2 - 룸 마감 - 룸 선택
        public class RoomFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem is Room;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }

        // 2 - 룸 마감 - 바닥 생성
        private void button8_Click(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                IList<Reference> refs = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new RoomFilter(), "룸을 선택하세요.");

                foreach (Reference item in refs)
                {
                    Room r = (Room)doc.GetElement(item);

                    // 룸의 SL과 3D View 찾기
                    FilteredElementCollector col3D = new FilteredElementCollector(doc);
                    col3D.OfClass(typeof(View3D));

                    View3D v3d = null;
                    foreach (View3D v in col3D)
                    {
                        if (v.IsTemplate == false)
                        {
                            v3d = v;
                            break;
                        }
                    }
                    if (v3d == null)
                    {
                        TaskDialog.Show("경고", "3D뷰를 찾을 수 없습니다.");
                        return;
                    }

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                    ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.Element, v3d);
                    LocationPoint lp = r.Location as LocationPoint;
                    XYZ sp = new XYZ(lp.Point.X, lp.Point.Y, lp.Point.Z + 1200 / 304.8);
                    ReferenceWithContext rwc = ri.FindNearest(sp, -XYZ.BasisZ);

                    double hitZ = 0;
                    if (rwc != null)
                    {
                        Reference r1 = rwc.GetReference();
                        XYZ hit = r1.GlobalPoint;
                        hitZ = hit.Z;
                    }

                    SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
                    opt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                    IList<IList<BoundarySegment>> loops = r.GetBoundarySegments(opt);

                    List<CurveLoop> cls = new List<CurveLoop>();
                    foreach (IList<BoundarySegment> loop in loops)
                    {
                        CurveLoop cl = new CurveLoop();
                        foreach (BoundarySegment bs in loop)
                        {
                            Curve c = bs.GetCurve();
                            cl.Append(c);
                        }
                        cls.Add(cl);
                    }

                    Level level = Utils.FindLevel(_bottomLvStr, doc);
                    if (level == null)
                    {
                        TaskDialog.Show("경고", "레벨을 선택해 주세요.");
                        return;
                    }

                    double floorTHK = 0;
                    {
                        FloorType ft = Utils.FindFloorType(_floorTypeSelect, doc);
                        if (ft == null)
                        {
                            TaskDialog.Show("경고", "유형을 찾을 수 없습니다.");
                            return;
                        }
                        Parameter param = ft.get_Parameter(BuiltInParameter.FLOOR_ATTR_DEFAULT_THICKNESS_PARAM);
                        if (param == null)
                        {
                            TaskDialog.Show("경고", "두께를 찾을 수 없습니다.");
                            return;
                        }

                        floorTHK = param.AsDouble();

                        using (Transaction trans = new Transaction(doc, "바닥 마감을 생성합니다."))
                        {
                            trans.Start();
                            Floor f = Floor.Create(doc, cls, ft.Id, level.Id);
                            Parameter upParam = f.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM);
                            if (upParam != null)
                            {
                                upParam.Set(floorTHK);
                            }
                            trans.Commit();
                        }

                    }
                }
            });
        }

        // 2 - 룸 마감 - 벽 생성
        private void button7_Click(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                IList<Reference> refs = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new RoomFilter(), "룸을 선택하세요.");

                foreach (Reference item in refs)
                {
                    Room r = (Room)doc.GetElement(item);

                    // 룸의 SL과 3D View 찾기
                    FilteredElementCollector col3D = new FilteredElementCollector(doc);
                    col3D.OfClass(typeof(View3D));

                    View3D v3d = null;
                    foreach (View3D v in col3D)
                    {
                        if (v.IsTemplate == false)
                        {
                            v3d = v;
                            break;
                        }
                    }
                    if (v3d == null)
                    {
                        TaskDialog.Show("경고", "3D뷰를 찾을 수 없습니다.");
                        return;
                    }

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                    ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.Element, v3d);
                    LocationPoint lp = r.Location as LocationPoint;
                    XYZ sp = new XYZ(lp.Point.X, lp.Point.Y, lp.Point.Z + 1200 / 304.8);
                    ReferenceWithContext rwc = ri.FindNearest(sp, -XYZ.BasisZ);

                    double hitZ = 0;
                    if (rwc != null)
                    {
                        Reference r1 = rwc.GetReference();
                        XYZ hit = r1.GlobalPoint;
                        hitZ = hit.Z;
                    }

                    SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
                    opt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                    IList<IList<BoundarySegment>> loops = r.GetBoundarySegments(opt);

                    List<CurveLoop> cls = new List<CurveLoop>();
                    foreach (IList<BoundarySegment> loop in loops)
                    {
                        CurveLoop cl = new CurveLoop();
                        foreach (BoundarySegment bs in loop)
                        {
                            Curve c = bs.GetCurve();
                            cl.Append(c);
                        }
                        cls.Add(cl);
                    }

                    Level level = Utils.FindLevel(_bottomLvStr, doc);
                    if (level == null)
                    {
                        TaskDialog.Show("경고", "레벨을 선택해 주세요.");
                        return;
                    }

                    double floorTHK = 0;
                    double wallTHK = 0;
                    {
                        WallType wt = Utils.FindWallType(_wallTypeSelect, doc);
                        if (wt == null)
                        {
                            TaskDialog.Show("경고", "유형을 찾을 수 없습니다.");
                            return;
                        }

                        double wallH = Convert.ToDouble(_wallHeightInput);
                        Parameter param = wt.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM);
                        wallTHK = param.AsDouble();

                        foreach (CurveLoop cl in cls)
                        {
                            CurveLoop offLoop = CurveLoop.CreateViaOffset(cl, -wallTHK / 2, XYZ.BasisZ);

                            foreach (Curve c in offLoop)
                            {
                                using (Transaction trans = new Transaction(doc, "벽 마감을 생성합니다."))
                                {
                                    trans.Start();
                                    Wall wall = Wall.Create(doc, c, wt.Id, level.Id, wallH / 384.8, floorTHK, false, false);
                                    Parameter upParam = wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE);
                                    Level topLevel = Utils.FindLevel(_topLvStr, doc);
                                    if (upParam != null && topLevel != null)
                                        upParam.Set(topLevel.Id);
                                    trans.Commit();
                                }
                            }
                        }
                    }
                }
            });
        }

        // 2 - 룸 마감 - 천장 생성
        private void button6_Click(object sender, EventArgs e)
        {
            RunRevit((uidoc, doc) =>
            {
                IList<Reference> refs = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new RoomFilter(), "룸을 선택하세요.");

                foreach (Reference item in refs)
                {
                    Room r = (Room)doc.GetElement(item);

                    // 룸의 SL과 3D View 찾기
                    FilteredElementCollector col3D = new FilteredElementCollector(doc);
                    col3D.OfClass(typeof(View3D));

                    View3D v3d = null;
                    foreach (View3D v in col3D)
                    {
                        if (v.IsTemplate == false)
                        {
                            v3d = v;
                            break;
                        }
                    }
                    if (v3d == null)
                    {
                        TaskDialog.Show("경고", "3D뷰를 찾을 수 없습니다.");
                        return;
                    }

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
                    ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.Element, v3d);
                    LocationPoint lp = r.Location as LocationPoint;
                    XYZ sp = new XYZ(lp.Point.X, lp.Point.Y, lp.Point.Z + 1200 / 304.8);
                    ReferenceWithContext rwc = ri.FindNearest(sp, -XYZ.BasisZ);

                    double hitZ = 0;
                    if (rwc != null)
                    {
                        Reference r1 = rwc.GetReference();
                        XYZ hit = r1.GlobalPoint;
                        hitZ = hit.Z;
                    }

                    SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
                    opt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                    IList<IList<BoundarySegment>> loops = r.GetBoundarySegments(opt);

                    List<CurveLoop> cls = new List<CurveLoop>();
                    foreach (IList<BoundarySegment> loop in loops)
                    {
                        CurveLoop cl = new CurveLoop();
                        foreach (BoundarySegment bs in loop)
                        {
                            Curve c = bs.GetCurve();
                            cl.Append(c);
                        }
                        cls.Add(cl);
                    }

                    Level level = Utils.FindLevel(_bottomLvStr, doc);
                    if (level == null)
                    {
                        TaskDialog.Show("경고", "레벨을 선택해 주세요.");
                        return;
                    }

                    double floorTHK = 0;
                    double wallTHK = 0;
                    {
                        CeilingType ct = Utils.FindCeilingType(_ceilingTypeSelect, doc);
                        double ceilingH = Convert.ToDouble(_ceilingHeightInput);

                        List<CurveLoop> offLoop = new List<CurveLoop>();
                        foreach (CurveLoop curveloop in cls)
                        {
                            if (wallTHK > 0)
                            {
                                CurveLoop cl = CurveLoop.CreateViaOffset(curveloop, -wallTHK, XYZ.BasisZ);
                                offLoop.Add(curveloop);
                            }
                            else
                            {
                                offLoop.Add(curveloop);
                            }
                        }


                        if (ct == null)
                        {
                            TaskDialog.Show("경고", "유형을 찾을 수 없습니다.");
                            return;
                        }

                        using (Transaction trans = new Transaction(doc, "천장 마감을 생성합니다."))
                        {
                            trans.Start();

                            Ceiling c = Ceiling.Create(doc, offLoop, ct.Id, level.Id);
                            Parameter param = c.get_Parameter(BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM);
                            param.Set(ceilingH / 304.8 + floorTHK + hitZ);

                            trans.Commit();
                        }
                    }
                }
            });
        }
    }
}
