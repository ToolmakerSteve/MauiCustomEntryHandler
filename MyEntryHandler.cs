using Microsoft.Maui.Handlers;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace MauiCustomEntryHandler
{
#if WINDOWS
    // Cross-platform partial of class. See Maui repo maui\src\Core\src\Handlers\Entry\EntryHandler.cs
    public partial class MyEntryHandler : IMyEntryHandler //: EntryHandler
    {
        // static c'tor.
        static MyEntryHandler()
        {
            // TBD: Fill MyMapper here by copying from Entry.Mapper, then add custom ones defined in MyEntry?
        }

        //public static IPropertyMapper<IEntry, IEntryHandler> MyMapper => Mapper;
        public static IPropertyMapper<IEntry, MyEntryHandler> MyMapper = new PropertyMapper<IEntry, MyEntryHandler>(ViewMapper)
        {
            // From Entry.
            [nameof(IEntry.Background)] = MapBackground,
            [nameof(IEntry.CharacterSpacing)] = MapCharacterSpacing,
            [nameof(IEntry.ClearButtonVisibility)] = MapClearButtonVisibility,
            [nameof(IEntry.Font)] = MapFont,
            [nameof(IEntry.IsPassword)] = MapIsPassword,
            [nameof(IEntry.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
            [nameof(IEntry.VerticalTextAlignment)] = MapVerticalTextAlignment,
            [nameof(IEntry.IsReadOnly)] = MapIsReadOnly,
            [nameof(IEntry.IsTextPredictionEnabled)] = MapIsTextPredictionEnabled,
            [nameof(IEntry.Keyboard)] = MapKeyboard,
            [nameof(IEntry.MaxLength)] = MapMaxLength,
            [nameof(IEntry.Placeholder)] = MapPlaceholder,
            [nameof(IEntry.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(IEntry.ReturnType)] = MapReturnType,
            [nameof(IEntry.Text)] = MapText,
            [nameof(IEntry.TextColor)] = MapTextColor,
            [nameof(IEntry.CursorPosition)] = MapCursorPosition,
            [nameof(IEntry.SelectionLength)] = MapSelectionLength,
            // From MyEntry
            [nameof(MyEntry.UnderlineThickness)] = MapUnderlineThickness
        };

        // TBD: What is this for? Cloned one on Entry.
        private static void MapUnderlineThickness(MyEntryHandler arg1, IEntry arg2)
        {
        }


        public MyEntryHandler() : base(MyMapper)
        {
        }

        //IEntry IEntryHandler.VirtualView => throw new NotImplementedException();

        //IView IViewHandler.VirtualView => throw new NotImplementedException();

        //IElement IElementHandler.VirtualView => throw new NotImplementedException();

        //TextBox IEntryHandler.PlatformView => throw new NotImplementedException();

        //object IElementHandler.PlatformView => throw new NotImplementedException();

        //bool IViewHandler.HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //object IViewHandler.ContainerView => throw new NotImplementedException();

        //IMauiContext IElementHandler.MauiContext => throw new NotImplementedException();

        //void IElementHandler.DisconnectHandler()
        //{
        //    throw new NotImplementedException();
        //}

        //Size IViewHandler.GetDesiredSize(double widthConstraint, double heightConstraint)
        //{
        //    throw new NotImplementedException();
        //}

        //void IElementHandler.Invoke(string command, object args)
        //{
        //    throw new NotImplementedException();
        //}

        //void IViewHandler.PlatformArrange(Rect frame)
        //{
        //    throw new NotImplementedException();
        //}

        //void IElementHandler.SetMauiContext(IMauiContext mauiContext)
        //{
        //    throw new NotImplementedException();
        //}

        //void IElementHandler.SetVirtualView(IElement view)
        //{
        //    throw new NotImplementedException();
        //}

        //void IElementHandler.UpdateValue(string property)
        //{
        //    throw new NotImplementedException();
        //}
    }
    // ========== MAY NEED BELOW FOR PLATFORMS NOT YET IMPLEMENTED ==========
#else
    public partial class MyEntryHandler : EntryHandler
    {
    }
#endif
}
