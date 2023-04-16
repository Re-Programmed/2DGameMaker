using _2DGameMaker.Game.Stages;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Objects.Scripting.GUI;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.BeatReadr;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace _2DGameMaker.StageCreator
{
    public class StageCreator : Game.Game
    {
        /// <summary>
        /// The value to multiply sprites scales by, and to lock sprites positions to, to emulate a 224x320 resolution.
        /// </summary>
        public const byte v32X_RETRO_SCALAR = 192;
        const string LvlCreatorSaveName = "curr_lvl_creator";

        public StageCreator(string title)
            : base(title)
        {

        }

        static List<StageObject> savedObjects = new List<StageObject>();
        static List<GameObject> relGameObjects = new List<GameObject>();

        protected override void Close()
        {        
            AppDataManager.UpdateFile("lvlcreator\\lvldata", new Stage(savedObjects.ToArray()), true);
        }

        protected override void loadCamera()
        {
            cam = new MoveableCamera(Vec2.Zero, 1f);
        }

        private static Random random = new Random();

        protected override void Init()
        {
            /*game specific*/_2DGameMaker.GAME_NAME.StageElements.Grass.Grass.InitilizeGrassSpawns();
            Stage s;
            if (AppDataManager.GetFile("lvlcreator\\lvldata" + AppDataManager.B64OverrideSuffix, out s))
            {
                if(StageManager.RegisterStage(LvlCreatorSaveName, s) != null)
                {
                    GameObject[] go;
                    StageManager.GenerateStage(LvlCreatorSaveName, out go);

                    foreach(GameObject obj in go)
                    {
                        relGameObjects.Add(obj);
                    }


                    savedObjects.AddRange(StageManager.GetStageObjects(LvlCreatorSaveName));
                }
            }

            GUIEditCreator.InitGUI();
            ScriptManager.CreateScriptGUI();
            GUIManager.GUIEventHandler += GUICreatorEvent;

            Input.Input.OnScroll += onScroll;
        }

        protected override void LateUpdate()
        {
            Utils.PhysX.PhysicsManager.PhysicsTick();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Render()
        {

        }

        List<GameObject> selectedObject = new List<GameObject>();
        List<Vec4> selectedColor = new List<Vec4>();

        int readingInput = 0;

        string currInput;
        bool awaitingNextInput = false;

        bool enterWasDown, retroScaleCheck;
        bool saving = false;

        bool duped = false;

        private void manageSelectedObject()
        {
            if (Input.Input.GetKey(GLFW.Keys.M))
            {
                if (!saving)
                {
                    saving = true;

                    Console.WriteLine("*SAVING*");
                    Close();
                }
            }
            else if (saving)
            {
                saving = false;
            }

            if (selectedObject.Count < 1) { return; }

            if (readingInput != 0)
            {
                int num = Input.Input.GetNumberKey();

                if (!awaitingNextInput)
                {
                    if (num != -1)
                    {
                        if (num == -2)
                        {
                            if (currInput == "")
                            {
                                currInput = "-";
                                Console.WriteLine("CURRENT: -");
                            }
                            return;
                        }

                        awaitingNextInput = true;
                        currInput += num.ToString();
                        Console.WriteLine("CURRENT: " + currInput);
                    }
                }

                if (num == -1)
                {
                    awaitingNextInput = false;
                }


                if (Input.Input.GetKey(GLFW.Keys.Enter) && currInput != "")
                {
                    if (readingInput == 1)
                    {
                        foreach (GameObject go in selectedObject) { go.SetPosition(new Vec2(int.Parse(currInput), go.GetLocalPosition().Y)); }
                        Console.WriteLine("^ SET");

                        readingInput = 0;
                    }else if(readingInput == 2)
                    {
                        foreach (GameObject go in selectedObject) { go.SetPosition(new Vec2(go.GetLocalPosition().X, int.Parse(currInput))); }
                        Console.WriteLine("^ SET");

                        readingInput = 0;
                    }
                    else if (readingInput == 3)
                    {
                        foreach (GameObject go in selectedObject) { go.SetScale(new Vec2(int.Parse(currInput), go.GetLocalScale().Y)); }
                        Console.WriteLine("[SETTING SCALE Y] Selected Object Scale: " + selectedObject[0].GetLocalScale().ToString());

                        currInput = "";
                        readingInput = 4;
                    }
                    else if (readingInput == 4 && !enterWasDown)
                    {
                        foreach (GameObject go in selectedObject) { go.SetScale(new Vec2(go.GetLocalScale().X, int.Parse(currInput))); }
                        Console.WriteLine("^SET");

                        readingInput = 0;
                    }
                    else if (readingInput == 5)
                    {
                        foreach (GameObject go in selectedObject) { go.SetRotation(_2DGameMaker.Utils.Math.Math.DegToRad(int.Parse(currInput))); }
                        Console.WriteLine("^SET");

                        readingInput = 0;
                    }else if(readingInput == 6)
                    {
                        foreach (GameObject go in selectedObject) { go.SetPosition(new Vec2(go.GetPosition().X * int.Parse(currInput), go.GetLocalPosition().Y * int.Parse(currInput))); }
                        Console.WriteLine("^SET");

                        readingInput = 0;
                    }

                    foreach (GameObject go in selectedObject) { updateRel(go); }
                    enterWasDown = true;

                }
                else
                {
                    enterWasDown = false;
                }
            }
            else
            {
                if(Input.Input.GetKey(GLFW.Keys.B))
                {
                    if(!duped)
                    {
                        StaticObject so = selectedObject[0].Clone<StaticObject>();
                        so.Texture.SetColor(Vec4.One);

                        INSTANCE.Instantiate(so, 1);

                        relGameObjects.Add(so);
                        savedObjects.Add(new StageObject(savedObjects[relGameObjects.IndexOf(selectedObject[0])]));
                        duped = true;
                    }
                }
                else
                {
                    duped = false;
                }

                if (Input.Input.GetKey(GLFW.Keys.Left))
                {
                    foreach (GameObject go in selectedObject) { go.SetPosition(go.GetLocalPosition() - (Vec2.OneX * 50 * Game.GameTime.DeltaTimeScale())); }
                }

                if (Input.Input.GetKey(GLFW.Keys.Right))
                {
                    foreach (GameObject go in selectedObject) { go.SetPosition(go.GetLocalPosition() + (Vec2.OneX * 50 * Game.GameTime.DeltaTimeScale())); }
                }

                if (Input.Input.GetKey(GLFW.Keys.Up))
                {
                    foreach (GameObject go in selectedObject) { go.SetPosition(go.GetLocalPosition() - (Vec2.OneY * 50 * Game.GameTime.DeltaTimeScale())); }
                }

                if (Input.Input.GetKey(GLFW.Keys.Down))
                {
                    foreach (GameObject go in selectedObject) { go.SetPosition(go.GetLocalPosition() + (Vec2.OneY * 50 * Game.GameTime.DeltaTimeScale())); }
                }

                if (Input.Input.GetKey(GLFW.Keys.X))
                {
                    Console.WriteLine("[SETTING X] Selected Object Position: " + selectedObject[0].GetLocalPosition().ToString());
                    readingInput = 1;
                    currInput = "";
                }

                if (Input.Input.GetKey(GLFW.Keys.V))
                {
                    Console.WriteLine("[MULTIPLYING POSITION] Selected Object Position: " + selectedObject[0].GetLocalPosition().ToString());
                    readingInput = 6;
                    currInput = "";
                }

                if (Input.Input.GetKey(GLFW.Keys.Y))
                {
                    Console.WriteLine("[SETTING Y] Selected Object Position: " + selectedObject[0].GetLocalPosition().ToString());
                    readingInput = 2;
                    currInput = "";
                }

                if (Input.Input.GetKey(GLFW.Keys.U))
                {
                    if (!retroScaleCheck)
                    {
                        retroScaleCheck = true;

                        if (Input.Input.GetKey(GLFW.Keys.LeftControl))
                        {
                            foreach (GameObject go in selectedObject) { go.SetScale((go.GetScale() / 16) * v32X_RETRO_SCALAR); }
                            Console.WriteLine("[SETTING SCALE] Retro-Scaled Object: " + selectedObject[0].GetLocalScale().ToString());
                            foreach (GameObject go in selectedObject) { updateRel(go); }

                        }
                        else
                        {
                            Console.WriteLine("[SETTING SCALE X] Selected Object Scale: " + selectedObject[0].GetLocalScale().ToString());
                            readingInput = 3;
                            currInput = "";
                        }
                    }
                }else if(retroScaleCheck)
                {
                    retroScaleCheck = false;
                }
                

                if (Input.Input.GetKey(GLFW.Keys.I))
                {
                    Console.WriteLine("[SETTING ROTATION] Selected Object Rotation: " + selectedObject[0].GetLocalRotation());
                    readingInput = 5;
                    currInput = "";
                }

                if (Input.Input.GetKey(GLFW.Keys.Delete))
                {
                    if (Input.Input.GetKey(GLFW.Keys.LeftControl))
                    {
                        foreach (GameObject go in selectedObject)
                        {
                            Console.WriteLine(relGameObjects.IndexOf(go));
                            StageObject rel = savedObjects[relGameObjects.IndexOf(go)];
                            Destroy(go, rel.Layer);

                            savedObjects.Remove(rel);
                            relGameObjects.Remove(go);
                        }

                        selectedObject = new List<GameObject>();
                    }
                }
            }
            
        }

        private void updateRel(GameObject obj)
        {
            StageObject so = savedObjects[relGameObjects.IndexOf(obj)];

            so.Position = obj.GetLocalPosition().GetArray();
            so.Rotation = obj.GetLocalRotation();
            so.Scale = obj.GetLocalScale().GetArray();

            
            so.Components = new StageComponent[obj.ObjectAppenedScripts.Count - 1];
            for (int i = 0; i < so.Components.Length; i++)
            {
                string[] p;
                string sc = ScriptManager.GetOASParams(obj.ObjectAppenedScripts[i], out p);
                so.Components[i] = new StageComponent(sc, p);
            }
            
            savedObjects[relGameObjects.IndexOf(obj)] = so;
        }

        double currentOffset = 0;
        private void onScroll(GLFW.Window window, double xoff, double yoff)
        {
            if(Input.Input.GetKey(GLFW.Keys.LeftControl))
            {
                currentOffset += yoff;
                GUIEditCreator.SetPlaceButtonOffset((float)currentOffset);
            }
        }

        protected override void Update()
        {

            base.Update();

            ScriptManager.Update();

            manageSelectedObject();

            if (Input.Input.GetMouseButtonEvent(GLFW.MouseButton.Right) == Input.Input.MOUSE_PRESSED)
            {
                foreach(ObjectLayer layer in objects)
                {
                    foreach(GameObject obj in layer.objects)
                    {
                        if(obj.IsUI()) { continue; }
                        if(CollisionCheck.BoxCheck(CurrMousePositionWorldCoords, obj, true))
                        {
                            if (!selectedObject.Contains(obj))
                            {
                                if(!Input.Input.GetKey(GLFW.Keys.LeftShift))
                                {
                                    if (selectedObject.Count != 0)
                                    {
                                        int i = 0;
                                        foreach (GameObject go in selectedObject) { go.Texture.SetColor(selectedColor[i]); i++; }
                                    }
                                    selectedObject = new List<GameObject>();
                                    selectedColor = new List<Vec4>();
                                }

                                selectedObject.Add(obj);
                                selectedColor.Add(obj.Texture.GetColor());

                                Console.WriteLine("-------- SCRIPTS: --------");
                                foreach (ObjectAppendedScript oas in obj.ObjectAppenedScripts)
                                {
                                    Console.WriteLine(ScriptManager.GetOASParams(oas, out _));
                                }
                                Console.WriteLine("--------------------------");

                                obj.Texture.SetColor(Vec4.Red);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public const string SCRIPT_BUTTON_PREFIX = "appendscript";
        public const string PLACE_OBJECT_BUTTON_PREFIX = "placeobj";

        public static void GUICreatorEvent(GUIManager.GUIEventType type, string code)
        {
            if(code.StartsWith(PLACE_OBJECT_BUTTON_PREFIX))
            {
                string[] code_d = code.Split(":");

                StaticObject so = new StaticObject(INSTANCE.GetCamera().FocusPosition, new ObjectTexture(AssetManager.GetTexture(code_d[1], code_d[2])));

                INSTANCE.Instantiate(so, 1);

                relGameObjects.Add(so);
                savedObjects.Add(new StageObject(so, code_d[1], code_d[2]));
            }

            if(code.StartsWith(SCRIPT_BUTTON_PREFIX))
            {
                Console.WriteLine("BUTTON");
                string code_d = code.Split(":")[1];

                StageCreator instance = (StageCreator)INSTANCE;

                if (instance.selectedObject.Count != 0)
                {
                    foreach (GameObject go in instance.selectedObject)
                    {
                        int i = 0;
                        List<object> args = new List<object>();
                        args.Add((GameObject)go);
                        foreach (string s in code.Split(":"))
                        {
                            if (i > 1)
                            {
                                args.Add(s);
                            }
                            i++;
                        }

                        Type t = Type.GetType(code_d);
                        foreach (ObjectAppendedScript oas in go.ObjectAppenedScripts)
                        {
                            Console.WriteLine(t.ToString());
                            if (ScriptManager.GetOASParams(oas, out _) == t.ToString())
                            {
                                return;
                            }
                        }
                        go.AppendScript((ObjectAppendedScript)Activator.CreateInstance(t, args.ToArray()));
                        Console.WriteLine("ADDED: " + code_d);
                        instance.updateRel(go);
                    }
                }
            }
        }

    }
}
