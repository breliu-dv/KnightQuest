# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**


**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

## Animation and Visuals

**List your assets including their sources and licenses.**

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

## Game Logic

To implement the core game logic systems of our game, which is our AI for the slime enemies, first, the AI for the green, blue, and red slimes were implemented according to the specifications that we defined for each of the types of slimes. For the green slime, I created green slime controller script where the green slime will move back and forth between two position values in the X axis, while ensuring that velocity and health values are managed properly within the code as the [player attacks the green slime](https://github.com/breliu-dv/KnightQuest/blob/24a773359ad45dea47ebe8d45a6a6e47d97a9296/Assets/Scripts/GreenSlimeController.cs#L91) and also when the [green slime attacks the player](https://github.com/breliu-dv/KnightQuest/blob/24a773359ad45dea47ebe8d45a6a6e47d97a9296/Assets/Scripts/GreenSlimeController.cs#L42). For the blue slime, the underlying AI logic had to be implemented in an more intricate manner as the slime has to not only [chase the player](https://github.com/breliu-dv/KnightQuest/blob/4a70f0b0d863c637548bb8ee8b829f9192124834/Assets/Scripts/BlueSlimeController.cs#L104) but also ensure that it does not move outside of a predefined boundary, [get stuck behind an obstacle or wall](https://github.com/breliu-dv/KnightQuest/blob/4a70f0b0d863c637548bb8ee8b829f9192124834/Assets/Scripts/BlueSlimeController.cs#L196), as well as [only jump when it is supposed to jump](https://github.com/breliu-dv/KnightQuest/blob/4a70f0b0d863c637548bb8ee8b829f9192124834/Assets/Scripts/BlueSlimeController.cs#L221). 

For instance, when the knight gets within the chasing boundaries of the blue slime, the blue slime will start to chase the knight and also jump at random intervals during the chase. When the slime reaches a ledge, it will also jump to avoid falling off as well. These logical systems of the behavior of the slime are defined in several sets of if conditions [within the update function of the blue slime](https://github.com/breliu-dv/KnightQuest/blob/33c2e135175af8fdffb68976f82f65d209a5eaae/Assets/Scripts/BlueSlimeController.cs#L107). This required me to effectively test and note the different ways and edge cases that the slime would behavior depending on whether it hits an object, encounters the player, among other things so that the behavior of the slime is more realistic, exciting and also responsive in a life like manner. On the other hand, the behavior for the red slime is very different from the blue slime, as it chases the player and always jumps regardless of the situation that it encounters which forces the player to adjust its attack strategy from the blue and green slimes.

Besides the slimes, I also managed other important game systems and the underlying logical implementation of those features. Falling spikes, moving platforms using [MoveTowards between two defined locations](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/WaypointFollower.cs#L24), disappearing platforms, a lake full of poisonous acid, as well as the varied terrain all add on the difficulty and excitement during game play. While moving and disappearing platforms can change the player's position (such as falling off of a disappearing platform or moving along with a moving platform), [spikes](https://github.com/breliu-dv/KnightQuest/blob/8a73df45f301c137995086807533516eade383c3/Assets/Scripts/FallingSpikeController.cs#L15), [acid](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/AcidPool.cs#L23), and enemies all alter the player's health value when the enemies [perform damage to the knight](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/KnightController.cs#L358). On the other hand, the player's health can also be boosted with a treasure chest, which also alters the player's health value by [setting the knight's health value](https://github.com/breliu-dv/KnightQuest/blob/86de2d382412197e95ffddd3721f704964c89f9d/Assets/Scripts/TreasureHitboxControl.cs#L23). Most notably, the moving platforms are implemented using [a coroutine design pattern](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/DisappearPlatform.cs#L50) to effectively represent the different states needed to control the behavior of such platform. Coroutines are useful here as this allows for the efficient modularization of the different stages of a disappearing platform and allows for good reusability of the different routines depending on the situations encountered during game play.

[The player's attack is implemented](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/KnightController.cs#L489) by calling the take damage function of the slimes, therefore allowing for an effective ability for the slimes to know that they have been damaged by the knight's attacks. The player must be grounded to perform certain actions, however, and so the [logic for detecting if the player is on the ground](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/KnightController.cs#L436) is implemented using the draw ray function, which will allow the movement functions to function properly with the state of the grounded boolean.

Last but not least, another important design pattern that was used is the observer pattern of which was modified and adopted from exercise 3. This effectively allows the slimes to re-spawn after they die during the hard difficulty of the game in a modular and easily maintainable manner. When the slimes die, aside from the usual logic to handle the disabling of the slime, they also automatically [subscribe to the re-spawn group](https://github.com/breliu-dv/KnightQuest/blob/4a70f0b0d863c637548bb8ee8b829f9192124834/Assets/Scripts/BlueSlimeController.cs#L248) and when the player plays the hard difficulty, then the [re-spawning of the enemies gets triggered](https://github.com/breliu-dv/KnightQuest/blob/4a70f0b0d863c637548bb8ee8b829f9192124834/Assets/Scripts/KnightController.cs#L393) when the knight re-spawns from its own death.

# Sub-Roles

## Audio

**List your assets including their sources and licenses.**

**Describe the implementation of your audio system.**

**Document the sound style.** 

## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**

In order to enhance the game feel user experience and ensure that the game feel is the most ideal, I added and tuned the various parameters of the slimes and their respective behaviors for each and every individual slime present in the game, locations, appearances, and behavior of the various obstacles as well as the platforms that move or disappear.

For the slimes, I tuned the slime controller parameters for the slimes in order to ensure that the slimes respond promptly and smoothly when it is following the knight, while also allowing for a balanced difficulty so that some slimes are faster than others, jump more, have longer patrol paths, chase the player more aggressively in terms of movement speed, as well as the distance that the slime chases the player before it gives up. The main goal of this is to ensure that the slimes cause the player to feel that the challenge of encountering the slimes, and adopt a strategy for surviving the encounter (either by fighting the slime or escaping from the slime). 

Animation controller and their parameters tuned for each slime:

![Alt text](https://github.com/breliu-dv/KnightQuest/blob/main/documentImages/animationControl.png?raw=true)

At the same time, because each slime has it's own unique properties and behavior (no two slimes have the same set of values in the fields of the unity inspector), the playability of the game is extended due to the uniqueness involved rather than simply repetition during each encounter with the enemies. In addition, the player will not be exhausted with the same slimes that all move at a same speed, instead, the player can essentially "take a break" between slimes of increased difficulty and also small stretches of the game level that have less or even no slimes to deal with. This further enhances the mid level game feel experience as the player will not be completely burned out with fighting enemies before the player reaches the end of the level (where the real challenge with fighting the slimes come!).

Controllers and their parameters that I tuned for the green, blue, and red slimes, respectively:

![](https://raw.githubusercontent.com/breliu-dv/KnightQuest/main/documentImages/greenSlimeControl.png)

![](https://github.com/breliu-dv/KnightQuest/blob/main/documentImages/blueSlimeControl.png?raw=true)

![](https://github.com/breliu-dv/KnightQuest/blob/main/documentImages/redSlimeControl.png?raw=true)

In addition, the starting health and damage levels of both the slime and the knight are tuned such that the gameplay is coherently tuned between the slimes damaging the player and the player damaging the slime depending on the difficulty level. The player will have a slightly easier time with the enemies in the easy level while normal and hard difficulty levels will require the player to devise a smarter and potentially more conservative attack or encounter strategy in order to preserve the knight's health. Thus, the tuning of the different parameters that affects the difficulty of the game is vital for ensuring that this game can be enjoyable and fun for gamers of different abilities and backgrounds.

Last but not least, I also added and adjusted the different parameters pertaining to the moving and disappearing platforms so that they provide a fair amount of challenge to the player to respond promptly to the control inputs, while ensuring that the transitions of the disappearing platforms are gradual. This is important in ensuring that the user can visually see the environment around them and devise a successful movement strategy and avoid falling onto the deadly traps. Most importantly, as the environment provides contextual clues to the player, the moving platforms and slimes all have to move at a smooth, reasonable speed. This is to ensure that the player has the sufficient reaction time that is needed to effectively time the strategically sound knight movements as well as respond to the jumps needed to navigate through the moving platforms, and knight movements needed to launch an effective attack against the slime enemies.

The parameters that I tuned for the moving platforms:

![](https://github.com/breliu-dv/KnightQuest/blob/main/documentImages/disappearPlatform.png?raw=true)

And for the disappearing platforms:

![](https://github.com/breliu-dv/KnightQuest/blob/main/documentImages/movingPlatform.png?raw=true)