using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TerrainBuilder_02 : MonoBehaviour {

    #region VARIABLES

    public Texture2D rawLevelImage;   // NOTE: ONLY USE BITMAP IMAGE OR IT WON'T WORK!!!!!!
    Texture2D levelImage;

    [SerializeField] int mapLengthColumnsX; // Both of these are to be used to determine the characteristics of our 2D array, aka grid size .
    [SerializeField] int mapWidthRowsZ;     //

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    public static int displace = 10;  // setting default to 10, because the default plane object in unity is 5x5 units.

    [SerializeField]
    public List<GroundTileScript> groundCells = new List<GroundTileScript>();  //   DON'T MAKE THIS STATIC!!! If you exit to the main menu, then reload the game scene, 
                                                                               //   the old list will still exist, and will add the new cells to create an invalid level.
    public GameObject mapHolder;
    public GameObject dBlockHolder;

    
    public GameObject cameraHolderPrefab; // original position relative to world central coords xyz(0,0,0) is xyz(-10,18,-10).
    public Vector3 cameraOffset = new Vector3(-10, 18, -10);

    public Camera minimapCam;

    public List<GroundTileScript> baseTilesList = new List<GroundTileScript>();
    public GameObject mainBaseBuildingPrefab;
    //List<GameObject> objectiveTiles = new List<GameObject>(); // might make it into a list of lists for each objective in a level.

    private UI_ButtonPackAssociator ui_ButtonPackAssociatorLocal;

    public LayerMask unwalkableNode;

    public GameObject destinationPt;
    public GameObject player_inf_Pool;
    public static playerUnitStruct[] player_inf_Array;
    public GameObject player_inf_prefab;
    public int popcap_inf;  // Population cap for unit pool/level. The represents the maximum amount of the CHEAPEST unit that may be active at one time in the level. 
                            // Default here is the miner, which is worth 1.

    public GameObject enemy_Pool;
    public static enemyUnitStruct[] enemy_inf_Array;
    public GameObject enemy_inf_Prefab;
    public int enemy_popcap;
    
    public GameObject cameraControl;

    public TileType[] traversableTiles;
    LayerMask traversableMask;
    Dictionary<int, int> traversableTilesDictionary= new Dictionary<int,int>();

    [SerializeField]
    MapNode[,] groundNodeGrid;

    Vector3 gridCentrePos;
    Vector2 mapSize;
    //public List<MapNode> unitPath;


    Color32 _black = new Color32(0, 0, 0, 255);             // Impassable
    Color32 _red = new Color32(255, 0, 0, 255);             // Hazard
    Color32 _redDark = new Color32(128, 0, 0, 255);         // Hazard WITH BLOCK
    Color32 _green = new Color32(0, 128, 0, 255);           // Clear
    Color32 _greenTeal = new Color32(0, 128, 128, 255);     // Clear WITH BLOCK
    Color32 _blue = new Color32(0, 0, 255, 255);            // Objective
    Color32 _grey = new Color32(128, 128, 128, 255);        // Rough
    Color32 _greyDark = new Color32(64, 64, 64, 255);       // Rough WITH BLOCK
    Color32 _magenta = new Color32(255, 0, 255, 255);       // Base

    Color32[] pix;
    [SerializeField]
    List<PixelClass> pixelList = new List<PixelClass>();

    public static bool color32IsEqual(Color32 aCol, Color32 aRef) {
        return aCol.r == aRef.r && aCol.g == aRef.g && aCol.b == aRef.b; //&& aCol.a == aRef.a  // don't need alpha compare
    }

    UnitPositionTrackerManager unitPTM;


    //public Material tile_impassable;    // Impassable
    //public Material tile_hazard;        // Hazard
    //public Material tile_default;       // Clear
    //public Material tile_objective;     // Objective
    //public Material tile_rough;         // Rough
    //public Material tile_basebuilding;  // Building/Base
    //public Material tile_circuit;       // Circuit path


    //[SerializeField]
    //public List<Material> tileMaterialsMaster;

    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------
    //
    //                                      !!!!! START AND UPDATERS !!!!!
    //
    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    // Use this for initialization

    void Start () {
        //InitialiseTileMaterials();

        CameraReferenceSingleton.Instance.InitialiseCam();
                
        AssignMasksToTileTypes();

        levelImage = convertToExploitableTexture(rawLevelImage);
        RetreiveLevelLayout(levelImage);
        BuildMapGrid(mapLengthColumnsX, mapWidthRowsZ, tilePrefab, mapHolder);
        SetupMinimapCamera();
        SetLevelTerrain(pixelList, groundCells);                                    // First pass, for the flat ground.
        SetLevelBlocks(groundCells, blockPrefab, dBlockHolder);                     // Second pass, for the minable/non-minable blocks.

        ui_ButtonPackAssociatorLocal = GetComponent<UI_ButtonPackAssociator>();     // Links player units and buildings to their respective UI buttons.
        unitPTM = GetComponent<UnitPositionTrackerManager>();                       // Initialises the Position Tracking Manager (PTM)

        SetupStartingArea();                                                        // Place base, centres camera holder on it and provides camera references for other objects!!!
                                                                                    // Also sends order to build UI to the ui building script.
        BuildUnitPools(popcap_inf, enemy_popcap);

        //PlaceBlocks()
	}
	
    	

    #region (DEPRECATED STUFF, KEPT FOR REFERENCE)
    //public void InitialiseTileMaterials() {

    //    tileMaterialsMaster = new List<Material> {
    //        tile_impassable,
    //        tile_hazard,
    //        tile_default,
    //        tile_objective,
    //        tile_rough,
    //        tile_basebuilding,
    //        tile_circuit };
    //}

    //void DefineTraversableArray() {
    //    /* Layers for each tile type and their associated binary value:
    //        9  : Clear/Objective/Base        512
    //        10 : FixedTerrain/Impassable    1024
    //        11 : Rough                      2048
    //        12 : Hazard                     4096
    //        13 : Circuit                    8192
    //     */

    //    traversableTiles = new TileType[5]; // 4 basics + circuit terrain.

    //    traversableTiles[0].terrainTileDistanceModifier = 0;   // Clear/Objective/Base
    //    traversableTiles[1].terrainTileDistanceModifier = 1000;   // FixedTerrain/Impassable
    //    traversableTiles[2].terrainTileDistanceModifier = 30;   // Rough
    //    traversableTiles[3].terrainTileDistanceModifier = 61;   // Hazard
    //    traversableTiles[4].terrainTileDistanceModifier = 0;   // Circuit
    //}
        
    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------
    //
    //                                      !!!!! MEAT AND POTATOES BELOW !!!!!
    //
    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------
    
    
    void AssignMasksToTileTypes() {        
        foreach (TileType region in traversableTiles) {

            traversableMask.value = traversableMask | region.terrainTileLayerMask.value; // can be simplified as "traversableMask.value |= region.terrainTileLayerMask.value;"
            traversableTilesDictionary.Add((int)Mathf.Log(region.terrainTileLayerMask.value, 2), region.terrainTileDistanceModifier);
        }
    }

    public int TotalMapSize {                           // This number is used by the main pathfinding script, do not remove it.
        get {
            return mapLengthColumnsX * mapWidthRowsZ;
        }
    }

    #region 1) READ BITMAP IMAGE
    Texture2D convertToExploitableTexture(Texture2D source) {            // creates a readable image from a raw source without editor read/write setting needed: https://stackoverflow.com/questions/44733841/how-to-make-texture2d-readable-via-script
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }



    void RetreiveLevelLayout(Texture2D levelMap) {

        // note: it seems that the image is analysed from bottom/left to top/right.
        

        mapLengthColumnsX = levelMap.height;
        mapWidthRowsZ = levelMap.width;

        pix = levelMap.GetPixels32();
        //Debug.Log("image pixel count: " +pix.Length);
        
        foreach (Color32 p in pix) {

            PixelClass pixelItem = new PixelClass();

            if (color32IsEqual(p, _black)) {         // BLACK   1

                pixelItem.colourCompare = PixelClass.pixelReader.BLACK;
                pixelItem.pixelColor32 = _black;
                //Debug.Log("BLACK pixel");
            }
            else if (color32IsEqual(p, _red)) {     // RED  2

                pixelItem.colourCompare = PixelClass.pixelReader.RED;
                pixelItem.pixelColor32 = _red;
                //Debug.Log("RED pixel");
            }
            else if (color32IsEqual(p, _green)) {    // GREEN (dark)    3

                pixelItem.colourCompare = PixelClass.pixelReader.GREEN;
                pixelItem.pixelColor32 = _green;
                //Debug.Log("GREEN pixel");
            }
            else if (color32IsEqual(p, _blue)) {    // BLUE     4

                pixelItem.colourCompare = PixelClass.pixelReader.BLUE;
                pixelItem.pixelColor32 = _blue;
                //Debug.Log("BLUE pixel");
            }
            else if (color32IsEqual(p, _grey)) {    // GREY     5

                pixelItem.colourCompare = PixelClass.pixelReader.GREY;
                pixelItem.pixelColor32 = _grey;
                //Debug.Log("GREY pixel");
            }
            else if (color32IsEqual(p, _magenta)) {    // MAGENTA   6

                pixelItem.colourCompare = PixelClass.pixelReader.MAGENTA;
                pixelItem.pixelColor32 = _magenta;
                //Debug.Log("MAGENTA pixel");
            }
            else if (color32IsEqual(p, _redDark))      // DARK_RED  7
            {    

                pixelItem.colourCompare = PixelClass.pixelReader.DARK_RED;
                pixelItem.pixelColor32 = _redDark;
                //Debug.Log("DARK_RED pixel");
            }
            else if (color32IsEqual(p, _greenTeal))       // TEAL_GREEN     8
            {    

                pixelItem.colourCompare = PixelClass.pixelReader.TEAL_GREEN;
                pixelItem.pixelColor32 = _greenTeal;
                //Debug.Log("TEAL_GREEN pixel");
            }
            else if (color32IsEqual(p, _greyDark))       // DARK_GREY       9
            {    

                pixelItem.colourCompare = PixelClass.pixelReader.DARK_GREY;
                pixelItem.pixelColor32 = _greyDark;
                //Debug.Log("DARK_GREY pixel");
            }
            else {
                Debug.Log("Kek, you missed that one: " + p);
            }

            pixelItem.pixelIndex = pixelList.Count;
            pixelItem.name = "Pixel n° " + pixelItem.pixelIndex;
            pixelList.Add(pixelItem);

        }

        //Debug.Log("List of pixel items count: " + pixelList.Count);  // Should be the same as "pix.Length"
    }
    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    #region 2) BUILD GRID AND SET TERRAIN
    void BuildMapGrid(int length, int width, GameObject cellObject, GameObject parentContainer) {

        //int cellRadius = displace / 2;

        groundNodeGrid = new MapNode[length, width];   // note that length and width here refer to a number of map tiles.
        Transform parentTransform = parentContainer.transform;

        for (int z = 0; z < width; z++) {
            for (int x = 0; x < length; x++) {

                Vector3 position = new Vector3((x+0.5f) * displace, 0, (z+0.5f) * displace);
                GameObject gridTile = GameObject.Instantiate(cellObject, position, Quaternion.identity, parentTransform);
                             
                GroundTileScript tileComponent = gridTile.AddComponent<GroundTileScript>();

                int moveMentModPF = 0;

                // need to then cast ray to set layer of tile in setLevelTerrain function. 0 is default.

                tileComponent.nodeReference = new MapNode(true, position, x, z, moveMentModPF);  // Creating corresponding node point.
                groundNodeGrid[x, z] = tileComponent.nodeReference;
                
                groundCells.Add(tileComponent);

                //gridTile.name = "Tile zx: " + z + ", " + x+ " || ID: "+ tileComponent.tileReference.tileID;
                gridTile.name = "Tile zx: " + z + ", " + x + " || ID: " + groundCells.Count;
                

            }
        }
        //Debug.Log("List of cells count: " + groundCells.Count);  // Should be the same as "pixelList.Count" and "pix.Length"
        gridCentrePos = transform.position + ((Vector3.right * (displace/2) * (mapLengthColumnsX )) + (Vector3.forward * (displace / 2) * (mapWidthRowsZ )));
        mapSize = new Vector2(displace * mapLengthColumnsX, displace * mapWidthRowsZ);

        //Debug.Log((gridCentrePos) +" "+ mapSize);
    }

    

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    void SetLevelTerrain(List<PixelClass> pList, List<GroundTileScript> gtList) {

        if (pList.Count != gtList.Count || pList == null || gtList == null) {

            Debug.Log("ERROR: Discrepancy between lists p|gt "+ pList.Count + " "+ gtList.Count);
            return;
        }

        /* Layers for each tile type and their binary value:
            9  :  Clear/Objective/Base      512
            10 : FixedTerrain/Impassable    1024
            11 : Rough                      2048
            12 : Hazard                     4096
         */

        for (int i=0; i<gtList.Count;i++) {

            switch (pList[i].colourCompare) {

                case PixelClass.pixelReader.BLACK:
                    ImpassableGround impassableTile = new ImpassableGround();
                    gtList[i].tileReference = impassableTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.hasBlockOnIt = true;
                    gtList[i].tileReference.type = GroundTile.GroundType.IMPASSABLE;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();                    
                    gtList[i].gameObject.layer = 10;
                    
                    break;
                case PixelClass.pixelReader.RED:
                    HazardGround hazardTile = new HazardGround();
                    gtList[i].tileReference = hazardTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.type = GroundTile.GroundType.HAZARD;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 12;

                    break;
                case PixelClass.pixelReader.DARK_RED:
                    HazardGround hazardTileBlocked = new HazardGround();
                    gtList[i].tileReference = hazardTileBlocked;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.hasBlockOnIt = true;
                    gtList[i].tileReference.type = GroundTile.GroundType.HAZARD;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 12;

                    break;

                case PixelClass.pixelReader.GREEN:
                    ClearGround clearTile = new ClearGround();
                    gtList[i].tileReference = clearTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.type = GroundTile.GroundType.CLEAR;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 9;

                    break;
                case PixelClass.pixelReader.TEAL_GREEN:
                    ClearGround clearTileBlocked = new ClearGround();
                    gtList[i].tileReference = clearTileBlocked;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.hasBlockOnIt = true;
                    gtList[i].tileReference.type = GroundTile.GroundType.CLEAR;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 9;

                    break;

                case PixelClass.pixelReader.BLUE:
                    ObjectiveGround objectiveTile = new ObjectiveGround();
                    gtList[i].tileReference = objectiveTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.type = GroundTile.GroundType.OBJECTIVE;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 9;

                    break;

                case PixelClass.pixelReader.GREY:
                    RoughGround roughTile = new RoughGround();
                    gtList[i].tileReference = roughTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.type = GroundTile.GroundType.ROUGH;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 11;

                    break;
                case PixelClass.pixelReader.DARK_GREY:
                    RoughGround roughTileBlocked = new RoughGround();
                    gtList[i].tileReference = roughTileBlocked;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.hasBlockOnIt = true;
                    gtList[i].tileReference.type = GroundTile.GroundType.ROUGH;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 11;

                    break;

                case PixelClass.pixelReader.MAGENTA:
                    BaseGround baseTile = new BaseGround();
                    gtList[i].tileReference = baseTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.type = GroundTile.GroundType.BASE;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 9;

                    baseTilesList.Add(gtList[i]);
                    // Maybe add baseTile to a list of base tiles, then return the 5th member (index [4]) to always find the center of the base prefab (which currently consists of 9 tiles).
                                        
                    break;

                default:
                    ClearGround defaultClearTile = new ClearGround();
                    gtList[i].tileReference = defaultClearTile;
                    gtList[i].tileReference.tileID = i;
                    gtList[i].tileReference.type = GroundTile.GroundType.CLEAR;
                    gtList[i].tileReference.tileRenderer = gtList[i].gameObject.GetComponent<MeshRenderer>();
                    gtList[i].gameObject.layer = 9;

                    Debug.Log("No usable info found, using default tile state");
                    break;
            }
            //Debug.Log("Loading properties for individual tile: " + i);
            gtList[i].tileReference.LoadProperties();

            Vector3 wPosition = gtList[i].nodeReference.worldPosition;
            gtList[i].nodeReference.walkableNode = !(Physics.CheckSphere(wPosition, (displace - 1) / 2, unwalkableNode));

            //gtList[i].nodeReference.movementModifier = gtList[i].tileReference.tileMovementPenaltyForPF;  // deprecated

            if (gtList[i].tileReference.isWalkable | gtList[i].nodeReference.walkableNode) {
                Ray ray = new Ray(gtList[i].transform.position + Vector3.up * 30, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, traversableMask)) {
                    traversableTilesDictionary.TryGetValue(hit.collider.gameObject.layer, out gtList[i].nodeReference.movementModifier);
                }
            }            
        }
        Debug.Log("Level Terrain Set");
    }
    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    #region 3) BUILD BLOCKS
    void SetLevelBlocks(List<GroundTileScript> gtList, GameObject _blockPrefab, GameObject parentTransform)
    {
        // Sets the blocks based on the hasBlockOnIt bool for each tile in groundCells.
        Vector3 yOffset = new Vector3(0, 2.1f, 0);  // based on the tile prefab's half height. Jerry-rigged here for the time being.
        
        foreach (GroundTileScript gt in gtList)
        {
            Vector3 spawnPos = gt.transform.position + yOffset;
            if (gt.tileReference.hasBlockOnIt)
            {
                GameObject dataBlock = GameObject.Instantiate(_blockPrefab, spawnPos, Quaternion.identity, parentTransform.transform);
                LevelBlockScript dBComponent = dataBlock.AddComponent<LevelBlockScript>();
                

                switch (gt.tileReference.type) {
                    case GroundTile.GroundType.CLEAR:
                        BasicBlock basicMinableDB = new BasicBlock(gt,true);
                        dBComponent.levelBlockReference = basicMinableDB;
                        dBComponent.levelBlockReference.dBlockRenderer = dataBlock.gameObject.GetComponent<MeshRenderer>();
                        dBComponent.levelBlockReference.dBSpriterndr = dataBlock.gameObject.GetComponentInChildren<SpriteRenderer>();
                        dBComponent.levelBlockReference.dBHoverandSel = dataBlock.gameObject.GetComponentInChildren<HoverAndSelection>();
                        dBComponent.levelBlockReference.dBHoverandSel.isInterractable = true;
                        dataBlock.AddComponent<Mineable>();
                        dataBlock.AddComponent<Explodable>();
                        break;
                    case GroundTile.GroundType.ROUGH:
                        ToughBlock toughMinableDB = new ToughBlock(gt, true);
                        dBComponent.levelBlockReference = toughMinableDB;
                        dBComponent.levelBlockReference.dBlockRenderer = dataBlock.gameObject.GetComponent<MeshRenderer>();
                        dBComponent.levelBlockReference.dBSpriterndr = dataBlock.gameObject.GetComponentInChildren<SpriteRenderer>();
                        dBComponent.levelBlockReference.dBHoverandSel = dataBlock.gameObject.GetComponentInChildren<HoverAndSelection>();
                        dBComponent.levelBlockReference.dBHoverandSel.isInterractable = true;
                        dataBlock.AddComponent<Mineable>();
                        dataBlock.AddComponent<Explodable>();
                        dataBlock.AddComponent<Tough>();
                        break;
                    case GroundTile.GroundType.IMPASSABLE:
                        UnbreakableBlock unbreakableDB = new UnbreakableBlock(gt, false);
                        dBComponent.levelBlockReference = unbreakableDB;
                        dBComponent.levelBlockReference.dBlockRenderer = dataBlock.gameObject.GetComponent<MeshRenderer>();
                        dBComponent.levelBlockReference.dBSpriterndr = dataBlock.gameObject.GetComponentInChildren<SpriteRenderer>();
                        dBComponent.levelBlockReference.dBHoverandSel = dataBlock.gameObject.GetComponentInChildren<HoverAndSelection>();
                        dBComponent.levelBlockReference.dBHoverandSel.isInterractable = false;
                        break;
                    case GroundTile.GroundType.HAZARD:
                        CorruptBlock corruptMinableDB = new CorruptBlock(gt, true);
                        dBComponent.levelBlockReference = corruptMinableDB;
                        dBComponent.levelBlockReference.dBlockRenderer = dataBlock.gameObject.GetComponent<MeshRenderer>();
                        dBComponent.levelBlockReference.dBSpriterndr = dataBlock.gameObject.GetComponentInChildren<SpriteRenderer>();
                        dBComponent.levelBlockReference.dBHoverandSel = dataBlock.gameObject.GetComponentInChildren<HoverAndSelection>();
                        dBComponent.levelBlockReference.dBHoverandSel.isInterractable = true;
                        dataBlock.AddComponent<Mineable>();
                        dataBlock.AddComponent<CleanMeImCorrupted>();
                        break;
                    default:
                        BasicBlock defaultBasicBlock = new BasicBlock(gt, true);
                        dBComponent.levelBlockReference = defaultBasicBlock;
                        dBComponent.levelBlockReference.dBlockRenderer = dataBlock.gameObject.GetComponent<MeshRenderer>();
                        dBComponent.levelBlockReference.dBSpriterndr = dataBlock.gameObject.GetComponentInChildren<SpriteRenderer>();
                        dBComponent.levelBlockReference.dBHoverandSel = dataBlock.gameObject.GetComponentInChildren<HoverAndSelection>();
                        dBComponent.levelBlockReference.dBHoverandSel.isInterractable = true;
                        dataBlock.AddComponent<Mineable>();
                        dataBlock.AddComponent<Explodable>();
                        Debug.Log("No usable info found, using default block state");
                        break;
                }
                dBComponent.levelBlockReference.LoadPropertiesDB();

                dBComponent.levelBlockReference.dBHoverandSel.isInterractable = dBComponent.levelBlockReference.isBreakable;
                dBComponent.levelBlockReference.dBHoverandSel.hsSprite = dBComponent.levelBlockReference.dBSpriterndr.sprite;
                dBComponent.levelBlockReference.dBHoverandSel.SetSpriteRef();

                dataBlock.name = "Block ID: " + dBComponent.levelBlockReference.dBlockID + " | Coords z/x: "+dBComponent.levelBlockReference.dBlock_YorZ + " ; "+ dBComponent.levelBlockReference.dBlock_X;
            }
            else continue;
        }
    }
    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    #region 4) BUILD PATHFINDING NODES + GET NEIGHBOURS WHEN PFing

    public MapNode NodeFromWorldPoint(Vector3 worldPos) {
        //Debug.Log("Processing worldPos x and z: "+worldPos.x+"; "+worldPos.z + " in " + worldPos +" mapsizeX: "+mapSize.x+" mapsizeY: "+mapSize.y);
        float percentX = (worldPos.x ) / mapSize.x;  // old: (worldPos.x + mapSize.x / 2) / mapSize.x
        float percentY = (worldPos.z ) / mapSize.y;  // old: (worldPos.z + mapSize.y / 2) / mapSize.y
        //Debug.Log("Percents: " + percentX+"; "+percentY);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        //Debug.Log("Clamping: " + percentX + "; " + percentY);

        int x = Mathf.RoundToInt((mapLengthColumnsX - 1) * percentX);
        int y = Mathf.RoundToInt((mapWidthRowsZ - 1) * percentY);


        return groundNodeGrid[x, y];
    }

    public List<MapNode> GetNeighbours(MapNode node) {

        List<MapNode> neighbours = new List<MapNode>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {

                if (x == 0 && y == 0)       // Only having this first condition allows the unit to travel diagonally, so 8 squares/nodes in total. I might cut this down to 4 later on.
                    continue;
                else if (x == -1 && y == -1)     // the 4 following ones allow to only look for neighbours vertically or horizontally.
                    continue;
                else if (x == -1 && y == 1)
                    continue;
                else if (x == 1 && y == -1)
                    continue;
                else if (x == 1 && y == 1)
                    continue;

                /*          
                 */
                int checkX = node.lvlGrid_X + x;
                int checkY = node.lvlGrid_YorZ + y;

                if (checkX >= 0 && checkX < mapLengthColumnsX && checkY >= 0 && checkY < mapWidthRowsZ) {
                    if(groundNodeGrid[checkX, checkY].walkableNode) {

                        neighbours.Add(groundNodeGrid[checkX, checkY]);
                    }                    
                }
            }
        }
        //Debug.Log("Neighbours count: "+ neighbours.Count);
        return neighbours;
    }
    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    #region 5) BUILD UNIT POOLS

    void BuildUnitPools(int _infcap, int _enemycap) { // note : add 2 more parametres for "_vehiclecap" and "_enemycap" later on.

        // take the reference from the public container gameobjects and instantiate the generic unit popcap amount in each of them.
        // take generic unit prefab, add the unit component + child gameObject + scripts.
        // pair up enemy and friendlies into "UnitFriendFoePair" objects.

        player_inf_Array = new playerUnitStruct[_infcap];
        enemy_inf_Array = new enemyUnitStruct[_enemycap];


        for (int i=0; i< player_inf_Array.Length; i++) {

            GameObject infPrefab = GameObject.Instantiate(player_inf_prefab, player_inf_Pool.transform, true);
            infPrefab.name = ("Unit " + (i + 1) + "/" + _infcap);
            infPrefab.layer = 14;
            UnitComponent unitComponent = infPrefab.transform.GetComponent<UnitComponent>() ?? infPrefab.AddComponent<UnitComponent>();
            UnitPositionAndFaction positionAndFaction = infPrefab.transform.GetComponent<UnitPositionAndFaction>() ?? infPrefab.AddComponent<UnitPositionAndFaction>();

            GameObject model = infPrefab.transform.GetChild(0).gameObject;
            model.layer = 14;
            PlayerControlled controlComponent = model.GetComponent<PlayerControlled>() ?? model.AddComponent<PlayerControlled>();
            controlComponent.selectionCircle = infPrefab.transform.GetChild(1).gameObject;

            GameObject mvtIndic = GameObject.Instantiate(destinationPt, player_inf_Pool.transform, true);
            mvtIndic.name = ("Unit " + (i + 1) + "/" + _infcap + "'s destPoint");
            PlayerControlled controlComponent2 = mvtIndic.GetComponent<PlayerControlled>() ?? mvtIndic.AddComponent<PlayerControlled>();
            

            infPrefab.GetComponent<DestinationPoint>().destinationPointObject = mvtIndic; // just in case, might be removed later if the dictionary works properly
            infPrefab.GetComponent<StorageStatus>().isStored = true;

            string player_infStructName = (infPrefab.name + "'s struct holder");
            
            player_inf_Array[i] = new playerUnitStruct(infPrefab, mvtIndic, player_infStructName);
            //Debug.Log(player_infStructName+" - ADDED");
        }

        for (int i=0; i < enemy_inf_Array.Length; i++)
        {
            GameObject enemyPrefab = GameObject.Instantiate(enemy_inf_Prefab, new Vector3 (95,0,25), Quaternion.identity ,enemy_Pool.transform);
            enemyPrefab.name = ("Enemy " + (i + 1) + "/" + _enemycap);
            enemyPrefab.layer = 16;
            NewEnemyAIScript aiScript = enemyPrefab.GetComponent<NewEnemyAIScript>() ?? enemyPrefab.AddComponent<NewEnemyAIScript>();
            UnitPositionAndFaction positionAndFaction = enemyPrefab.transform.GetComponent<UnitPositionAndFaction>() ?? enemyPrefab.AddComponent<UnitPositionAndFaction>();

            GameObject model = enemyPrefab.transform.GetChild(0).gameObject;
            model.layer = 16;
            
            aiScript.enemyModel = model;

            enemyPrefab.GetComponent<StorageStatus>().isStored = false;
            enemyPrefab.SetActive(true); // TEMPORARY until block spawning is implemented.
        }
    }



    void InitialiseUPTM ()
    {
        int dimensions = player_inf_Array.Length + enemy_inf_Array.Length;  // add vehicle later on.
        Debug.Log("Array compare: " + ((player_inf_Array.Length + enemy_inf_Array.Length) == (popcap_inf + enemy_popcap)) );
        unitPTM.distanceTableInit(dimensions);


    }

    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------

    #region 6) CAMERA SETUP FOR MINIMAP AND PLAYER VIEWPORT
    void SetupMinimapCamera()
    {
        minimapCam = CameraReferenceSingleton.Instance.minimapCamPrefab;

        Vector3 spawnPosition = gridCentrePos + new Vector3(0,101,0);
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
        minimapCam.orthographicSize = (int)mapSize.x/2;

        Camera minimapCamIG = Camera.Instantiate(minimapCam, spawnPosition, spawnRotation);
        minimapCamIG.name = "Minimap Camera";
    }

    void SetupStartingArea() {
        cameraHolderPrefab = CameraReferenceSingleton.Instance.gameplayCamHolderPrefab_01;
        GroundTileScript centreBaseTile= baseTilesList[4];
        Vector3 positionOfBase = centreBaseTile.nodeReference.worldPosition;
        Vector3 centreCameraToBase = positionOfBase + cameraOffset;

        GameObject baseBuilding = GameObject.Instantiate(mainBaseBuildingPrefab, positionOfBase, Quaternion.identity);
        baseBuilding.name = "MainBase";

        baseBuilding.GetComponent<UI_ActionsButtons>().UIButtonPack = ui_ButtonPackAssociatorLocal.UIButtonPack[0];   // Make sure the first element in the list is the base button pack !!!!

        // Instantiates camera holder prefab in the world and centres it on the base building.

        GameObject mainCameraHolder = GameObject.Instantiate(cameraHolderPrefab, centreCameraToBase, Quaternion.identity);
        mainCameraHolder.name = "Main Camera Holder";

        CameraControl cameraController = cameraControl.GetComponent<CameraControl>();
        cameraController.FetchCameraprefab(mainCameraHolder);
        UnitComponent.FetchCameraprefab(mainCameraHolder);  // if "FetchCameraprefab()" is not static, this line needs to be placed in BaseSpawner script instead, and the reference set upon unit instantiation.
        UnitSelectionManager uSM = gameObject.GetComponent<UnitSelectionManager>();
        uSM.FetchCameraPrefab(mainCameraHolder);
        BaseSpawner_a01.FetchCameraPrefab(mainCameraHolder);
        MinimapClickToGoThere.FetchCameraprefabs(minimapCam, mainCameraHolder, cameraOffset, mapSize);


        gameObject.GetComponent<UI_SetCamsOrder>().SetCams(mainCameraHolder);
    }
    #endregion

    //---------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------
    

    private void OnDrawGizmos() {        
        Gizmos.DrawWireCube(gridCentrePos,new Vector3(mapSize.x,1, mapSize.y));
    }
    

    [System.Serializable]
    public class TileType {
        public LayerMask terrainTileLayerMask;
        public int terrainTileDistanceModifier;
    }
}


//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                              ADDITIONAL STRUCTS
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------


[Serializable]
public struct playerUnitStruct
{
    public GameObject unitPrefab;
    public GameObject destinationIndicator;
    string unitStructName;

    public playerUnitStruct(GameObject _unitPrefab, GameObject _destinationIndicator, string _unitStructName)
    {
        unitPrefab = _unitPrefab;
        destinationIndicator = _destinationIndicator;
        unitStructName = _unitStructName;
    }
}

[Serializable]
public struct enemyUnitStruct
{
    public GameObject unitPrefab;
    string unitStructName;

    public enemyUnitStruct(GameObject _unitPrefab,  string _unitStructName)
    {
        unitPrefab = _unitPrefab;
        unitStructName = _unitStructName;
    }
}
