WhiteSharp
==========

Our first test will be splitted up in 3 obvious steps:
 1. Find some window;
 2. Find a control inside your window;
 3. Do something with control.

Let's examine each step.

1. Find some window
-------------------
The simpliest way is to find a window by it's title. To do this, simply type
```csharp
Window window = new Window("some title");
```
There are another ways to find a window, for example, by **ProcessId** or even **Predicate\<AutomationElement\>**.

2. Find a control inside your window
-------------------
You can find a control by it's **AutomationId**, **Name**, **ClassName**, **ControlType** and any combination of this properties.

For examlpe,
```csharp
string automationId = "id";
string name = "name";
ControlType controlType = ControlType.Combobox;
var control = window.FindControl(By.AutomationId(automationId)
    .AndNameContains(name)
    .AndControlType(controlType));
```      
If you are lucky to have unique AutomationId, you can simplify this even more:
```csharp
var control = window.FindControl(id);
```
3. Do something with control
-------------------
Well, now you have your control, time to do something with it, for example, click it:
```csharp
control.Click();
```    
or send some text to it:
```csharp
control.Send("some text");
```    
or send a special key:
```csharp
control.Send(Keys.Enter);
```    
or select item (if this is a ComboBox):
```csharp
control.SelectItem("itemName");
```    
or select this control (if this is CheckBox):
```csharp
control.SetToggleState(ToggleState.On);
```

Thats it, the first test is ready!
----------------------------------
For further information see [full docs](index.md) and how to use this with [screen objects pattern](screenobjects.md).
