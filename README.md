[Project ruleset](https://pgbsnh19.github.io/dataatkomst/project4.html)


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
because I need to move tokens in Engine aswell as graphically on the board.

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
you live(study) and you learn.

After all this was done I started to refactor my Classes (Player and Token).
I made a Game class that consists of all the players so that I could easier save
and load a game state. The main thought here was to have everything I needed
in my classes so I could create my DBContext and add my initial migration.

Now I need to instantiate a new game, create 2-4 players depending on how many
are chosen to play. Add tokens for every player, place the tokens on their
starting positions on the game board.

**The game begins!**

Getting the pieces to move and keeping track of how long they've moved to be
able to also get them across the finish line instead of just going around in
circles on the board made this a bit trickier. There's one thing of just keeping
track of MovedSteps and another thing of doing this and placing the token on a
graphical board. But after some trial and error and && operators even this turned
out great.

This is where I eventually stopped checking if the player could move and then
checking what tokens that could move and just returned a List<Token> of the
tokens that can move. This made things easier for me and I didn't have to nest
quiet so many if statements.
  
Now we're getting somewhere. After getting tokens to move, that actually should
be able to move, we need to pass the turn to the next player unless the player
rolled a six but havn't rolled a six too many times.

Getting to the stage where we need to implement the finish line and goal.
This stage proved to be bug ridden, atleast when I did it. The most annoying
bug for me was that I could bump players back that were on the finish line
which shouldn't be possible. But ofcourse this was because I was just checking
if they were on the same Position on the GUI board and not if they were also
on the finish line. Some better planning would've solved this but I solved it
with an extra && operator to check if the token was on a finish line or not

## DBContext

**Problems saving and loading**
I realise that my solution to my global DBContext isn't a good solution but
after re-starting my project two times I just didnt want to do that again.
Reading on how to solve this issue I've learned alot about entity frameworks
and DBContext practices. I got my way to work but will do this alot more cleanly
in the future. Most likely by just saving when the player either presses a 
save button, loads and game, creates a new game or shuts the game down.
Now I save the game too much(if this is possible) and as mentioned above
my DBContext is a global variable.
