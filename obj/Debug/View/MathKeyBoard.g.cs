﻿#pragma checksum "..\..\..\View\MathKeyBoard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "050274D23FA92EAC0D2FC01B93B886DD"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using GalaSoft.MvvmLight.Command;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MathSpace {
    
    
    /// <summary>
    /// MathKeyBoard
    /// </summary>
    public partial class MathKeyBoard : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\View\MathKeyBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem symbolTabItem;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\View\MathKeyBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem operatorTabItem;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\View\MathKeyBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem numberTabItem;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\View\MathKeyBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem letterTabItem;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MathSpace;component/view/mathkeyboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\MathKeyBoard.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\View\MathKeyBoard.xaml"
            ((MathSpace.MathKeyBoard)(target)).Loaded += new System.Windows.RoutedEventHandler(this.MathKeyBoard_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.symbolTabItem = ((System.Windows.Controls.TabItem)(target));
            return;
            case 3:
            this.operatorTabItem = ((System.Windows.Controls.TabItem)(target));
            return;
            case 4:
            this.numberTabItem = ((System.Windows.Controls.TabItem)(target));
            return;
            case 5:
            this.letterTabItem = ((System.Windows.Controls.TabItem)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

