﻿// *****************************************************************************
// BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
//  © Component Factory Pty Ltd, 2006-2019, All rights reserved.
// The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to license terms.
// 
//  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV) 2017 - 2019. All rights reserved. (https://github.com/Wagnerp/Krypton-NET-5.470)
//  Version 5.470.0.0  www.ComponentFactory.com
// *****************************************************************************

using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// View element that represents a single gallery item.
    /// </summary>
    internal class ViewDrawMenuImageSelectItem : ViewDrawButton,
                                                 IContentValues
    {
	    private readonly KryptonContextMenuImageSelect _imageSelect;
        private readonly ViewLayoutMenuItemSelect _layout;
        private readonly MenuImageSelectController _controller;
        private readonly NeedPaintHandler _needPaint;
        private ImageList _imageList;
        private int _imageIndex;

        /// <summary>
        /// Initialize a new instance of the ViewDrawMenuImageSelectItem class.
        /// </summary>
        /// <param name="viewManager">Owning view manager instance.</param>
        /// <param name="imageSelect">Owning image select instance.</param>
        /// <param name="palette">Palette used to recover values.</param>
        /// <param name="layout">Reference to item layout.</param>
        /// <param name="needPaint">Delegate for requesting paints.</param>
        public ViewDrawMenuImageSelectItem(ViewContextMenuManager viewManager,
                                           KryptonContextMenuImageSelect imageSelect,
                                           IPaletteTriple palette,
                                           ViewLayoutMenuItemSelect layout,
                                           NeedPaintHandler needPaint)
            : base(palette, palette, palette, palette, 
                   null, null, VisualOrientation.Top, false)
        {
            _imageSelect = imageSelect;
            _layout = layout;
            _needPaint = needPaint;

            // We provide the content for the button
            ButtonValues = this;

            // Need controller to handle tracking/pressing etc
            _controller = new MenuImageSelectController(viewManager, this, layout, needPaint);
            _controller.Click += OnItemClick;
            MouseController = _controller;
            SourceController = _controller;
            KeyController = _controller;
        }

        /// <summary>
        /// Obtains the String representation of this instance.
        /// </summary>
        /// <returns>User readable name of the instance.</returns>
        public override string ToString()
        {
            // Return the class name and instance identifier
            return "ViewDrawMenuImageSelectItem:" + Id;
        }

        /// <summary>
        /// Item is becoming tracked by the mouse.
        /// </summary>
        public bool IsTracking => (_imageSelect.TrackingIndex == _imageIndex);

        /// <summary>
        /// Item is becoming tracked by the mouse.
        /// </summary>
        public void Track()
        {
            if (_imageSelect.TrackingIndex != _imageIndex)
            {
                _imageSelect.TrackingIndex = _imageIndex;
            }
        }

        /// <summary>
        /// Item is becoming tracked by the mouse.
        /// </summary>
        public void Untrack()
        {
            if (_imageSelect.TrackingIndex == _imageIndex)
            {
                _imageSelect.TrackingIndex = -1;
            }
        }

        /// <summary>
        /// Sets the image list to use for the source of the image.
        /// </summary>
        public ImageList ImageList
        {
            set => _imageList = value;
        }

        /// <summary>
        /// Sets the index of the image to show.
        /// </summary>
        public int ImageIndex
        {
            set => _imageIndex = value;
        }

        /// <summary>
        /// Perform a render of the elements.
        /// </summary>
        /// <param name="context">Rendering context.</param>
        public override void Render(RenderContext context)
        {
            Debug.Assert(context != null);

            // If this item is being tracked, then show as tracking
            PaletteState tempState = ElementState;
            if (_imageSelect.TrackingIndex == _imageIndex)
            {
                switch (tempState)
                {
                    case PaletteState.Normal:
                        ElementState = PaletteState.Tracking;
                        break;
                    case PaletteState.CheckedNormal:
                        ElementState = PaletteState.CheckedTracking;
                        break;
                }
            }

            // Let base class draw using the temp state, then put back to original
            base.Render(context);
            ElementState = tempState;
        }

        /// <summary>
        /// Gets the content image.
        /// </summary>
        /// <param name="state">The state for which the image is needed.</param>
        /// <returns>Image value.</returns>
        public virtual Image GetImage(PaletteState state)
        {
            if ((_imageList != null) && (_imageIndex >= 0))
            {
                return _imageList.Images[_imageIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the image color that should be transparent.
        /// </summary>
        /// <param name="state">The state for which the image is needed.</param>
        /// <returns>Color value.</returns>
        public Color GetImageTransparentColor(PaletteState state)
        {
            return Color.Empty;
        }

        /// <summary>
        /// Gets the content short text.
        /// </summary>
        public string GetShortText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the content long text.
        /// </summary>
        public string GetLongText()
        {
            return string.Empty;
        }

        private void OnItemClick(object sender, MouseEventArgs e)
        {
            // Set new selection index
            _imageSelect.SelectedIndex = _imageIndex;

            //Fire a click item event : seb the right way ?? after selected index
            _imageSelect.OnClick(e);

            // Should we automatically try and close the context menu stack
            if (_imageSelect.AutoClose)
            {
                // Is the menu capable of being closed?
                if (_layout.CanCloseMenu)
                {
                    // Ask the original context menu definition, if we can close
                    CancelEventArgs cea = new CancelEventArgs();
                    _layout.Closing(cea);

                    if (!cea.Cancel)
                    {
                        // Close the menu from display and pass in the item clicked as the reason
                        _layout.Close(new CloseReasonEventArgs(ToolStripDropDownCloseReason.ItemClicked));
                    }
                }
            }

            _needPaint(this, new NeedLayoutEventArgs(true));
        }
    }
}
