# DataMiners_demo_scripts
This repository contains the main functionality scripts for the tech demo of Data Miners, an rts-management game where the player controls little robots trying to clean up a computer's circuit board after a virus infection. The project was co-developped with my colleague Luke Kiernan, who worked on the art assets, animation, minimap setup, and provided the initial enemy AI.

The elements present are the level builder, the pathfinding manager, the selection manager, the UI integration scripts, unit data, and a shader. A centralised distance calculator is also in the making.

## General level building and management scripts:
* TerrainBuilder_02.cs *(Primary level constructor script)*

* PixelClass.cs *(Used when scanning the level BMP image)*

* CameraReferenceSingleton.cs *(Tells where to find the camera prefabs, i.e. pull them from “Assets/Resources” )*

* UI_ButtonPackAssociator *(Provides player units with a reference to their respective UI action buttons)*

## UI/GUI/Camera scripts:
* CameraControl.cs

* UI_SetCamsOrder.cs *(Only relevant if one wishes to make screenspace - overlay UI elements. This script automates their setup if needed)*

* GameScrollingBackground.cs

* MinimapClickToGoThere.cs

* UI_ActionsButtons.cs *(Contains listener to enable/disable action buttons associated with a given player unit or building)*

* GreenGlowScript.cs *(Unit visual effects)*

* RedGlowScript.cs

* ParticleEffectScipt.cs

## Selection handling scripts:
* UnitSelectionManager.cs

* MouseOverUnit.cs

* HighlightableUnit.cs

* RectangleDragSelection.cs *(For units, GUI visualisation + bound selection)*

* HoverAndSelection.cs *(For Blocks)*

## Units general scripts:
* PlayerControlled.cs

* Unit.cs *(Non Monobehaviour, contains vars and virtual functions)*

* UnitComponent.cs *(Monobehaviour, with an internal reference to its own Unit.cs)*

* Unit_Miner.cs *(Exptends from Unit, holds miner-specific data)*

* StorageStatus.cs

* NewEnemyAIScript.cs *(This is mostly for experimenting now. For composition puposes, it should later be broken down into smaller, reusable parts)*

* EnemyAnimationScript.cs

* EnemyShootingScipt.cs
## Pathfinding scripts:
* PathFinderRequestManager.cs

* Pathfinding_a3.cs

* MapNode.cs *(node object)*

* HeapOptim.cs *(Allows to treat mapnodes as heap items, and make pathfinding faster)*

* DestinationPoint.cs *(Associated to its own unique unit. Its puprose is to "reserve" the player unit's destination coord by physically occupying it while said unit is en route. This prevents group selection from converging towards the same spot)*
## Terrain info scripts:
* GroundTile.cs *(Non Monobehaviour)*

* GroundTileScript.cs *(Mono with reference to GT)*

* LevelBlock.cs *(For mineable blocks)*

* LevelBlockScript.cs

* 8 x “ xxxxxGround.cs ” *(8 different terrain property scripts. All extend from "GroundTile.cs")*

## Block info scripts: 
***Note: The following would only be checked for when interacted with (“if it contains…, then…..”). They are attached to blocks, not tiles, and should not have any function inside them:***

* CleanMeImCorrupted.cs

* Explodable.cs

* Mineable.cs

* Tough.cs

## Shader:
CircleSelectorShader.shader *(for the projected texture cast around highlighted/selected friendly units)*
