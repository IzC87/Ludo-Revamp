# User Stories

As a **player**, I want to be able to roll my own die, to make it feel like I am participating.

As a **player**, I want to automatically move a piece when I've rolled the die. But only if I can only move one piece.

As a **player**, I want to be able to choose what piece to move.

As a **user**, I want my games to save automatically.

As a **user**, I want to be able to play versus computers and/or real people.

As a **computer**, I want to automate everything so that the real people don't have to wait.

As a **user**, I want to be able to load my saved game(s).



# Project Documentation


## Reflection
I've re-started this project three times because of a couple of different reasons.
- I've thought of better ways to do things
- I'm not experienced enough to know how to think ahead effectively
- I'm not planning enough, especially when I am alone.


## First thoughts
- I immediatly decided to make a GUI for the game because I hate the console and
because I think it's easier to see what's going on in a real GUI. This let me
down a path where my WPF class and my GameEngine class Library had to communicate
because I need to move tokens in Engine aswell ass graphically on the board.

- Being able to play versus the computer was important to me aswell. Aswell as
automating the movements for the computer. This ended up being pretty complex
until I came up with the "obvious" solution to just return all the tokens that
can move for the player whose turn it is. The turn and movement of a player is
the main reason why I restarted the project because I made it unnecesarry 
complicated and the bugs started to creep up on me.


## Coding process
I first made the GUI to be able to move my pieces on the board Graphically. Made
the JSON files with all the X, Y coordinates for the GUI. Created the logic for
what happens when I press the GUI buttons. Then I made the methods to actually
move the pieces on the GUI. I started with all this because I wanted to see my
movements to be able to debug easier and faster. So most of these methods stay
about the same during my re-writes. Also starting this new course and learning
more about git has taught me that I shouldn't have restarted the project but
you live(ready study) and you learn.

After all this was done I started to refactor my Classes (Player and Token).
I made a Game class that consists of all the players so that I could easier save
and load a game state.
