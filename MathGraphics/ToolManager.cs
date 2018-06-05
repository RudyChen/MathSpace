using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGraphics
{

    public enum ToolControlType
    {
        ToggleButton,
        Label,
        Line,
        Polyline,
        Text,
        Other
    }

    public enum ToolType
    {
        /// <summary>
        /// 图形选择
        /// </summary>
        ItemSelect,
        /// <summary>
        /// 移动画布
        /// </summary>
        CanvasMove,
        /// <summary>
        /// 文本工具
        /// </summary>
        Text,
        /// <summary>
        /// 直线
        /// </summary>
        StraightLine,
        /// <summary>
        /// L型线
        /// </summary>
        LShapeLine,
        /// <summary>
        /// 开关
        /// </summary>
        Switch,
        /// <summary>
        /// 接地设备
        /// </summary>
        GroundSwitch,
        /// <summary>
        /// 验电点
        /// </summary>
        ElectricConfirm,
        /// <summary>
        /// 轨道形状
        /// </summary>
        TrackLine,
        /// <summary>
        /// 轨道名称
        /// </summary>
        TrackID,
        /// <summary>
        /// 电路断开标识
        /// </summary>
        LineBreaker,
        /// <summary>
        /// 平台门
        /// </summary>
        PlatDoor,
        /// <summary>
        /// 断路器
        /// </summary>
        CircuitBreaker,
        /// <summary>
        /// 母线
        /// </summary>
        Bus,
        /// <summary>
        /// 斜线
        /// </summary>
        ObliqueLine,
        /// <summary>
        /// 跌落熔丝
        /// </summary>
        DropFuse,
        /// <summary>
        /// 地线灯
        /// </summary>
        GroundLeadLight,
        /// <summary>
        /// 接地线
        /// </summary>
        GroundLead,
        /// <summary>
        /// 网门
        /// </summary>
        NetDoor,
        /// <summary>
        /// 双卷变
        /// </summary>
        TwoPhaseTransformer,
        /// <summary>
        /// 三卷变
        /// </summary>
        ThreePhaseTransformer,
        /// <summary>
        /// 箭头
        /// </summary>
        Arrow,
        /// <summary>
        /// 空心箭头
        /// </summary>
        ArrowEmpty,
        /// <summary>
        /// 静态接地
        /// </summary>
        GroundLeadStatic,
        /// <summary>
        /// 避雷接地
        /// </summary>
        LightningGroundLead,
        /// <summary>
        /// 避雷接地2
        /// </summary>
        LightningGroundLead2,
        /// <summary>
        /// 电缆双头
        /// </summary>
        ElectricCable,
    }

    public class ToolItem
    {
        private ToolType toolType;

        private bool isSelected;

        private bool isDrawShape;

        private ToolControlType controlType;

        public ToolControlType ControlType
        {
            get { return controlType; }
            set { controlType = value; }
        }


        public bool IsDrawShape
        {
            get { return isDrawShape; }
            set { isDrawShape = value; }
        }


        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public ToolType ToolType
        {
            get { return toolType; }
            set { toolType = value; }
        }
    }

    public class ToolManager
    {
       
        private static ToolManager instance;
        private static object _lock = new object();

        private List<ToolItem> tools=new List<ToolItem>();

        public List<ToolItem> Tools
        {
            get { return tools; }
            set { tools = value; }
        }


        private ToolManager()
        {
            Tools.Add(new ToolItem() { ToolType = ToolType.ItemSelect, IsSelected = false , IsDrawShape =false, ControlType=ToolControlType.Other });
            Tools.Add(new ToolItem() { ToolType = ToolType.CanvasMove, IsSelected = false,IsDrawShape=false, ControlType = ToolControlType.Other });
            Tools.Add(new ToolItem() { ToolType = ToolType.Text, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Text });
            Tools.Add(new ToolItem() { ToolType = ToolType.StraightLine, IsSelected = false ,IsDrawShape = true, ControlType = ToolControlType.Line });
            Tools.Add(new ToolItem() { ToolType = ToolType.LShapeLine, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Polyline });
            Tools.Add(new ToolItem() { ToolType = ToolType.Switch, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.GroundSwitch, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.ElectricConfirm, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.TrackLine, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.TrackID, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.LineBreaker, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.PlatDoor, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.CircuitBreaker, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.Bus, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Line });
            Tools.Add(new ToolItem() { ToolType = ToolType.ObliqueLine, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Line });
            Tools.Add(new ToolItem() { ToolType = ToolType.DropFuse, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.GroundLeadLight, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.GroundLead, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.NetDoor, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.ToggleButton });
            Tools.Add(new ToolItem() { ToolType = ToolType.TwoPhaseTransformer, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.ThreePhaseTransformer, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.Arrow, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.ArrowEmpty, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.GroundLeadStatic, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.LightningGroundLead, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.LightningGroundLead2, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
            Tools.Add(new ToolItem() { ToolType = ToolType.ElectricCable, IsSelected = false, IsDrawShape = true, ControlType = ToolControlType.Label });
        }

        public static ToolManager GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new ToolManager();
                    }
                }
            }
            return instance;
        }

        public void SetSelectToolItem(ToolType toolType)
        {
            foreach (var item in tools)
            {
                if (item.ToolType== toolType)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

    }
}
