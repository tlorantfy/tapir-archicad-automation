﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using TapirGrasshopperPlugin.Data;
using TapirGrasshopperPlugin.Utilities;
using TapirGrasshopperPlugin.Components;

namespace TapirGrasshopperPlugin.Components.ClassificationsComponents
{
    public class ClassificationSystemValueList : ArchicadAccessorValueList
    {
        public ClassificationSystemValueList () :
            base ("Classification Systems", "", "Value List for Classification Systems.", "Classifications")
        {
        }

        public override void RefreshItems ()
        {
            CommandResponse response = SendArchicadCommand ("GetAllClassificationSystems", null);
            if (!response.Succeeded) {
                AddRuntimeMessage (GH_RuntimeMessageLevel.Error, response.GetErrorMessage ());
                return;
            }

            string previouslySelected = SelectedItems[0].Expression;

            ListItems.Clear ();

            AllClassificationSystems classificationSystems = response.Result.ToObject<AllClassificationSystems> ();
            foreach (ClassificationSystemDetailsObj system in classificationSystems.ClassificationSystems) {
                var item = new GH_ValueListItem (system.ToString (), '"' + system.ClassificationSystemId.Guid + '"');
                if (item.Expression == previouslySelected) {
                    item.Selected = true;
                }
                ListItems.Add (item);
            }
            if (SelectedItems.Count == 0) {
                ListItems[0].Selected = true;
            }
        }

        protected override System.Drawing.Bitmap Icon => TapirGrasshopperPlugin.Properties.Resources.ClassificationSystemsValueList;

        public override Guid ComponentGuid => new Guid ("a4206a77-3e1e-42e1-b220-dbd7aafdf8f5");
    }
}