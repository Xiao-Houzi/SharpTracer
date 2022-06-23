using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SharpTracer.Base
{
    public static class UIHelpers
   {

      #region find parent

      /// <summary>
      /// Finds a parent of a given item on the visual tree.
      /// </summary>
      /// <typeparam name="T">The type of the queried item.</typeparam>
      /// <param name="child">A direct or indirect child of the
      /// queried item.</param>
      /// <returns>The first parent item that matches the submitted
      /// type parameter. If not matching item can be found, a null
      /// reference is being returned.</returns>
      public static T TryFindParent<T>(DependencyObject child)
        where T : DependencyObject
      {
         //get parent item
         DependencyObject parentObject = GetParentObject(child);

         //we've reached the end of the tree
         if (parentObject == null) return null;

         //check if the parent matches the type we're looking for
         T parent = parentObject as T;
         if (parent != null)
         {
            return parent;
         }
         else
         {
            //use recursion to proceed with next level
            return TryFindParent<T>(parentObject);
         }
      }


      /// <summary>
      /// This method is an alternative to WPF's
      /// <see cref="VisualTreeHelper.GetParent"/> method, which also
      /// supports content elements. Do note, that for content element,
      /// this method falls back to the logical tree of the element.
      /// </summary>
      /// <param name="child">The item to be processed.</param>
      /// <returns>The submitted item's parent, if available. Otherwise
      /// null.</returns>
      public static DependencyObject GetParentObject(DependencyObject child)
      {
         if (child == null) return null;
         ContentElement contentElement = child as ContentElement;

         if (contentElement != null)
         {
            DependencyObject parent = ContentOperations.GetParent(contentElement);
            if (parent != null) return parent;

            FrameworkContentElement fce = contentElement as FrameworkContentElement;
            return fce != null ? fce.Parent : null;
         }

         //if it's not a ContentElement, rely on VisualTreeHelper
         return VisualTreeHelper.GetParent(child);
      }

      #endregion


      #region update binding sources

      /// <summary>
      /// Recursively processes a given dependency object and all its
      /// children, and updates sources of all objects that use a
      /// binding expression on a given property.
      /// </summary>
      /// <param name="obj">The dependency object that marks a starting
      /// point. This could be a dialog window or a panel control that
      /// hosts bound controls.</param>
      /// <param name="properties">The properties to be updated if
      /// <paramref name="obj"/> or one of its childs provide it along
      /// with a binding expression.</param>
      public static void UpdateBindingSources(DependencyObject obj,
                                params DependencyProperty[] properties)
      {
         foreach (DependencyProperty depProperty in properties)
         {
            //check whether the submitted object provides a bound property
            //that matches the property parameters
            BindingExpression be = BindingOperations.GetBindingExpression(obj, depProperty);
            if (be != null) be.UpdateSource();
         }

         int count = VisualTreeHelper.GetChildrenCount(obj);
         for (int i = 0; i < count; i++)
         {
            //process child items recursively
            DependencyObject childObject = VisualTreeHelper.GetChild(obj, i);
            UpdateBindingSources(childObject, properties);
         }
      }

      #endregion


      /// <summary>
      /// Tries to locate a given item within the visual tree,
      /// starting with the dependency object at a given position. 
      /// </summary>
      /// <typeparam name="T">The type of the element to be found
      /// on the visual tree of the element at the given location.</typeparam>
      /// <param name="reference">The main element which is used to perform
      /// hit testing.</param>
      /// <param name="point">The position to be evaluated on the origin.</param>
      public static T TryFindFromPoint<T>(UIElement reference, Point point)
        where T : DependencyObject
      {
         DependencyObject element = reference.InputHitTest(point)
                                      as DependencyObject;
         if (element == null) return null;
         else if (element is T) return (T)element;
         else return TryFindParent<T>(element);
      }

        public static Cursor ConvertToCursor(UIElement uiElement, Point hotSpot)
        {
            // https://stackoverflow.com/questions/46805/custom-cursor-in-wpf
            // https://jigneshon.blogspot.com/2013/11/c-wpf-tutorial-how-to-use-custom.html
            // convert FrameworkElement to PNG stream
            using (var pngStream = new MemoryStream())
            {
                uiElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                uiElement.Arrange(new Rect(0, 0, uiElement.DesiredSize.Width, uiElement.DesiredSize.Height));
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)uiElement.DesiredSize.Width, (int)uiElement.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(uiElement);

                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                png.Save(pngStream);

                // write cursor header info
                using (var cursorStream = new MemoryStream())
                {
                    cursorStream.Write(new byte[2] { 0x00, 0x00 }, 0, 2);                               // ICONDIR: Reserved. Must always be 0.
                    cursorStream.Write(new byte[2] { 0x02, 0x00 }, 0, 2);                               // ICONDIR: Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image. Other values are invalid
                    cursorStream.Write(new byte[2] { 0x01, 0x00 }, 0, 2);                               // ICONDIR: Specifies number of images in the file.
                    cursorStream.Write(new byte[1] { (byte)uiElement.DesiredSize.Width }, 0, 1);          // ICONDIRENTRY: Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
                    cursorStream.Write(new byte[1] { (byte)uiElement.DesiredSize.Height }, 0, 1);         // ICONDIRENTRY: Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
                    cursorStream.Write(new byte[1] { 0x00 }, 0, 1);                                     // ICONDIRENTRY: Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
                    cursorStream.Write(new byte[1] { 0x00 }, 0, 1);                                     // ICONDIRENTRY: Reserved. Should be 0.
                    cursorStream.Write(new byte[2] { (byte)hotSpot.X, 0x00 }, 0, 2);                    // ICONDIRENTRY: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
                    cursorStream.Write(new byte[2] { (byte)hotSpot.Y, 0x00 }, 0, 2);                    // ICONDIRENTRY: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
                    cursorStream.Write(new byte[4] {                                                    // ICONDIRENTRY: Specifies the size of the image's data in bytes
                                          (byte)((pngStream.Length & 0x000000FF)),
                                          (byte)((pngStream.Length & 0x0000FF00) >> 8),
                                          (byte)((pngStream.Length & 0x00FF0000) >> 16),
                                          (byte)((pngStream.Length & 0xFF000000) >> 24)
                                       }, 0, 4);
                    cursorStream.Write(new byte[4] {                                                    // ICONDIRENTRY: Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
                                          (byte)0x16,
                                          (byte)0x00,
                                          (byte)0x00,
                                          (byte)0x00,
                                       }, 0, 4);

                    // copy PNG stream to cursor stream
                    pngStream.Seek(0, SeekOrigin.Begin);
                    pngStream.CopyTo(cursorStream);

                    // return cursor stream
                    cursorStream.Seek(0, SeekOrigin.Begin);
                    return new Cursor(cursorStream);
                }
            }
        }
    }
}
