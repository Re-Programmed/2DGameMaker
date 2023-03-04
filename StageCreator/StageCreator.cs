using _2DGameMaker.Game.Stages;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Objects.Scripting.GUI;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.BeatReadr;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.StageCreator
{
    public class StageCreator : Game.Game
    {
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
            GUIManager.GUIEventHandler += GUICreatorEvent;
        }

        protected override void LateUpdate()
        {

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

        bool enterWasDown;
        private void manageSelectedObject()
        {
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
                    Console.WriteLine("[SETTING SCALE X] Selected Object Scale: " + selectedObject.GetLocalScale().ToString());
                    readingInput = 3;
                    currInput = "";
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

            savedObjects[relGameObjects.IndexOf(obj)] = so;
        }

        protected override void Update()
        {

            base.Update();

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

        public static void GUICreatorEvent(GUIManager.GUIEventType type, string code)
        {
            if(code.StartsWith("placeobj"))
            {
                string[] code_d = code.Split(":");

                StaticObject so = new StaticObject(INSTANCE.GetCamera().FocusPosition, new ObjectTexture(AssetManager.GetTexture(code_d[1], code_d[2])));

                INSTANCE.Instantiate(so, 1);

                relGameObjects.Add(so);
                savedObjects.Add(new StageObject(so, code_d[1], code_d[2]));
            }
        }
    }
}
