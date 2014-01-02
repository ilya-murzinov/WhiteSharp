WhiteExtension
==============

This is an extension for [TestStack.White UI automation library] (https://github.com/TestStack/White) designed to simplify test writing and make tests more readable.

**This is not a framework**, you can use it this any framework you like, such as [**NUnit**](https://github.com/nunit/nunit-framework) or **MSTest**.

**Warning!**
The main purpose of this project is to *simplify* automated test writing and reading, so it may lack some advanced features. But I believe I'll make it much better after a while.

Key features
------------

Key features of this extension are:

1. Method chaining;
2. No "waits" needed; 
3. Detailed log.

Qiuck start
-----------

1. Download [UI Automation Verify](http://uiautomationverify.codeplex.com/) to watch control's properties (or you can use any other similar application for this if you want);
2. Clone repository and build project into WhiteExtension.dll; 
3. Create new test project;
4. Add references to WhiteExtension and TestStack.White (see Libs);
5. Find desired window by title: <code>var window = new UIWindow("window_title");</code>
6. Find control for example by it's automationId and name: <code>var control = window.FindControl(By.AutomationId("automation_id").Name("name"));</code>
7. Do something with found control (click, send keys etc.): <code>control.Click().Send("something").Send("{Enter}");</code>
8. Well done, now you are ready to write your first automated test!

For futher study read the documentation.

Docs
----

View docs [here](http://murz42.viewdocs.io/WhiteExtension)
