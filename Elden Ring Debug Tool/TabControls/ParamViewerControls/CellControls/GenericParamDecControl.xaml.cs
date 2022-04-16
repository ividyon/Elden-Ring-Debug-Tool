﻿using PropertyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Elden_Ring_Debug_Tool
{
    //https://stackoverflow.com/questions/3811179/wpf-usercontrol-with-generic-code-behind lol
    public abstract partial class GenericParamDecControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        // If you use event handlers in GenericUserControl.xaml, you have to define 
        // them here as abstract and implement them in the generic class below, e.g.:

        // abstract protected void MouseClick(object sender, MouseButtonEventArgs e);
    }

    /// <summary>
    /// Interaction logic for ParamControl.xaml
    /// </summary>
    public partial class ParamDecControl<T> : GenericParamDecControl, ICellControl
    {
        public ERParam Param { get; private set; }
        private int FieldOffset { get; set; }
        public int Offset => Param.SelectedRow?.DataOffset + FieldOffset ?? 0;
        public string FieldName { get; set; }
        public string Value { get => ParamValue.ToString(); }
        public float ParamValue
        {
            get => BitConverter.ToSingle(Param.Bytes, Offset);
            set
            {
                Param.Pointer.WriteSingle(Offset, value);
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0,Param.Bytes, Offset,  bytes.Length);
            }
        }

        public ParamDecControl(ERParam param, int fieldOffset, string name)
        {
            Param = param;
            FieldOffset = fieldOffset;
            FieldName = name;

            InitializeComponent();
        }

        public void UpdateField()
        {
            OnPropertyChanged(nameof(ParamValue));
        }
    }
}
