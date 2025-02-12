# [Your Project Title Here]

## Team Members

List your team members here. Link to each member's individual github account.

| Member    | Github |
| -------- | ------- |
| Stella Huo  | https://github.com/Stellahhh    |
| Hanbei Zhou | https://github.com/HanbeiZhou     |
| Linda Fan    | https://github.com/yfan43    |

## Game Summary

A zombie-infection game where you will be a zombie or a human. If you are a zombie, try to infect more humans! If you are a human, try to avoid being infected!

## Genres

The broad category (or categories) your game will fall under. Examples include first-person shooter (FPS), real-time strategy (RTS), puzzle, rogue-like, etc.
**Our game category will fall under First-person Real-time strategy.**

## Inspiration

### [1. Cat-mouse game]

Cat-mouse game. An offline real-person game where players are assigned to a Cat or Mouse team. Cats need to catch the mouse and convert them to a cat. Every player will have a location tracker to display their location on the map.
<img src="cat_mouse.JPG" alt="Cat-mouse Image" width="500"/>

### [2. PUBG]

In our game, since would be a time limit, we will implement a similar mechanic to PUBG’s shrinking play area. We plan that every 15 minutes, one-fifth of the game map will be shut down, which force both zombies and humans to leave the zone. Any player that remains in a restricted area will be eliminated instantly. One thing to note is that the shut down area will be reopened after 15 minutes. 
<img src="PUBG.jpg" alt="PUBG Image" width="500"/>

## Gameplay

**General rule:**
- A paragraph or bulleted list describing how the player will interact with the game, and the key gameplay mechanics that you plan to have implemented in your finalized game. Also use this section to broadly describe the expected user interface and game-controls.
- In this game, players (10 - 50 players) will be assigned to a map containing trees and obstacles randomly. 
- At the beginning of the game, every player will have 10 min to explore the map. 
- At the end of the 10 min, the players will be assigned to 2 teams: a Zombie team or a Human team, in a 1:9 ratio. (e.g., if the game consists of 30 players, there will be 3 players in the Zombie team and 47 in the Human team).


**Zombie team**: The zombies’ goal is to catch and infect humans. The Zombie who infected (contacted) the most human players wins.
**Human team**: The humans’ goal is to avoid being infected. Humans will join the Zombie team if they are contacted by the zombie. The human who didn’t get infected at the end of the game (e.g., 2 hours) wins. If all humans got infected, the last human who got infected wins.
Every player can access the map, which contains the rough location of every player as well as their identities.

**Movement**:
Users will move with their mouse and keyboard. 
- The user will use mouse movement to change perspective. (mouse moving left means looking towards the left)
- The user will use the keyboard (WASD) to move given the current perspective.
- The user will use the keyboard (space bar) to jump

**Special adjustment as the game progresses**:
- Zombies will have a hungriness bar that will be filled in 15 min. As the zombies get more hungry, their moving speed increases. Catching a human will empty the hungriness bar. If the hungriness bar is filled, the zombie dies.
- Every 15 min, randomly ⅕ of the whole map will be temporally shut down for 15 min. Players will have to get out of that area. There will be a warning 15 min prior to the shutdown.

## Development Plan

### Project Checkpoint 1-2: Basic Mechanics and Scripting (Ch 5-9)

Sketch out a rough idea of what parts of your game you will implement for the next submission, Project Checkpoint 1-2: Basic Mechanics and Scripting involving Unity textbook Chapters 5 through 9. You will come back to update this for each submission based on which things you've accomplished and which need to be prioritized next. This will help you practice thinking ahead as well as reflecting on the progress you've made throughout the semester.
Implement the map
Design the basic elements, such as trees, houses, water area, pit.
