using CYQ.Data.Table;
using CYQ.Visualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            if (dt != null)
            {
                try
                {
                    FormCreate.BindTable(windowService, dt, null);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                
            }
        }
    }
    public class EnumerableVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, System.IO.Stream outgoingData)
        {
            if (target is NameObjectCollectionBase)
            {
                target = MDataTable.CreateFrom(target as NameObjectCollectionBase);
            }
            else
            {
                target = MDataTable.CreateFrom(target as IEnumerable);
            }
            base.GetData(target, outgoingData);

        }
    }
}
