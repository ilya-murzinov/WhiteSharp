if not exist Results mkdir Results
set name=WhiteSharp
set nunit="%cd%\..\..\Tools\NUnit 2.6.3\bin"
set report="%cd%\..\..\Tools\NUnit2Report.Console"
%nunit%\nunit-console.exe WhiteSharp\bin\Debug\%name%.dll /out:Results\%name%.txt
%report%\NUnit2Report.Console.exe --todir Results --out %name%.html --fileset=TestResult.xml --lang en
