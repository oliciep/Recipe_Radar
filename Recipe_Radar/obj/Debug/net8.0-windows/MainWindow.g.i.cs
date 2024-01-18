﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "059A115E796AE9A7149F60B1634E3C2815CECC4D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Recipe_Radar;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
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


namespace RecipeRadar {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 102 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ButtonsPanel;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel AccountPanel;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ingredientsTextBox;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ingredientListBox;
        
        #line default
        #line hidden
        
        
        #line 140 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CuisinesComboBox;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox timeTextBox;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox RecipesComboBox;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FindButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Recipe_Radar;V1.0.0.0;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ButtonsPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            
            #line 103 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.createRegisterWindow);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 104 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.createLoginWindow);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AccountPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            
            #line 107 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AccountButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 108 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.logOutButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ingredientsTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.ingredientListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 9:
            
            #line 135 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddIngredient_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 136 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveIngredient_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.CuisinesComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 140 "..\..\..\MainWindow.xaml"
            this.CuisinesComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CuisinesComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.timeTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 156 "..\..\..\MainWindow.xaml"
            this.timeTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.timeTextBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 13:
            this.RecipesComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 159 "..\..\..\MainWindow.xaml"
            this.RecipesComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.RecipesComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            this.FindButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

