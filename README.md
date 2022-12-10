# LangBot.NET

![ecample](https://user-images.githubusercontent.com/12978622/206853292-89438ea8-335a-4747-902c-12e786473076.PNG)


This is a simple Telegram bot that sends a time when being restarted a new word with a usage example and a random picture.
It’s written in **.NET core 6.0** and uses 
+ Entity Framework
+ SQLite
+ Telegram API
 
**How to start your own bot?**    
This project could be easily compiled with VS Studio or with the bare dotnet SDK.
After the first start, 2 SQLite databases will be generated in the binary folder and you can edit the words database and add words you want to learn. 
In the binary folder also should be **token.token** file with the valid Telegram Bot token.

There should be a web server with images so the telegram client would be able to download them.
To restart bot in order to receive messages periodically you can set up a cron schedule or use any other scheduler.


