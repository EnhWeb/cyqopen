using CYQ.Data.Table;
using CYQ.Visualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CYQ.Visualizer
{
    public class EnumerableVisualizer : DialogDebuggerVisualizer
    {
        public const string Description = "Enumerable Visualizer";
        override protected void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MDataTable dt = objectProvider.GetObject() as MDataTable;
            FormCreate.BindTable(windowService, dt, null);
        }
    }
    public class EnumerableVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, System.IO.Stream outgoingData)
        {
            target = MDataTable.CreateFrom(target as IEnumerable);
            base.GetData(target, outgoingData);

        }
    }
}
