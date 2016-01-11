using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;

namespace WhiteSharp.Controls
{
    public class Button : Control
    {
        public Button(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index)
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public override IControl ClickAnyway(bool doCheckErrorWindow = false)
        {
            var middleX = (BoundingRectangle.Right + BoundingRectangle.Left) / 2;
            var middleY = (BoundingRectangle.Top + BoundingRectangle.Bottom) / 2;

            Point? point = null;
            try
            {
                point = AutomationElement.GetClickablePoint();
            }
            catch (NoClickablePointException)
            {
                ((Window)Window).OnTop();
            }
            Mouse.Instance.Click(point != null
                ? new Point(point.Value.X, point.Value.Y)
                : new Point(middleX, middleY));

            Logging.Click(SearchCriteria.Identifiers);
            if (doCheckErrorWindow) TryFindErrorWindow();

            return this;
        }

        public new void SelectItem(string itemName)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 30 && (!Exists(By.Name(itemName))))
            {
                Click();
                Thread.Sleep(1000);
                try
                {
                    FindControl<Button>(By.Name(itemName)).Click();
                    return;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
            var button = FindControl<Button>(By.Name(itemName));
            button.Click();
        }

        public bool CheckItem(string itemName, bool shouldBeEnabled)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 10)
            {
                Click();
                Thread.Sleep(1000);
                if (Exists(By.Name(itemName).AndEnabled(shouldBeEnabled))) return true;

            }
            return false;
        }

        public void SelectFromComboboxInFilterWindow(By criteria, string value)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 30 && (!Exists(criteria)))
            {
                Click();
                Thread.Sleep(1000);
                try
                {
                    FindControl(By.Name("FilteringControl")).FindControl<ComboBox>(criteria).SelectItem(value);
                    return;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
            FindControl(By.Name("FilteringControl")).FindControl<ComboBox>(criteria).SelectItem(value);
        }

        public void SendToTextBoxInFilterWindow(By criteria, string value)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 30 && (!Exists(criteria)))
            {
                Click();
                Thread.Sleep(1000);
                try
                {
                    FindControl(By.Name("FilteringControl")).FindControl<TextBox>(criteria).Send(value);
                    return;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
            FindControl(By.Name("FilteringControl")).FindControl<TextBox>(criteria).Send(value);
        }

        public bool CheckItem(By criteria)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 15 && (!Exists(criteria)))
            {
                Click();
                Thread.Sleep(1000);
                if (Exists(criteria)) return true;
            }
            return Exists(criteria);
        }

        public void SelectItem(By criteria)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 30 && (!Exists(criteria)))
            {
                Click();
                Thread.Sleep(1000);
                try
                {
                    FindControl<Button>(criteria).Click();
                    return;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
            var button = FindControl<Button>(criteria);
            button.Click();
        }
        public bool SelectItemIfExists(By criteria)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 10 && (!Exists(criteria)))
            {
                Click();
                Thread.Sleep(1000);
                if (!Exists(criteria)) continue;
                FindControl<Button>(criteria).Click();
                return true;
            }
            return false;
        }
    }
}