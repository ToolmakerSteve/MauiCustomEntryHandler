using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiTextField;
#elif MONOANDROID
using PlatformView = AndroidX.AppCompat.Widget.AppCompatEditText;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.TextBox;
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.Entry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Microsoft.Maui.Handlers;
using ViewExtensions = Microsoft.Maui.Platform.ViewExtensions;
using static Android.Views.View;
using static Android.Widget.TextView;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using TouchEventArgs = Android.Views.View.TouchEventArgs;
using Microsoft.Maui.Platform;

namespace MauiCustomEntryHandler
{
    /// <summary>
    /// Android platform partial of class. Based on Maui src\Core\src\Handlers\Entry\EntryHandler.Android.cs.
    /// </summary>
    public partial class MyEntryHandler : Microsoft.Maui.Handlers.EntryHandler
    {
        protected override AppCompatEditText CreatePlatformView()
        {
            var nativeEntry = new AppCompatEditText(Context);
            var myentry = VirtualView as MyEntry;

            if (myentry.UnderlineThickness == 0)
            {   // Hide Underline.
                nativeEntry.PaintFlags &= ~Android.Graphics.PaintFlags.UnderlineText;
                //nativeEntry.Background = null;
                //nativeEntry.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            }
            else
            {   // Show Underline. (Is only under the typed text, not the whole control.)
                nativeEntry.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
                // TODO: Line thickness and color. For color, see https://stackoverflow.com/a/62486103/199364.
                // For thickness, probably need to "nest controls", similar to Windows implementation.
            }

            return nativeEntry;
        }



        // ========== TBD: Which of below are needed? (Most can be inherited) ==========
        //}
        //public partial class EntryHandler : Microsoft.Maui.Handlers.ViewHandler<IEntry, AppCompatEditText>
        //{
        Drawable? _clearButtonDrawable;

        // Returns the default 'X' char drawable in the AppCompatEditText.
        protected virtual Drawable GetClearButtonDrawable() =>
            _clearButtonDrawable ??= ContextCompat.GetDrawable(Context, Resource.Drawable.abc_ic_clear_material);

        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            platformView.TextChanged += OnTextChanged;
            platformView.FocusChange += OnFocusedChange;
            platformView.Touch += OnTouch;
            platformView.EditorAction += OnEditorAction;
        }

        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            _clearButtonDrawable = null;
            platformView.TextChanged -= OnTextChanged;
            platformView.FocusChange -= OnFocusedChange;
            platformView.Touch -= OnTouch;
            platformView.EditorAction -= OnEditorAction;
        }

        public static void MapBackground(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateBackground(entry);

        public static void MapText(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateText(entry);

        public static void MapTextColor(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateTextColor(entry);

        public static void MapIsPassword(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateIsPassword(entry);

        public static void MapHorizontalTextAlignment(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateHorizontalTextAlignment(entry);

        public static void MapVerticalTextAlignment(IEntryHandler handler, IEntry entry) =>
            handler?.PlatformView?.UpdateVerticalTextAlignment(entry);

        public static void MapIsTextPredictionEnabled(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateIsTextPredictionEnabled(entry);

        public static void MapMaxLength(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateMaxLength(entry);

        public static void MapPlaceholder(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdatePlaceholder(entry);

        public static void MapPlaceholderColor(IEntryHandler handler, IEntry entry)
        {
            if (handler is EntryHandler platformHandler)
                handler.PlatformView?.UpdatePlaceholderColor(entry);
        }

        public static void MapFont(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateFont(entry, handler.GetRequiredService<IFontManager>());

        public static void MapIsReadOnly(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateIsReadOnly(entry);

        public static void MapKeyboard(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateKeyboard(entry);

        public static void MapReturnType(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateReturnType(entry);

        public static void MapCharacterSpacing(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateCharacterSpacing(entry);

        public static void MapCursorPosition(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateCursorPosition(entry);

        public static void MapSelectionLength(IEntryHandler handler, IEntry entry) =>
            handler.PlatformView?.UpdateSelectionLength(entry);

        public static void MapClearButtonVisibility(IEntryHandler handler, IEntry entry)
        {
            if (handler is MyEntryHandler platformHandler)
                handler.PlatformView?.UpdateClearButtonVisibility(entry, platformHandler.GetClearButtonDrawable);
        }

        void OnTextChanged(object? sender, TextChangedEventArgs e) =>
            VirtualView?.UpdateText(e);

        // This will eliminate additional native property setting if not required.
        void OnFocusedChange(object? sender, FocusChangeEventArgs e)
        {
            if (VirtualView?.ClearButtonVisibility == ClearButtonVisibility.WhileEditing)
                UpdateValue(nameof(IEntry.ClearButtonVisibility));
        }

        // Check whether the touched position inbounds with clear button.
        void OnTouch(object? sender, TouchEventArgs e) =>
            e.Handled =
                VirtualView?.ClearButtonVisibility == ClearButtonVisibility.WhileEditing &&
                PlatformView.HandleClearButtonTouched(VirtualView.FlowDirection, e, GetClearButtonDrawable);

        void OnEditorAction(object? sender, EditorActionEventArgs e)
        {
            if (e.IsCompletedAction())
            {
                // TODO: Dismiss keyboard for hardware / physical keyboards

                VirtualView?.Completed();
            }

            e.Handled = true;
        }
    }
}
