Using Screen Objects with WhiteSharp
==============================

Screen Objects pattern is a very useful structural test pattern, similar to Page Objects, which is 
popular in web UI testing. Screen Objects is basically same pattern, but it is used for writing UI tests for desktop applications.
The main idea of this technique is creating classes for each window you going to use and wrap interactions with it's items in methods of theese classes. Thus you can use same methods in multiple tests and avoid writing same code again and again. Also, methods can be very easily combined together in tests.