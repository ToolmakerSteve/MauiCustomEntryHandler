#if WINDOWS
//using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
//using Microsoft.UI.Xaml.Media;
using Windows.System;
//using Windows.UI;
using static Microsoft.Maui.ElementHandlerExtensions;
using Border = Microsoft.UI.Xaml.Controls.Border;
using Color = Windows.UI.Color;
using Colors = Microsoft.UI.Colors;
using MauiColor = Microsoft.Maui.Graphics.Color;
using PlatformEntryType = Microsoft.Maui.Platform.MauiPasswordTextBox;
using PlatformView = Microsoft.UI.Xaml.Controls.Border; // Microsoft.UI.Xaml.FrameworkElement;
using SolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
using TextChangedEventArgs = Microsoft.UI.Xaml.Controls.TextChangedEventArgs;
using Thickness = Microsoft.UI.Xaml.Thickness;

namespace MauiCustomEntryHandler
{
    /// <summary>
    /// Windows platform partial of class. See Maui repo maui\src\Core\src\Handlers\Entry\EntryHandler.Windows.cs
    /// "TPlatformView" is "Microsoft.UI.Xaml.FrameworkElement". WAS "Microsoft.UI.Xaml.Controls.TextBox".
    /// </summary>
    public partial class MyEntryHandler : Microsoft.Maui.Handlers.ViewHandler<IEntry, PlatformView>   //EntryHandler
    {
        static readonly bool s_shouldBeDelayed = DeviceInfo.Idiom != DeviceIdiom.Desktop;

        /// <summary>
        /// Adapted from EntryHandler.CreatePlatformView.
        /// </summary>
        /// <returns></returns>
        protected override PlatformView CreatePlatformView()
        {
            var myentry = VirtualView as MyEntry;

            var textbox = new MauiPasswordTextBox
            {
                // From EntryHandler.
                IsObfuscationDelayed = s_shouldBeDelayed

                // TODO: pass entry properties through.
            };

            MauiColor color = myentry != null
                    ? myentry.UnderlineColor
                    : MyEntry.UnderlineColorProperty.DefaultValue as MauiColor;
            int thickness = myentry != null
                    ? myentry.UnderlineThickness
                    : (int)MyEntry.UnderlineThicknessProperty.DefaultValue;

            var border = new Border
            {
                Child = textbox,
                BorderBrush = color.ToPlatform(),
                BorderThickness = new Thickness(0, 0, 0, thickness)
            };


            return border;
        }


        private PlatformEntryType MyTextBox => (PlatformEntryType)PlatformView.Child;


        // ----- Based on Maui EntryHandler.Windows.cs -----

        protected override void ConnectHandler(PlatformView platformView)
        {
            var textbox = (PlatformEntryType)platformView.Child;
            textbox.KeyUp += OnPlatformKeyUp;
            textbox.TextChanged += OnPlatformTextChanged;
            textbox.SelectionChanged += OnPlatformSelectionChanged;
            platformView.Loaded += OnPlatformLoaded;
        }

        protected override void DisconnectHandler(PlatformView platformView)
        {
            var textbox = (PlatformEntryType)platformView.Child;
            platformView.Loaded -= OnPlatformLoaded;
            textbox.KeyUp -= OnPlatformKeyUp;
            textbox.TextChanged -= OnPlatformTextChanged;
            textbox.SelectionChanged -= OnPlatformSelectionChanged;
        }

        public static void MapText(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateText(entry);

        public static void MapIsPassword(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateIsPassword(entry);

        public static void MapBackground(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateBackground(entry);

        public static void MapTextColor(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateTextColor(entry);

        public static void MapHorizontalTextAlignment(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateHorizontalTextAlignment(entry);

        public static void MapVerticalTextAlignment(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateVerticalTextAlignment(entry);

        public static void MapIsTextPredictionEnabled(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateIsTextPredictionEnabled(entry);

        public static void MapMaxLength(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateMaxLength(entry);

        public static void MapPlaceholder(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdatePlaceholder(entry);

        public static void MapPlaceholderColor(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdatePlaceholderColor(entry);

        public static void MapIsReadOnly(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateIsReadOnly(entry);

        public static void MapFont(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateFont(entry, handler.GetRequiredService<IFontManager>());

        public static void MapReturnType(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateReturnType(entry);

        public static void MapClearButtonVisibility(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateClearButtonVisibility(entry);

        public static void MapCharacterSpacing(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateCharacterSpacing(entry);

        public static void MapKeyboard(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateKeyboard(entry);

        public static void MapCursorPosition(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateCursorPosition(entry);

        public static void MapSelectionLength(MyEntryHandler handler, IEntry entry) =>
            handler.MyTextBox?.UpdateSelectionLength(entry);

        void OnPlatformTextChanged(object sender, TextChangedEventArgs args)
        {
            var textbox = sender as PlatformEntryType;
            if (textbox == null)
                textbox = MyTextBox;

            VirtualView?.UpdateText(textbox.Password);
        }

        void OnPlatformKeyUp(object? sender, KeyRoutedEventArgs args)
        {
            if (args?.Key != VirtualKey.Enter)
                return;

            if (VirtualView?.ReturnType == ReturnType.Next)
            {
                PlatformView?.TryMoveFocus(FocusNavigationDirection.Next);
            }
            else
            {
                // TODO: Hide the soft keyboard; this matches the behavior of .NET MAUI on Android/iOS
            }

            VirtualView?.Completed();
        }

        void OnPlatformSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (VirtualView.CursorPosition != MyTextBox.SelectionStart)
                VirtualView.CursorPosition = MyTextBox.SelectionStart;

            if (VirtualView.SelectionLength != MyTextBox.SelectionLength)
                VirtualView.SelectionLength = MyTextBox.SelectionLength;
        }

        void OnPlatformLoaded(object sender, RoutedEventArgs e) =>
            MauiTextBox.InvalidateAttachedProperties(PlatformView);
    }
}
#endif