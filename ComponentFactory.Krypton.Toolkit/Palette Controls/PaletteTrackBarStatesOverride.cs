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

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Allow the palette to be overriden optionally.
    /// </summary>
    public class PaletteTrackBarStatesOverride : GlobalId
    {
	    /// <summary>
        /// Initialize a new instance of the PaletteTrackBarStatesOverride class.
        /// </summary>
        /// <param name="normalStates">Normal state values.</param>
        /// <param name="overrideStates">Override state values.</param>
        /// <param name="overrideState">State to override.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PaletteTrackBarStatesOverride(PaletteTrackBarRedirect normalStates,
                                             PaletteTrackBarStates overrideStates,
                                             PaletteState overrideState)
        {
            Debug.Assert(normalStates != null);
            Debug.Assert(overrideStates != null);

            // Validate incoming references
            if (normalStates == null)
            {
                throw new ArgumentNullException(nameof(normalStates));
            }

            if (overrideStates == null)
            {
                throw new ArgumentNullException(nameof(overrideStates));
            }

            // Create the triple override instances
            Back = normalStates.Back;
            Tick = new PaletteElementColorInheritOverride(normalStates.Tick, overrideStates.Tick);
            Track = new PaletteElementColorInheritOverride(normalStates.Track, overrideStates.Track);
            Position = new PaletteElementColorInheritOverride(normalStates.Position, overrideStates.Position);

            // Do not apply an override by default
            Apply = false;

            // Always override the state
            Override = true;
            OverrideState = overrideState;
        }

	    /// <summary>
        /// Update the the normal and override palettes.
        /// </summary>
        /// <param name="normalStates">New normal palette.</param>
        /// <param name="overrideStates">New override palette.</param>
        public void SetPalettes(PaletteTrackBarRedirect normalStates,
                                PaletteTrackBarStates overrideStates)
        {
            Tick.SetPalettes(normalStates.Tick, overrideStates.Tick);
            Track.SetPalettes(normalStates.Track, overrideStates.Track);
            Position.SetPalettes(normalStates.Position, overrideStates.Position);
        }

	    /// <summary>
        /// Gets and sets a value indicating if override should be applied.
        /// </summary>
        public bool Apply
        {
            get => Tick.Apply;

            set
            {
                Tick.Apply = value;
                Track.Apply = value;
                Position.Apply = value;
            }
        }

	    /// <summary>
        /// Gets and sets a value indicating if override state should be applied.
        /// </summary>
        public bool Override
        {
            get => Tick.Override;

            set
            {
                Tick.Override = value;
                Track.Override = value;
                Position.Override = value;
            }
        }

	    /// <summary>
        /// Gets and sets the override palette state to use.
        /// </summary>
        public PaletteState OverrideState
        {
            get => Tick.OverrideState;

            set
            {
                Tick.OverrideState = value;
                Track.OverrideState = value;
                Position.OverrideState = value;
            }
        }

	    /// <summary>
        /// Gets access to the back appearance.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining background appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteBack Back { get; }

	    /// <summary>
        /// Gets access to the tick appearance.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining tick appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteElementColorInheritOverride Tick { get; }

	    /// <summary>
        /// Gets access to the track appearance.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining track appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteElementColorInheritOverride Track { get; }

	    /// <summary>
        /// Gets access to the position appearance.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining position appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteElementColorInheritOverride Position { get; }
    }
}