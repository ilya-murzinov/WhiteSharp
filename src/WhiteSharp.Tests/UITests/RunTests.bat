if not exist Results mkdir Results
set nunit="%cd%\..\..\..\Tools\NUnit 2.6.3\bin"
set report="%cd%\..\..\..\Tools\NUnit2Report.Console"
%nunit%\nunit-console.exe ..\bin\Debug\WhiteSharp.Tests.dll /out:Results\WhiteSharp.Tests.txt
%report%\NUnit2Report.Console.exe --todir Results --out WhiteSharp.Tests.html --fileset=TestResult.xml --lang ru
pause