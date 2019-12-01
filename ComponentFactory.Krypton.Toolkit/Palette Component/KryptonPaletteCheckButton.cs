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

using System.ComponentModel;

namespace ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Storage of palette check button states.
    /// </summary>
    public class KryptonPaletteCheckButton : Storage
    {
	    /// <summary>
        /// Initialize a new instance of the KryptonPaletteButtonBase class.
        /// </summary>
        /// <param name="redirect">Redirector to inherit values from.</param>
        /// <param name="backStyle">Background style.</param>
        /// <param name="borderStyle">Border style.</param>
        /// <param name="contentStyle">Content style.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public KryptonPaletteCheckButton(PaletteRedirect redirect,
                                         PaletteBackStyle backStyle,
                                         PaletteBorderStyle borderStyle,
                                         PaletteContentStyle contentStyle,
                                         NeedPaintHandler needPaint) 
        {
            // Create the storage objects
            OverrideDefault = new PaletteTripleRedirect(redirect, backStyle, borderStyle, contentStyle, needPaint);
            OverrideFocus = new PaletteTripleRedirect(redirect, backStyle, borderStyle, contentStyle, needPaint);
            StateCommon = new PaletteTripleRedirect(redirect, backStyle, borderStyle, contentStyle, needPaint);
            StateDisabled = new PaletteTriple(StateCommon, needPaint);
            StateNormal = new PaletteTriple(StateCommon, needPaint);
            StateTracking = new PaletteTriple(StateCommon, needPaint);
            StatePressed = new PaletteTriple(StateCommon, needPaint);
            StateCheckedNormal = new PaletteTriple(StateCommon, needPaint);
            StateCheckedTracking = new PaletteTriple(StateCommon, needPaint);
            StateCheckedPressed = new PaletteTriple(StateCommon, needPaint);
        }

	    /// <summary>
        /// Update the redirector with new reference.
        /// </summary>
        /// <param name="redirect">Target redirector.</param>
        public void SetRedirector(PaletteRedirect redirect)
        {
            OverrideDefault.SetRedirector(redirect);
            OverrideFocus.SetRedirector(redirect);
            StateCommon.SetRedirector(redirect);
        }

	    /// <summary>
        /// Gets a value indicating if all values are default.
        /// </summary>
        [Browsable(false)]
        public override bool IsDefault => StateCommon.IsDefault &&
                                          OverrideDefault.IsDefault &&
                                          OverrideFocus.IsDefault &&
                                          StateDisabled.IsDefault &&
                                          StateNormal.IsDefault &&
                                          StateTracking.IsDefault &&
                                          StatePressed.IsDefault &&
                                          StateCheckedNormal.IsDefault &&
                                          StateCheckedTracking.IsDefault &&
                                          StateCheckedPressed.IsDefault;

	    /// <summary>
        /// Populate values from the base palette.
        /// </summary>
        public void PopulateFromBase()
        {
            // Populate only the designated styles
            OverrideDefault.PopulateFromBase(PaletteState.NormalDefaultOverride);
            OverrideFocus.PopulateFromBase(PaletteState.FocusOverride);
            StateDisabled.PopulateFromBase(PaletteState.Disabled);
            StateNormal.PopulateFromBase(PaletteState.Normal);
            StateTracking.PopulateFromBase(PaletteState.Tracking);
            StatePressed.PopulateFromBase(PaletteState.Pressed);
            StateCheckedNormal.PopulateFromBase(PaletteState.CheckedNormal);
            StateCheckedTracking.PopulateFromBase(PaletteState.CheckedTracking);
            StateCheckedPressed.PopulateFromBase(PaletteState.CheckedPressed);
        }

	    /// <summary>
        /// Gets access to the common button appearance that other states can override.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining common button appearance that other states can override.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTripleRedirect StateCommon { get; }

        private bool ShouldSerializeStateCommon()
        {
            return !StateCommon.IsDefault;
        }

        /// <summary>
        /// Gets access to the disabled button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining disabled button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StateDisabled { get; }

        private bool ShouldSerializeStateDisabled()
        {
            return !StateDisabled.IsDefault;
        }

        /// <summary>
        /// Gets access to the normal button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining normal button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StateNormal { get; }

        private bool ShouldSerializeStateNormal()
        {
            return !StateNormal.IsDefault;
        }

        /// <summary>
        /// Gets access to the hot tracking button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining hot tracking button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StateTracking { get; }

        private bool ShouldSerializeStateTracking()
        {
            return !StateTracking.IsDefault;
        }

        /// <summary>
        /// Gets access to the pressed button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining pressed button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StatePressed { get; }

        private bool ShouldSerializeStatePressed()
        {
            return !StatePressed.IsDefault;
        }

        /// <summary>
        /// Gets access to the normal checked button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining normal checked button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StateCheckedNormal { get; }

        private bool ShouldSerializeStateCheckedNormal()
        {
            return !StateCheckedNormal.IsDefault;
        }

        /// <summary>
        /// Gets access to the hot tracking checked button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining hot tracking checked button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StateCheckedTracking { get; }

        private bool ShouldSerializeStateCheckedTracking()
        {
            return !StateCheckedTracking.IsDefault;
        }

        /// <summary>
        /// Gets access to the pressed checked button appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining pressed checked button appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTriple StateCheckedPressed { get; }

        private bool ShouldSerializeStateCheckedPressed()
        {
            return !StateCheckedPressed.IsDefault;
        }

        /// <summary>
        /// Gets access to the normal button appearance when default.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining normal button appearance when default.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTripleRedirect OverrideDefault { get; }

        private bool ShouldSerializeOverrideDefault()
        {
            return !OverrideDefault.IsDefault;
        }

        /// <summary>
        /// Gets access to the button appearance when it has focus.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining button appearance when it has focus.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteTripleRedirect OverrideFocus { get; }

        private bool ShouldSerializeOverrideFocus()
        {
            return !OverrideFocus.IsDefault;
        }
    }
}
