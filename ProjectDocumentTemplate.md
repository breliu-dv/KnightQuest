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

**Document what game states and game data you managed and what design patterns you used to complete your task.**

To implement the game logic systems of our game, first, the AI for the green, blue, and red slimes were implemented according to the specifications that we defined for each of the types of slimes. For the green slime, I created green slime controller script where the green slime will move back and forth between two position values in the X axis, while ensuring that velocity and health values are managed properly within the code as the [player attacks the green slime](https://github.com/breliu-dv/KnightQuest/blob/24a773359ad45dea47ebe8d45a6a6e47d97a9296/Assets/Scripts/GreenSlimeController.cs#L91) and also when the [green slime attacks the player](https://github.com/breliu-dv/KnightQuest/blob/24a773359ad45dea47ebe8d45a6a6e47d97a9296/Assets/Scripts/GreenSlimeController.cs#L42). For the blue slime, the underlying AI logic had to be implemented in an more intricate manner as the slime has to not only chase the player but also ensure that it does not move outside of a predefined boundary, get stuck behind an obstacle or wall, as well as only jump when it is supposed to jump. For instance, when the knight gets within the chasing boudaries of the blue slime, the blue slime will start to chase the knight and also jump at random intervals during the chase. When the slime reaches a ledge, it will also jump to avoid falling off as well. These logical systems of the behavior of the slime are defined in several sets of if conditions [within the update function of the blue slime](https://github.com/breliu-dv/KnightQuest/blob/33c2e135175af8fdffb68976f82f65d209a5eaae/Assets/Scripts/BlueSlimeController.cs#L107). This required me to effectively test and note the different ways and edge cases that the slime would behavior depending on whether it hits an object, encounters the player, among other things so that the behavior of the slime is more realistic, exciting and also responsive in a life like manner. On the other hand, the behavior for the red slime is different from the blue slime, as it chases the player and always jumps regardless of the situation that it encounters.

Besides the slimes, I also managed other important game systems and the underlying logical implementation of those features. Falling spikes, moving platforms using [MoveTowards between two defined locations](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/WaypointFollower.cs#L24), disappearing platforms, a lake full of poisonous acid, as well as the varied terrian all add on the difficulty and excitement during game play. While moving and disappearing platforms can change the player's position (such as falling off of a disappearing platform or moving along with a moving platform), [spikes](https://github.com/breliu-dv/KnightQuest/blob/8a73df45f301c137995086807533516eade383c3/Assets/Scripts/FallingSpikeController.cs#L15), [acid](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/AcidPool.cs#L23), and enemies all alter the player's health value when the enemies [perform damage to the knight](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/KnightController.cs#L358). On the other hand, the player's health can also be boosted with a treasure chest, which also alters the player's health value by [setting the knight's health value](https://github.com/breliu-dv/KnightQuest/blob/86de2d382412197e95ffddd3721f704964c89f9d/Assets/Scripts/TreasureHitboxControl.cs#L23). Most notably, the moving platforms are implemented using [a coroutine design pattern](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/DisappearPlatform.cs#L50) to effectively represent the different states needed to control the behavior of such platform.

[The player's attack is implemented](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/KnightController.cs#L489) by calling the take damage function of the slimes, therefore allowing for an effective ability for the slimes to know that they have been damaged by the kight's attacks. The player must be grounded to perform certain actions, however, and so the [logic for detecting if the player is on the ground](https://github.com/breliu-dv/KnightQuest/blob/21fa9164ae17b09ad8876fa14673267ee1e3ae5e/Assets/Scripts/KnightController.cs#L436) is implemented using the draw ray function, which will allow the movement functions to function properly with the state of the grounded boolean.

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
