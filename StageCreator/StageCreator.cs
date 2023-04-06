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

        GameObject selectedObject;
        Vec4 selectedColor;

        int readingInput = 0;

        string currInput;
        bool awaitingNextInput = false;

        bool enterWasDown, retroScaleCheck;
        bool saving = false;

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

            if (selectedObject == null) { return; }

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
                        selectedObject.SetPosition(new Vec2(int.Parse(currInput), selectedObject.GetLocalPosition().Y));
                        Console.WriteLine("^ SET");

                        readingInput = 0;
                    }else if(readingInput == 2)
                    {
                        selectedObject.SetPosition(new Vec2(selectedObject.GetLocalPosition().X, int.Parse(currInput)));
                        Console.WriteLine("^ SET");

                        readingInput = 0;
                    }
                    else if (readingInput == 3)
                    {
                        selectedObject.SetScale(new Vec2(int.Parse(currInput), selectedObject.GetLocalScale().Y));
                        Console.WriteLine("[SETTING SCALE Y] Selected Object Scale: " + selectedObject.GetLocalScale().ToString());

                        currInput = "";
                        readingInput = 4;
                    }
                    else if (readingInput == 4 && !enterWasDown)
                    {
                        selectedObject.SetScale(new Vec2(selectedObject.GetLocalScale().X, int.Parse(currInput)));
                        Console.WriteLine("^SET");

                        readingInput = 0;
                    }
                    else if (readingInput == 5)
                    {
                        selectedObject.SetRotation(_2DGameMaker.Utils.Math.Math.DegToRad(int.Parse(currInput)));
                        Console.WriteLine("^SET");

                        readingInput = 0;
                    }

                    updateRel(selectedObject);
                    enterWasDown = true;

                }
                else
                {
                    enterWasDown = false;
                }
            }
            else
            {
                if (Input.Input.GetKey(GLFW.Keys.Left))
                {
                    selectedObject.SetPosition(selectedObject.GetLocalPosition() - (Vec2.OneX * 50 * Game.GameTime.DeltaTimeScale()));
                }

                if (Input.Input.GetKey(GLFW.Keys.Right))
                {
                    selectedObject.SetPosition(selectedObject.GetLocalPosition() + (Vec2.OneX * 50 * Game.GameTime.DeltaTimeScale()));
                }

                if (Input.Input.GetKey(GLFW.Keys.Up))
                {
                    selectedObject.SetPosition(selectedObject.GetLocalPosition() - (Vec2.OneY * 50 * Game.GameTime.DeltaTimeScale()));
                }

                if (Input.Input.GetKey(GLFW.Keys.Down))
                {
                    selectedObject.SetPosition(selectedObject.GetLocalPosition() + (Vec2.OneY * 50 * Game.GameTime.DeltaTimeScale()));
                }

                if (Input.Input.GetKey(GLFW.Keys.X))
                {
                    Console.WriteLine("[SETTING X] Selected Object Position: " + selectedObject.GetLocalPosition().ToString());
                    readingInput = 1;
                    currInput = "";
                }

                if (Input.Input.GetKey(GLFW.Keys.Y))
                {
                    Console.WriteLine("[SETTING Y] Selected Object Position: " + selectedObject.GetLocalPosition().ToString());
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
                            selectedObject.SetScale((selectedObject.GetScale() / 16) * v32X_RETRO_SCALAR);
                            Console.WriteLine("[SETTING SCALE] Retro-Scaled Object: " + selectedObject.GetLocalScale().ToString());
                            updateRel(selectedObject);

                        }
                        else
                        {
                            Console.WriteLine("[SETTING SCALE X] Selected Object Scale: " + selectedObject.GetLocalScale().ToString());
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
                    Console.WriteLine("[SETTING ROTATION] Selected Object Rotation: " + selectedObject.GetLocalRotation());
                    readingInput = 5;
                    currInput = "";
                }

                if (Input.Input.GetKey(GLFW.Keys.Delete))
                {
                    if (Input.Input.GetKey(GLFW.Keys.LeftControl))
                    {
                        Console.WriteLine(relGameObjects.IndexOf(selectedObject));
                        StageObject rel = savedObjects[relGameObjects.IndexOf(selectedObject)];
                        Destroy(selectedObject, rel.Layer);

                        savedObjects.Remove(rel);
                        relGameObjects.Remove(selectedObject);

                        selectedObject = null;
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

            so.Components = new StageComponent[obj.ObjectAppenedScripts.Count];
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
                            if (obj != selectedObject)
                            {
                                if (selectedObject != null)
                                {
                                    selectedObject.Texture.SetColor(selectedColor);
                                }
                                selectedObject = obj;
                                selectedColor = obj.Texture.GetColor();
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

                if (instance.selectedObject != null)
                {
                    int i = 0;
                    List<object> args = new List<object>();
                    args.Add((GameObject)instance.selectedObject);
                    foreach (string s in code.Split(":"))
                    {
                        if(i > 1)
                        {
                            args.Add(s);
                        }
                        i++;
                    }

                    Type t = Type.GetType(code_d);
                    foreach(ObjectAppendedScript oas in instance.selectedObject.ObjectAppenedScripts)
                    {
                        Console.WriteLine(t.ToString());
                        if (ScriptManager.GetOASParams(oas, out _) == t.ToString())
                        {
                            return;
                        }
                    }
                    instance.selectedObject.AppendScript((ObjectAppendedScript)Activator.CreateInstance(t, args.ToArray()));
                    Console.WriteLine("ADDED: " + code_d);
                    instance.updateRel(instance.selectedObject);
                }
            }
        }

    }
}
