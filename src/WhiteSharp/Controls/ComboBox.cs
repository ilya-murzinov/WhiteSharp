using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using TestStack.White.UIItems.Actions;
using WhiteSharp.Extensions;

namespace WhiteSharp.Controls
{
    public class ComboBox : Control
    {
        public ComboBox(AutomationElement automationElement, IControlContainer window, By searchCriteria, int index)
            : base(automationElement, window, searchCriteria, index)
        {
        }

        public Control TrySelectItem(string name)
        {
            if (name == null)
                return this;

            WaitForEnabled();

            object o;

            var allItems = GetAllItemsText();
            var allItemsNumber = allItems.Count;
            if (allItemsNumber == 0)
            {
                Thread.Sleep(1000);
                WaitForEnabled();
                allItems = GetAllItemsText();
                allItemsNumber = allItems.Count;
            }
            var number = new Random().Next(0, allItemsNumber);
            if (!allItems.Contains(name))
            {
                Logging.Info(String.Format("Вариант по имени '{0}' не найден", name));
                var sb = new StringBuilder();
                name = allItems[number];
                name = name.Substring(Math.Max(name.IndexOf(':') + 2, 0));
                Logging.Info(String.Format("Вместо него берем вариант '{0}', выбранный произвольно", name));
            }

            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var valuePattern = (ValuePattern)o;
                valuePattern.SetValue(name);
            }
            else
            {
                var comboBox = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement,
                    new NullActionListener());
                comboBox.Select(name);
            }
            Send(Keys.Tab);
            Logging.ItemSelected(name, SearchCriteria.Identifiers);
            return this;
        }

        public Control SelectItem(string name, bool doLog = true)
        {
            if (name == null)
                return this;

            WaitForEnabled();
            Thread.Sleep(500);

            object o;

            if (AutomationElement.TryGetCurrentPattern(ValuePattern.Pattern, out o))
            {
                var valuePattern = (ValuePattern)o;
                valuePattern.SetValue(name);
            }
            else
            {
                var comboBox = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement,
                    new NullActionListener());
                comboBox.Select(name);
            }

            if (doLog)
                Logging.ItemSelected(name, SearchCriteria.Identifiers);
            return this;
        }

        public Control SelectExactItem(string name)
        {
            var start = DateTime.Now;
            var text = String.Empty;
            while ((DateTime.Now - start).TotalSeconds < 20 && (!text.Equals(name)))
            {
                try
                {
                    Click().Wait(500);
                    var control = FindControl<Button>(By.Name(name).AndEnabled());
                    text = control.GetName();
                    control.Click();
                }
                catch { Thread.Sleep(100); }
            }
            return this;
        }

        public Control SelectItemByClick(string name)
        {
            if (name == null)
                return this;
            var currentText = "";
            WaitForEnabled();
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 15 && (!currentText.Equals(name)))
            {
                try
                {
                    try { currentText = GetText(); }
                    catch (Exception)
                    {
                        Thread.Sleep(100);
                    }
                    Click().Wait(500);
                    FindControl<Button>(By.Name(name)).Click();
                    Logging.ItemSelected(name, SearchCriteria.Identifiers);
                }
                catch { Thread.Sleep(100); }
            }
            return this;
        }

        public Control SelectItem(int index)
        {
            WaitForEnabled();

            var combobox = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, null);
            try
            {
                combobox.Select(index);
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }
            Logging.ItemSelected(index.ToString(CultureInfo.InvariantCulture), SearchCriteria.Identifiers);
            return this;
        }

        public new Control Click()
        {
            ClickAnyway();
            Logging.Click(SearchCriteria.Identifiers);
            return this;
        }

        /// <summary>
        /// Selects random value from combobox.
        /// </summary>
        public Control SelectRandomItem()
        {
            WaitForEnabled();
            Click();
            var allItems = GetAllItems();
            var allItemsNumber = allItems.Count;
            Logging.Info("Выбираем произвольный варинат из комбобокса, всего вариантов: " + allItemsNumber);
            if (allItemsNumber == 0) throw new Exception("В комбобоксе не найдены варианты.");
            var number = new Random().Next(0, allItemsNumber);
            Logging.Info("Выбираем номер: " + number + ", текст выбираемого варианта = " + allItems[number]
                             + ", текущий текст = " + GetSelectedItemText());
            SelectItemByClick(allItems[number]);
            Logging.Info("Был выбиран вариант: " + allItems[number]
                                + ", текущий текст в комбобоксе = " + GetSelectedItemText());
            return this;
        }

        /// <summary>
        /// Get all items names from combobox.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllItems()
        {
            var c = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, new NullActionListener());

            try
            {
                var e = (ExpandCollapsePattern)AutomationElement.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                var s = (ScrollPattern)AutomationElement.GetCurrentPattern(ScrollPattern.Pattern);

                e.Expand();
                while (s.Current.VerticalScrollPercent < 100)
                {
                    s.ScrollVertical(ScrollAmount.LargeIncrement);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(100);
            }

            return c.Items.ToList().Select(x => x.Name).ToList();
        }

        public List<string> GetAllItemsText()
        {
            var c = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, new NullActionListener());

            try
            {
                var e = (ExpandCollapsePattern)AutomationElement.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                var s = (ScrollPattern)AutomationElement.GetCurrentPattern(ScrollPattern.Pattern);

                e.Expand();
                while (s.Current.VerticalScrollPercent < 100)
                {
                    s.ScrollVertical(ScrollAmount.LargeIncrement);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(100);
            }

            return c.Items.ToList().Select(x => x.Text).ToList();
        }

        public string GetSelectedItemText()
        {
            var c = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, new NullActionListener());
            return c.SelectedItemText;
        }

        public string GetSelectedItemName()
        {
            var c = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, new NullActionListener());
            return c.Name;
        }

        public string GetAutomationElementText()
        {
            var c = new TestStack.White.UIItems.ListBoxItems.ComboBox(AutomationElement, new NullActionListener());
            return c.AutomationElement.GetText();
        }
    }
}