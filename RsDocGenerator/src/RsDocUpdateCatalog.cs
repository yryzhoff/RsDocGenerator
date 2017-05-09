﻿using System;
using System.IO;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.UI.ActionsRevised;
using System.Diagnostics;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace RsDocGenerator
{
    [Action("RsDocUpdateCatalog", "Update ReSharper Feature Catalog (RsFeatureCatalog.xml)", Id = 43292)]
    internal class RsDocUpdateCatalog : IExecutableAction
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            UpdateCatalog(context);
        }

        public static void UpdateCatalog(IDataContext context)
        {
            var featureKeeper = new FeatureKeeper(context);
            var featureDigger = new FeatureDigger(context);
            var configurableInspetions = featureDigger.GetConfigurableInspections();
            var staticInspetions = featureDigger.GetStaticInspections();
            var contextActions = featureDigger.GetContextActions();
            var quickFixes = featureDigger.GetQuickFixes();
            var fixesInScope = featureDigger.GetFixesInScope();
            var actionsInScope = featureDigger.GetContextActionsInScope();

            featureKeeper.AddFeatures(configurableInspetions);
            featureKeeper.AddFeatures(staticInspetions);
            featureKeeper.AddFeatures(contextActions);
            featureKeeper.AddFeatures(quickFixes);
            featureKeeper.AddFeatures(fixesInScope);
            featureKeeper.AddFeatures(actionsInScope);

            featureKeeper.CloseSession();

            MessageBox.Show(String.Format("ReSharper Feature Catalog (RsFeatureCatalog.xml) is updated successfully according to version {0}.",
                    GeneralHelpers.GetCurrentVersion()),
                "Export completed", MessageBoxButtons.OK);
        }
    }
}