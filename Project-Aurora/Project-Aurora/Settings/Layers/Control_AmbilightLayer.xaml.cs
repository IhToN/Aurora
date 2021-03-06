﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Aurora.Settings.Layers
{
    /// <summary>
    /// Interaction logic for Control_AmbilightLayer.xaml
    /// </summary>
    public partial class Control_AmbilightLayer : UserControl
    {
        private bool settingsset = false;

        public Control_AmbilightLayer()
        {
            InitializeComponent();
        }

        public Control_AmbilightLayer(AmbilightLayerHandler datacontext)
        {
            InitializeComponent();
            this.DataContext = datacontext;
        }

        public void SetSettings()
        {
            if (this.DataContext is AmbilightLayerHandler && !settingsset)
            {
                var properties = (this.DataContext as AmbilightLayerHandler).Properties;
                this.XCoordinate.Value = properties._Coordinates.Value.Left;
                this.YCoordinate.Value = properties._Coordinates.Value.Top;
                this.HeightCoordinate.Value = properties._Coordinates.Value.Height;
                this.WidthCoordinate.Value = properties._Coordinates.Value.Width;
                settingsset = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetSettings();
            this.Loaded -= UserControl_Loaded;
        }

        private void Coordinate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingsset)
                return;

            (DataContext as AmbilightLayerHandler).Properties._Coordinates = new System.Drawing.Rectangle(
                XCoordinate.Value ?? 0, 
                YCoordinate.Value ?? 0,
                WidthCoordinate.Value ?? 0,
                HeightCoordinate.Value ?? 0
            );
        }
    }

    public class AmbilightCaptureTypeValueConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is AmbilightCaptureType v && Enum.TryParse(parameter.ToString(), out AmbilightCaptureType r) && v == r;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
