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

namespace ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Redirect requests for image/text colors to remap.
    /// </summary>
    public class ButtonSpecRemapByContentCache : ButtonSpecRemapByContentBase
    {
	    private IPaletteContent _paletteContent;
        private PaletteState _paletteState;

        /// <summary>
        /// Initialize a new instance of the ButtonSpecRemapByContentCache class.
        /// </summary>
        /// <param name="target">Initial palette target for redirection.</param>
        /// <param name="buttonSpec">Reference to button specification.</param>
        public ButtonSpecRemapByContentCache(IPalette target,
                                             ButtonSpec buttonSpec)
            : base(target, buttonSpec)
        {
        }

        /// <summary>
        /// Set the palette content to use for remapping.
        /// </summary>
        /// <param name="paletteContent">Palette for requesting foreground colors.</param>
        public void SetPaletteContent(IPaletteContent paletteContent)
        {
            _paletteContent = paletteContent;
        }

        /// <summary>
        /// Set the palette state of the remapping element.
        /// </summary>
        /// <param name="paletteState">Palette state.</param>
        public void SetPaletteState(PaletteState paletteState)
        {
            _paletteState = paletteState;
        }

        /// <summary>
        /// Gets the palette content to use for remapping.
        /// </summary>
        public override IPaletteContent PaletteContent => _paletteContent;

        /// <summary>
        /// Gets the state of the remapping area
        /// </summary>
        public override PaletteState PaletteState => _paletteState;
    }
}
