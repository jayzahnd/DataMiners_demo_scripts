using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class LevelBlock {

    public enum DataBlockType { BASIC, TOUGH, UNBREAKABLE, CORRUPT};
    public DataBlockType dbType;

    TerrainBuilder_02 referenceTerrain;

    public int dBlockID;
    public int dBlock_X;
    public int dBlock_YorZ;

    public bool isBreakable;

    public int toughness;

    public GroundTileScript dBUnderlyingTile;


    public MeshRenderer dBlockRenderer;

    public SpriteRenderer dBSpriterndr;
    public HoverAndSelection dBHoverandSel;

    [SerializeField]
    protected Material dBlockCurrentMaterial;

    [SerializeField] protected Sprite dBlockSpriteMaterial;

    public virtual void LoadPropertiesDB()
    {
        dBlockID = dBUnderlyingTile.tileReference.tileID;
        dBlock_X = dBUnderlyingTile.nodeReference.lvlGrid_X;
        dBlock_YorZ = dBUnderlyingTile.nodeReference.lvlGrid_YorZ;
        dBUnderlyingTile.tileReference.isWalkable = false;
        dBUnderlyingTile.nodeReference.walkableNode = false; // don't forget to set to true upon issuing a mining action on it, so that it can use getneighbours.

        //Debug.Log("virtual root loaded");
    }

    protected void ComeMineMe(UnitComponent unit) { // should probs make a delegate. // Should probably move this to a manager script for this.

        if(unit == null) {
            return;
        }

        Vector3 selectedUnitPosition = unit.transform.position;

        List<MapNode> neighbourListUnder = referenceTerrain.GetNeighbours(dBUnderlyingTile.nodeReference);

        foreach (MapNode neighbour in neighbourListUnder) {

            if (!neighbour.walkableNode) {
                neighbourListUnder.Remove(neighbour);
                continue;
            }
            int xCoord = neighbour.lvlGrid_X;
            int zCoord = neighbour.lvlGrid_YorZ;
            Vector3 testCoords = new Vector3(xCoord, 0, zCoord);

            PathFinderRequestManager.RequestPath(new PathRequest(selectedUnitPosition, testCoords, PathCompare));
        }
    }

    public void PathCompare(Vector3[] testPath, bool found) {
        if (found) {

            
        }
    }
}


[Serializable]
public class BasicBlock: LevelBlock
{
    public BasicBlock(GroundTileScript _underlyingTile, bool interractable)
    {
        dbType = DataBlockType.BASIC;
        dBUnderlyingTile = _underlyingTile;
    }
    public override void LoadPropertiesDB()
    {
        base.LoadPropertiesDB();

        dBlockCurrentMaterial = Resources.Load<Material>("Materials/Blocks_Minable");
        dBlockRenderer.material = dBlockCurrentMaterial;
        dBlockSpriteMaterial = Resources.Load<Sprite>("Textures/TextureLightGreen");
        dBSpriterndr.sprite = dBlockSpriteMaterial;
        isBreakable = true;
        toughness = 1;

        //Debug.Log("override basic loaded");
    }
}


[Serializable]
public class ToughBlock : LevelBlock
{
    public ToughBlock(GroundTileScript _underlyingTile, bool interractable)
    {
        dbType = DataBlockType.TOUGH;
        dBUnderlyingTile = _underlyingTile;
    }
    public override void LoadPropertiesDB()
    {
        base.LoadPropertiesDB();

        dBlockCurrentMaterial = Resources.Load<Material>("Materials/Blocks_Minable_Tough");
        dBlockRenderer.material = dBlockCurrentMaterial;
        dBlockSpriteMaterial = Resources.Load<Sprite>("Textures/TextureLightGreen");
        dBSpriterndr.sprite = dBlockSpriteMaterial;
        isBreakable = true;
        toughness = 2;

        //Debug.Log("override tough loaded");
    }
}


[Serializable]
public class UnbreakableBlock: LevelBlock
{
    public UnbreakableBlock(GroundTileScript _underlyingTile, bool interractable)
    {
        dbType = DataBlockType.UNBREAKABLE;
        dBUnderlyingTile = _underlyingTile;
    }
    public override void LoadPropertiesDB()
    {
        base.LoadPropertiesDB();

        dBlockCurrentMaterial = Resources.Load<Material>("Materials/Blocks_Unbreakable");
        dBlockRenderer.material = dBlockCurrentMaterial;
        dBlockSpriteMaterial = null;
        dBSpriterndr.sprite = dBlockSpriteMaterial;
        isBreakable = false;
        toughness = 1024; // just in case

        //Debug.Log("override unbreakable loaded");
    }
}


[Serializable]
public class CorruptBlock : LevelBlock
{
    public CorruptBlock(GroundTileScript _underlyingTile, bool interractable)
    {
        dbType = DataBlockType.CORRUPT;
        dBUnderlyingTile = _underlyingTile;
    }
    public override void LoadPropertiesDB()
    {
        base.LoadPropertiesDB();

        dBlockCurrentMaterial = Resources.Load<Material>("Materials/Blocks_Minable_Corrupt");
        dBlockRenderer.material = dBlockCurrentMaterial;
        dBlockSpriteMaterial = Resources.Load<Sprite>("Textures/TextureArmsAndLegs");
        dBSpriterndr.sprite = dBlockSpriteMaterial;
        isBreakable = true;
        toughness = 4;

        //Debug.Log("override Corrupt loaded");
    }
}
