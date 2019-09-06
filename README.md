# DataMiners_demo_scripts
This repository contains the main functionality scripts for the tech demo of Data Miners, an rts-management game. The elements present are the level builder, the pathfinding manager, the selection manager, the UI integration scripts, unit data, and a shader. A centralised distance calculator is also in the making.

### General level building and management:
TerrainBuilder_02.cs (primary level constructor script)

PixelClass.cs (used when scanning the level BMP image)

CameraReferenceSingleton.cs (tells where to find the camera prefabs, i.e. pull
them from “Assets/Resources” )

UI_ButtonPackAssociator (provides player units with a reference to their
respective action buttons).

### UI/GUI/Camera:
CameraControl.cs

UI_SetCamsOrder.cs (only relevant if one wishes to make screenspace - overlay UI
elements. This script automates their setup if needed)

GameScrollingBackground.cs

MinimapClickToGoThere.cs

UI_ActionsButtons.cs (contains listener to enable/disable action buttons
associated with a given player unit or building)

GreenGlowScript.cs (Unit visual effects)

RedGlowScript.cs

ParticleEffectScipt.cs

### Selection handling:
UnitSelectionManager.cs

MouseOverUnit.cs

HighlightableUnit.cs

RectangleDragSelection.cs (for units, GUI visualisation + bound selection)

HoverAndSelection.cs (for Blocks)

### Units general:
PlayerControlled.cs

Unit.cs (non Monobehaviour, contains vars and virtual functions)

UnitComponent.cs (Monobehaviour, with an internal reference to its own Unit.cs)

Unit_Miner.cs

StorageStatus.cs

NewEnemyAIScript.cs

EnemyAnimationScript.cs

EnemyShootingScipt.cs
### Pathfinding:
PathFinderRequestManager.cs

Pathfinding_a3.cs

MapNode.cs

HeapOptim.cs (allows to treat mapnodes as heap items, and make pathfinding
faster)

DestinationPoint.cs (associated to its own unique unit)
### Terrain info:
GroundTile.cs (non Monobehaviour)

GroundTileScript.cs (Mono with reference to GT)

LevelBlock.cs (for mineable blocks)

LevelBlockScript.cs

8 x “ xxxxxGround.cs ” (8 different terrain property scripts. All extend from
GroundTile.cs)

The following would only be checked for when interacted with (“if it contains….,
then…..”). They are attached to blocks, not tiles:

CleanMeImCorrupted.cs

Explodable.cs

Mineable.cs

Tough.cs

### Shader
CircleSelectorShader.shader (for the projected texture cast around
highlighted/selected friendly units).
