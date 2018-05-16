﻿using EarTrumpet.Services;
using EarTrumpet.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace EarTrumpet.UserControls
{
    public partial class VolumeControlPopup : Popup
    {
        public VolumeControlPopup()
        {
            InitializeComponent();

            AllowsTransparency = true;
        }

        private void ShowWithAnimation()
        {
            var fadeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                FillBehavior = FillBehavior.Stop,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut },
                From = 0.25,
                To = 1,
            };

            Child.BeginAnimation(OpacityProperty, fadeAnimation);

            IsOpen = true;
        }

        public void HideWithAnimation()
        {
            var fadeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                FillBehavior = FillBehavior.Stop,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut },
                From = 1,
                To = 0.25,
            };

            fadeAnimation.Completed += (_, __) => IsOpen = false;

            Child.BeginAnimation(OpacityProperty, fadeAnimation);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            HideWithAnimation();
        }

        private void MoveToAnotherDevice_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = MainViewModel.Instance;
            var selectedApp = (AppItemViewModel)((FrameworkElement)sender).DataContext;
            var persistedDeviceId = selectedApp.PersistedOutputDevice;

            var moveMenu = new ContextMenu();

            foreach (var dev in viewModel.AllDevices)
            {
                var newItem = new MenuItem { Header = dev.Device.DisplayName };
                newItem.Click += (_, __) =>
                {
                    viewModel.MoveAppToDevice(selectedApp, dev);

                    HideWithAnimation();
                };

                newItem.IsCheckable = true;
                newItem.IsChecked = (dev.Device.Id == persistedDeviceId);

                moveMenu.Items.Add(newItem);
            }

            var defaultItem = new MenuItem { Header = EarTrumpet.Properties.Resources.DefaultDeviceText };
            defaultItem.IsCheckable = true;
            defaultItem.IsChecked = (string.IsNullOrWhiteSpace(persistedDeviceId));
            defaultItem.Click += (_, __) =>
            {
                viewModel.MoveAppToDevice(selectedApp, null);
                HideWithAnimation();
            };
            moveMenu.Items.Insert(0, defaultItem);

            moveMenu.Items.Insert(1, new Separator());

            moveMenu.PlacementTarget = (UIElement)sender;
            moveMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            moveMenu.IsOpen = true;
        }

        public void PositionAndShow(Window relativeTo, AppExpandedEventArgs e)
        {
            var taskbarState = TaskbarService.GetWinTaskbarState();
            var HEADER_SIZE = (double)App.Current.Resources["DeviceTitleCellHeight"];
            var ITEM_SIZE = (double)App.Current.Resources["AppItemCellHeight"];
            var PopupBorderSize = (Thickness)App.Current.Resources["PopupBorderThickness"];
            var volumeListMargin = (Thickness)App.Current.Resources["VolumeAppListMargin"];

            DataContext = e.ViewModel;

            Point offsetFromWindow = e.Container.TranslatePoint(new Point(0, 0), relativeTo);
            // Adjust for the title bar, top border and top margin on the app list.
            offsetFromWindow.Y -= (HEADER_SIZE + volumeListMargin.Bottom + PopupBorderSize.Top);

            var popupHeight = HEADER_SIZE + (e.ViewModel.ChildApps.Count * ITEM_SIZE) + volumeListMargin.Bottom + volumeListMargin.Top;
            var popupOriginYScreenCoordinates = relativeTo.PointToScreen(new Point(0, 0)).Y + offsetFromWindow.Y;

            // If we flow off the bottom
            if (popupOriginYScreenCoordinates + popupHeight > taskbarState.TaskbarScreen.WorkingArea.Bottom)
            {
                popupOriginYScreenCoordinates = taskbarState.TaskbarScreen.WorkingArea.Bottom - popupHeight;

                // If we also flow off the top
                if (popupOriginYScreenCoordinates < taskbarState.TaskbarScreen.WorkingArea.Top)
                {
                    popupOriginYScreenCoordinates = taskbarState.TaskbarScreen.WorkingArea.Top;
                    popupHeight = taskbarState.TaskbarScreen.WorkingArea.Bottom - taskbarState.TaskbarScreen.WorkingArea.Top;
                }
            }

            Placement = System.Windows.Controls.Primitives.PlacementMode.Absolute;
            HorizontalOffset = relativeTo.PointToScreen(new Point(0, 0)).X + offsetFromWindow.X;
            VerticalOffset = popupOriginYScreenCoordinates;

            Width = ((FrameworkElement)e.Container).ActualWidth;
            Height = popupHeight;

            ShowWithAnimation();
        }
    }
}
