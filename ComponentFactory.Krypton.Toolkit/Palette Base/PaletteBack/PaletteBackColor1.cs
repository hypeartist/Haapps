// *****************************************************************************
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

namespace ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Implement storage for palette background details.
    /// </summary>
    public class PaletteBackColor1 : PaletteBack
    {
	    /// <summary>
        /// Initialize a new instance of the PaletteBackColor1 class.
        /// </summary>
        /// <param name="inherit">Source for inheriting defaulted values.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public PaletteBackColor1(IPaletteBack inherit,
                                 NeedPaintHandler needPaint)
            : base(inherit, needPaint)
        {
        }

	    /// <summary>
        /// Populate values from the base palette.
        /// </summary>
        /// <param name="state">Palette state to use when populating.</param>
        public new void PopulateFromBase(PaletteState state)
        {
            // Get the values and set into storage
            Color1 = GetBackColor1(state);
        }

	    /// <summary>
        /// Gets a value indicating if background should be drawn.
        /// </summary>
        [Browsable(false)]
        public new InheritBool Draw
        {
            get => base.Draw;
            set => base.Draw = value;
        }

	    /// <summary>
        /// Gets the graphics hint for drawing the background.
        /// </summary>
        [Browsable(false)]
        public new PaletteGraphicsHint GraphicsHint
        {
            get => base.GraphicsHint;
            set => base.GraphicsHint = value;
        }

	    /// <summary>
        /// Gets and sets the second background color.
        /// </summary>
        [Browsable(false)]
        public new Color Color2
        {
            get => base.Color2;
            set => base.Color2 = value;
        }

	    /// <summary>
        /// Gets and sets the color drawing style.
        /// </summary>
        [Browsable(false)]
        public new PaletteColorStyle ColorStyle
        {
            get => base.ColorStyle;
            set => base.ColorStyle = value;
        }

	    /// <summary>
        /// Gets and set the color alignment.
        /// </summary>
        [Browsable(false)]
        public new PaletteRectangleAlign ColorAlign
        {
            get => base.ColorAlign;
            set => base.ColorAlign = value;
        }

	    /// <summary>
        /// Gets and sets the color angle.
        /// </summary>
        [Browsable(false)]
        public new float ColorAngle
        {
            get => base.ColorAngle;
            set => base.ColorAngle = value;
        }

	    /// <summary>
        /// Gets and sets the background image.
        /// </summary>
        [Browsable(false)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }

	    /// <summary>
        /// Gets and sets the background image style.
        /// </summary>
        [Browsable(false)]
        public new PaletteImageStyle ImageStyle
        {
            get => base.ImageStyle;
            set => base.ImageStyle = value;
        }

	    /// <summary>
        /// Gets and set the image alignment.
        /// </summary>
        [Browsable(false)]
        public new PaletteRectangleAlign ImageAlign
        {
            get => base.ImageAlign;
            set => base.ImageAlign = value;
        }
    }
}
