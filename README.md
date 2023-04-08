# Carcassonne

Hello everyone. This is my project for one of my college subjects. Due to the fact that I am a huge fan of board games, I wanted to design a digital version of the Carcassonne game.

### Server side
The server I designed is based on ASP.NET Core technology using
* SignalR for one-to-one server-client communication
* EntityFramework to connect to the database

### Database
I put the database on Docker. But if you want to run a solution with cached database without Docker, all you have to do is change one line 
[here](Server/Server/Program.cs)

### Client
I did this part using React with a proxy server connecting backend and frontend.

Currently the project is under construction.