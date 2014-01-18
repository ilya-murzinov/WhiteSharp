using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.Finders;
using WhiteSharp;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;

namespace WhiteSharp.UITests
{
    [TestClass]
    public class PerformanceTest
    {
        [TestMethod]
        public void Test()
        {
            var w = Desktop.Instance.Windows().Find(x => x.Title.Contains("MainWindow"));
            var wnd = new UIWindow("MainWindow");
            List<string> AutomationIdList = new List<string>
            {
                "AComboBox",
                "EditableComboBox",
                "OpenHorizontalSplitterButton",
                "ListBoxWithVScrollBar",
                "CheckedListBox",
                "ListBoxWithVScrollBar",
                "OpenListView",
                "CustomUIItemScenario",
                "ControlsTab"
            };
            List<UIControl> results = new List<UIControl>();

            var number = 10;
            var startAll = DateTime.Now;
            //for (int i = 0; i < number; i++)
            //{
            //    var starti = DateTime.Now;
            //    foreach (var id in AutomationIdList)
            //    {
            //        var start = DateTime.Now;
            //        w.Get(SearchCriteria.ByAutomationId(id));
            //        Trace.WriteLine((DateTime.Now - start).TotalSeconds);
            //    }
            //    Trace.WriteLine("---------" + (DateTime.Now - starti).TotalSeconds);
            //}
            //Trace.WriteLine("-----------------------------------------------------");
            //Trace.WriteLine("---------" + (DateTime.Now - startAll).TotalSeconds);

            startAll = DateTime.Now;
            for (int i = 0; i < number; i++)
            {
                var starti = DateTime.Now;
                foreach (var id in AutomationIdList)
                {
                    var start = DateTime.Now;
                    results.Add(wnd.FindControl(By.AutomationId(id)));
                    Trace.WriteLine((DateTime.Now - start).TotalSeconds);
                }
                Trace.WriteLine("---------" + (DateTime.Now - starti).TotalSeconds);
            }
            Trace.WriteLine("-----------------------------------------------------");
            Trace.WriteLine("---------" + (DateTime.Now - startAll).TotalSeconds);

        }
    }
}
