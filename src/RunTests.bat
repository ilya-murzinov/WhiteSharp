if not exist Results mkdir Results
set nunit="%cd%\..\..\Tools\NUnit 2.6.3\bin"
set report="%cd%\..\..\Tools\NUnit2Report.Console"
%nunit%\nunit-console.exe WhiteSharp\bin\Debug\WhiteSharp.dll /out:Results\WhiteSharp.txt
%report%\NUnit2Report.Console.exe --todir Results --out WhiteSharp.html --fileset=WhiteSharp\bin\Debug\TestResult.xml --lang ru
pause