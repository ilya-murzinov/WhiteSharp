if not exist Results mkdir Results
set nunit="%cd%\..\..\..\Tools\NUnit 2.6.3\bin"
set report="%cd%\..\..\..\Tools\NUnit2Report.Console"
%nunit%\nunit-console.exe ..\bin\Debug\WhiteSharp.dll /out:Results\WhiteSharp.txt
%report%\NUnit2Report.Console.exe --todir Results --out WhiteSharp.html --fileset=TestResult.xml --lang ru
pause