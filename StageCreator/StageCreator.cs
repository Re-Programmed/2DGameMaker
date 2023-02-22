using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Objects.Scripting.GUI;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.StageCreator
{
    public class StageCreator : Game.Game
    {
        public StageCreator(string title)
            : base(title)
        {

        }

        protected override void Close()
        {

        }

        protected override void loadCamera()
        {
            cam = new MoveableCamera(Vec2.Zero, 1f);
        }

        protected override void Init()
        {
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
        private void ManageSelectedObject()
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
            }
            
        }

        protected override void Update()
        {
            base.Update();

            ManageSelectedObject();

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

                Console.WriteLine(so.GetPosition().ToString());
            }
        }
    }
}
