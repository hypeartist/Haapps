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

using System.ComponentModel.Design;

namespace ComponentFactory.Krypton.Toolkit
{
    internal class KryptonManagerActionList : DesignerActionList
    {
	    private readonly KryptonManager _manager;
        private readonly IComponentChangeService _service;

        /// <summary>
        /// Initialize a new instance of the KryptonManagerActionList class.
        /// </summary>
        /// <param name="owner">Designer that owns this action list instance.</param>
        public KryptonManagerActionList(KryptonManagerDesigner owner)
            : base(owner.Component)
        {
            // Remember the panel instance
            _manager = owner.Component as KryptonManager;

            // Cache service used to notify when a property has changed
            _service = (IComponentChangeService)GetService(typeof(IComponentChangeService));
        }

        /// <summary>
        /// Gets and sets the global palette mode.
        /// </summary>
        public PaletteModeManager GlobalPaletteMode
        {
            get => _manager.GlobalPaletteMode;

            set
            {
                if (_manager.GlobalPaletteMode != value)
                {
                    _service.OnComponentChanged(_manager, null, _manager.GlobalPaletteMode, value);
                    _manager.GlobalPaletteMode = value;
                }
            }
        }

        /// <summary>
        /// Returns the collection of DesignerActionItem objects contained in the list.
        /// </summary>
        /// <returns>A DesignerActionItem array that contains the items in this list.</returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            // Create a new collection for holding the single item we want to create
            DesignerActionItemCollection actions = new DesignerActionItemCollection();

            // This can be null when deleting a component instance at design time
            if (_manager != null)
            {
                // Add the list of panel specific actions
                actions.Add(new DesignerActionHeaderItem("Visuals"));
                actions.Add(new DesignerActionPropertyItem("GlobalPaletteMode", "Global Palette", "Visuals", "Global palette setting"));
            }

            return actions;
        }
    }
}