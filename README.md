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
can move for the player whose turn it is.
