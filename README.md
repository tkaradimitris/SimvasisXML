# Introduction 
Convert rental agreements as export from ? to taxisnet format

It uses oledb to access data found in an excel file.
Download Access Redistribution from:
https://www.microsoft.com/en-us/download/details.aspx?id=54920

Change app.config to:
-set the correct VAT number of the company
-set the correct OleDb version to reflect the installed version on the target pc, eg:
	Provider=Microsoft.ACE.OLEDB.16.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;';
	
Remember to use a x86 or x64 version of the app, to match the installed office version