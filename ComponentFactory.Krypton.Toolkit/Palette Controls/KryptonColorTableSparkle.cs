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
using System.Diagnostics;
using Microsoft.Win32;

namespace ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Provide KryptonColorTable values using an array of Color values as the source.
    /// </summary>
    public class KryptonColorTableSparkle : KryptonColorTable
    {
	    private static readonly Color _menuBorder = Color.Black;
        private static readonly Color _menuItemText = Color.Black;
        private static readonly Color _contextMenuBackground = Color.FromArgb(240, 240, 240);
        private static readonly Color _imageMarginMiddle = Color.FromArgb(226, 227, 227);
        private static readonly Color _imageMarginEnd = Color.White;
        private static Font _menuToolFont;
        private static Font _statusFont;

        private readonly Color[] _colors;
        private readonly Color[] _sparkleColors;

        static KryptonColorTableSparkle()
        {
            // Get the font settings from the system
            DefineFonts();

            // We need to notice when system color settings change
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        }

        /// <summary>
        /// Initialize a new instance of the KryptonColorTableSparkle class.
        /// </summary>
        /// <param name="colors">Source of ribbon colors.</param>
        /// <param name="sparkleColors">Source of sparkle colors.</param>
        /// <param name="roundedEdges">Should have rounded edges.</param>
        /// <param name="palette">Associated palette instance.</param>
        public KryptonColorTableSparkle(Color[] colors,
                                        Color[] sparkleColors,
                                        InheritBool roundedEdges,
                                        IPalette palette)
            : base(palette)
        {
            Debug.Assert(colors != null);
            Debug.Assert(sparkleColors != null);

            _colors = colors;
            _sparkleColors = sparkleColors;
            UseRoundedEdges = roundedEdges;
        }

        /// <summary>
        /// Gets a value indicating if rounded egdes are required.
        /// </summary>
        public override InheritBool UseRoundedEdges { get; }

        /// <summary>
        /// Gets the border color for a button being pressed.
        /// </summary>
        public override Color ButtonPressedBorder => Color.Black;

        /// <summary>
        /// Gets the background starting color for a button being pressed.
        /// </summary>
        public override Color ButtonPressedGradientBegin => _sparkleColors[8];

        /// <summary>
        /// Gets the background middle color for a button being pressed.
        /// </summary>
        public override Color ButtonPressedGradientMiddle => _sparkleColors[8];

        /// <summary>
        /// Gets the background ending color for a button being pressed.
        /// </summary>
        public override Color ButtonPressedGradientEnd => _sparkleColors[9];

        /// <summary>
        /// Gets the highlight background for a pressed button.
        /// </summary>
        public override Color ButtonPressedHighlight => _sparkleColors[8];

        /// <summary>
        /// Gets the highlight border for a pressed button.
        /// </summary>
        public override Color ButtonPressedHighlightBorder => _colors[(int)SchemeOfficeColors.ButtonBorder];

        /// <summary>
        /// Gets the border color for a button being selected.
        /// </summary>
        public override Color ButtonSelectedBorder => Color.Black;

        /// <summary>
        /// Gets the background starting color for a button being selected.
        /// </summary>
        public override Color ButtonSelectedGradientBegin => _sparkleColors[6];

        /// <summary>
        /// Gets the background middle color for a button being selected.
        /// </summary>
        public override Color ButtonSelectedGradientMiddle => _sparkleColors[17];

        /// <summary>
        /// Gets the background ending color for a button being selected.
        /// </summary>
        public override Color ButtonSelectedGradientEnd => _sparkleColors[7];

        /// <summary>
        /// Gets the highlight background for a selected button.
        /// </summary>
        public override Color ButtonSelectedHighlight => _sparkleColors[12];

        /// <summary>
        /// Gets the highlight border for a selected button.
        /// </summary>
        public override Color ButtonSelectedHighlightBorder => _colors[(int)SchemeOfficeColors.ButtonBorder];

        /// <summary>
        /// Gets the background starting color for a checked button.
        /// </summary>
        public override Color ButtonCheckedGradientBegin => _sparkleColors[10];

        /// <summary>
        /// Gets the background middle color for a checked button.
        /// </summary>
        public override Color ButtonCheckedGradientMiddle => _sparkleColors[10];

        /// <summary>
        /// Gets the background ending color for a checked button.
        /// </summary>
        public override Color ButtonCheckedGradientEnd => _sparkleColors[11];

        /// <summary>
        /// Gets the highlight background for a checked button.
        /// </summary>
        public override Color ButtonCheckedHighlight => _sparkleColors[10];

        /// <summary>
        /// Gets the highlight border for a checked button.
        /// </summary>
        public override Color ButtonCheckedHighlightBorder => _colors[(int)SchemeOfficeColors.ButtonBorder];

        /// <summary>
        /// Get background of the check mark area.
        /// </summary>
        public override Color CheckBackground => _sparkleColors[20];

        /// <summary>
        /// Get background of a pressed check mark area.
        /// </summary>
        public override Color CheckPressedBackground => _sparkleColors[20];

        /// <summary>
        /// Get background of a selected check mark area.
        /// </summary>
        public override Color CheckSelectedBackground => _sparkleColors[20];

        /// <summary>
        /// Gets the light color used to draw grips.
        /// </summary>
        public override Color GripLight => _colors[(int)SchemeOfficeColors.GripLight];

        /// <summary>
        /// Gets the dark color used to draw grips.
        /// </summary>
        public override Color GripDark => _colors[(int)SchemeOfficeColors.GripDark];

        /// <summary>
        /// Gets the starting color for the context menu margin.
        /// </summary>
        public override Color ImageMarginGradientBegin => _colors[(int)SchemeOfficeColors.ImageMargin];

        /// <summary>
        /// Gets the middle color for the context menu margin.
        /// </summary>
        public override Color ImageMarginGradientMiddle => _imageMarginMiddle;

        /// <summary>
        /// Gets the ending color for the context menu margin.
        /// </summary>
        public override Color ImageMarginGradientEnd => _imageMarginEnd;

        /// <summary>
        /// Gets the starting color for the context menu margin revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientBegin => _colors[(int)SchemeOfficeColors.ImageMargin];

        /// <summary>
        /// Gets the middle color for the context menu margin revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientMiddle => _colors[(int)SchemeOfficeColors.ImageMargin];

        /// <summary>
        /// Gets the ending color for the context menu margin revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientEnd => _colors[(int)SchemeOfficeColors.ImageMargin];

        /// <summary>
        /// Gets the color of the border around menus.
        /// </summary>
        public override Color MenuBorder => _menuBorder;

        /// <summary>
        /// Gets the border color for around the menu item.
        /// </summary>
        public override Color MenuItemBorder => _menuBorder;

        /// <summary>
        /// Gets the color of a selected menu item.
        /// </summary>
        public override Color MenuItemSelected => _colors[(int)SchemeOfficeColors.ButtonBorder];

        /// <summary>
        /// Gets the starting color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientBegin => _colors[(int)SchemeOfficeColors.ToolStripBegin];

        /// <summary>
        /// Gets the end color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientEnd => _colors[(int)SchemeOfficeColors.ToolStripEnd];

        /// <summary>
        /// Gets the middle color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientMiddle => _colors[(int)SchemeOfficeColors.ToolStripMiddle];

        /// <summary>
        /// Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientBegin => _colors[(int)SchemeOfficeColors.ButtonBorder];

        /// <summary>
        /// Gets the end color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientEnd => _colors[(int)SchemeOfficeColors.ButtonBorder];

        /// <summary>
        /// Gets the starting color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientBegin => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the end color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientEnd => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientBegin => _colors[(int)SchemeOfficeColors.OverflowBegin];

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientEnd => _colors[(int)SchemeOfficeColors.OverflowEnd];

        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientMiddle => _colors[(int)SchemeOfficeColors.OverflowMiddle];

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientBegin => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientEnd => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the light separator color.
        /// </summary>
        public override Color SeparatorLight => _colors[(int)SchemeOfficeColors.SeparatorLight];

        /// <summary>
        /// Gets the dark separator color.
        /// </summary>
        public override Color SeparatorDark => _colors[(int)SchemeOfficeColors.SeparatorDark];

        /// <summary>
        /// Gets the starting color for the status strip background.
        /// </summary>
        public override Color StatusStripGradientBegin => _colors[(int)SchemeOfficeColors.StatusStripLight];

        /// <summary>
        /// Gets the ending color for the status strip background.
        /// </summary>
        public override Color StatusStripGradientEnd => _colors[(int)SchemeOfficeColors.StatusStripDark];

        /// <summary>
        /// Gets the text color used on the menu items.
        /// </summary>
        public override Color MenuItemText => _menuItemText;

        /// <summary>
        /// Gets the text color used on the menu strip.
        /// </summary>
        public override Color MenuStripText => _colors[(int)SchemeOfficeColors.TextLabelPanel];

        /// <summary>
        /// Gets the text color used on the tool strip.
        /// </summary>
        public override Color ToolStripText => _colors[(int)SchemeOfficeColors.TextLabelPanel];

        /// <summary>
        /// Gets the text color used on the status strip.
        /// </summary>
        public override Color StatusStripText => _colors[(int)SchemeOfficeColors.StatusStripText];

        /// <summary>
        /// Gets the font used on the menu strip.
        /// </summary>
        public override Font MenuStripFont => _menuToolFont;

        /// <summary>
        /// Gets the font used on the tool strip.
        /// </summary>
        public override Font ToolStripFont => _menuToolFont;

        /// <summary>
        /// Gets the font used on the status strip.
        /// </summary>
        public override Font StatusStripFont => _statusFont;

        /// <summary>
        /// Gets the border color to use on the bottom edge of the ToolStrip.
        /// </summary>
        public override Color ToolStripBorder => _colors[(int)SchemeOfficeColors.ToolStripBorder];

        /// <summary>
        /// Gets the starting color for the content panel background.
        /// </summary>
        public override Color ToolStripContentPanelGradientBegin => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the ending color for the content panel background.
        /// </summary>
        public override Color ToolStripContentPanelGradientEnd => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the background color for drop down menus.
        /// </summary>
        public override Color ToolStripDropDownBackground => _contextMenuBackground;

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientBegin => _colors[(int)SchemeOfficeColors.ToolStripBegin];

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientEnd => _colors[(int)SchemeOfficeColors.ToolStripEnd];

        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientMiddle => _colors[(int)SchemeOfficeColors.ToolStripMiddle];

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientBegin => _colors[(int)SchemeOfficeColors.ToolStripBack];

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientEnd => _colors[(int)SchemeOfficeColors.ToolStripBack];

        private static void DefineFonts()
        {
            // Release existing resources
            _menuToolFont?.Dispose();

            _statusFont?.Dispose();

            // Create new font using system information
            _menuToolFont = new Font("Segoe UI", SystemFonts.MenuFont.SizeInPoints, FontStyle.Regular);
            _statusFont = new Font("Segoe UI", SystemFonts.StatusFont.SizeInPoints, FontStyle.Regular);
        }

        private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            // Update fonts to reflect any change in system settings
            DefineFonts();
        }
    }
}
