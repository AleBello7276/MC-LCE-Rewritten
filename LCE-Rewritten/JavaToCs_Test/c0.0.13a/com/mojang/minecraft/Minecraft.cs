using com.mojang.minecraft.character;
using com.mojang.minecraft.gui;
using com.mojang.minecraft.level;
using com.mojang.minecraft.level.levelgen;
using com.mojang.minecraft.level.tile;
using com.mojang.minecraft.particle;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;

using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace com.mojang.minecraft
{

    public class Minecraft : GameWindow, LevelLoaderListener
    {
        private Screen screen = null;
        public int width;
        public int height;

        
        //Fog Arrays
        private float[] fogColor0 = new float[4];
        private float[] fogColor1 = new float[4];

        //Selected Tile
        public int paintTexture = 101;

        public bool appletMode = false;
        public volatile bool pause = false;
        private bool mouseGrabbed = false;
        private int editMode = 0;
        private int yMouseAxis = 1;
        public float fov = 70.0F;

        private Cursor emptyCursor;
        public Textures textures;
        public Font font;
        private LevelIO levelIo;
        private LevelGen levelGen;
        private Timer timer = new Timer(20.0F);
        private Level level;
        private LevelRenderer levelRenderer;
        private ParticleEngine particleEngine;
        public User user = new User("noname");
        private List<Entity> entities = new List<Entity>();
        private Player player;


        //Fps Counter
        private string fpsString = "";
        int frames = 0;
        private static readonly Stopwatch stopwatch = new();

        //Pick
        private int[] viewportBuffer = new int[16];
        private int[] selectBuffer = new int[2000];
        private HitResult hitResult = null;
        float[] lb = new float[16];
        private string title = "";
        //Idenity Matrix for pick
        private static readonly float[] IDENTITY_MATRIX =
        new float[] {
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f };

        //Mod - AleBello
        public bool PauseChecker = false;
        private RedstoneTile redstoneTile;
        private SkinTile skinTile;
        private SlabTile slabTile;

        //Texture Ids
        int terrId;
        int dirtId;

        //Mouse / Camera Rotation
        bool _firstMove;
        Vector2 _lastPos;


        public Minecraft(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            stopwatch.Start();
            this.width = ClientSize.X;
            this.height = ClientSize.Y;
            this.textures = new Textures();
            levelIo = new LevelIO(this);
            levelGen = new LevelGen(this);
            terrId = this.textures.loadTexture("res/terrain.png", 9728);
            dirtId = this.textures.loadTexture("res/dirt.png", 9728);
        }



        protected override void OnLoad()
        {
            base.OnLoad();
            init();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            run();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            this.width = ClientSize.X;
            this.height = ClientSize.Y;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            destroy();
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            //Make Sure winodw height is not 0
            if (ClientSize.Y < 50)
            {
                Size = new Vector2i(ClientSize.X, 50);
            }
        }


        public void init()
        {
            //Fill Fog Arrays (The fog is the block in shadow)
            int col1 = 920330;
            float fr = 0.5F;
            float fg = 0.8F;
            float fb = 1.0F;
            fogColor0 = new float[] { fr, fg, fb, 1.0F };
            fogColor1 = new float[] { (float)(col1 >> 16 & 255) / 255.0F, (float)(col1 >> 8 & 255) / 255.0F, (float)(col1 & 255) / 255.0F, 1.0F };

            //Initialize GL things
            this.checkGlError("Pre startup");
            GL.Enable(EnableCap.Texture2D);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.ClearColor(fr, fg, fb, 0.0F);
            GL.ClearDepth(1.0D);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0F);
            GL.CullFace(CullFaceMode.Back);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            this.checkGlError("Startup");
            this.font = new Font("res/default.png", this.textures);
            GL.Viewport(0, 0, width, this.height);
            this.level = new Level();
            bool success = false;

            // Load / Generate Level
            try
            {
                success = levelIo.load(level, new FileStream("level.dat", FileMode.Open));
                if (!success)
                {
                    success = levelIo.loadLegacy(level, new FileStream("level.dat", FileMode.Open));
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            if (!success)
            {
                this.levelGen.generateLevel(this.level, this.user.name, 256, 256, 64);
            }

            this.levelRenderer = new LevelRenderer(this.level, this.textures);
            this.player = new Player(level);
            this.particleEngine = new ParticleEngine(this.level, this.textures);
            //Mod - AleBello
            this.redstoneTile = new RedstoneTile(8);
            this.slabTile = new SlabTile(100, 4);


            //Spawn 10 Mobs
            for (int i = 0; i < 10; ++i)
            {
                Zombie zombie = new Zombie(this.level, this.textures, 128.0F, 0.0F, 128.0F);
                zombie.resetPos();
                this.entities.Add(zombie);
            }

            this.checkGlError("Post startup");
        }

        //Set the screen (the pause screen for example)
        public void setScreen(Screen tscreen)
        {
            this.screen = tscreen;
            if (tscreen != null)
            {
                int screenWidth = width * 240 / height;
                int screenHeight = height * 240 / height;
                tscreen.init(this, screenWidth, screenHeight);
            }
        }

        //Checks for OpenGL errors
        private void checkGlError(string stringa)
        {
            int errorCode = (int)GL.GetError();
            if (errorCode != 0)
            {
                //string errorString = GLU.gluErrorString(errorCode);
                string errorString = "implement gluErrorString plz :}";
                Console.WriteLine("########## GL ERROR ##########");
                Console.WriteLine("@ " + stringa);
                Console.WriteLine(errorCode + ": " + errorString);
                Close();
            }

        }

        public void destroy()
        {
            this.attemptSaveLevel();
        }

        protected void attemptSaveLevel()
        {
            try
            {
                using (FileStream fileStream = new FileStream("level.dat", FileMode.Create))
                {
                    this.levelIo.save(this.level, fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        public void run()
        {
            if (this.pause)
            {
                Thread.Sleep(100);
            }
            else
            {
                //Update Timer
                this.timer.advanceTime();


                //20 ticks per second
                for (int i = 0; i < this.timer.ticks; ++i)
                {
                    this.tick();
                }

                //Render
                this.checkGlError("Pre render");
                this.render(this.timer.a);
                this.checkGlError("Post render");

                //Counter
                ++frames;
                if (stopwatch.ElapsedMilliseconds >= 1000)
                {
                    stopwatch.Restart();
                    this.fpsString = frames + " fps, " + Chunk.updates + " chunk updates";
                    Chunk.updates = 0;
                    frames = 0;
                }
            }
        }


        public void grabMouse()
        {
            if (!this.mouseGrabbed)
            {
                this.player.releaseAllKeys(mouseGrabbed);
                this.mouseGrabbed = true;


                CursorState = CursorState.Grabbed;


                this.setScreen((Screen)null);

            }
        }

        public void releaseMouse()
        {
            if (this.mouseGrabbed)
            {
                this.player.releaseAllKeys(mouseGrabbed);
                this.mouseGrabbed = false;

                CursorState = CursorState.Normal;


                this.setScreen((Screen)new PauseScreen());
            }
        }





        private void handleMouseClick()
        {
            if (this.editMode == 0)
            {
                if (this.hitResult != null)
                {
                    Tile oldTile = Tile.tiles[this.level.getTile(this.hitResult.x, this.hitResult.y, this.hitResult.z)];
                    bool changed = this.level.setTile(this.hitResult.x, this.hitResult.y, this.hitResult.z, 0);
                    if (oldTile != null && changed)
                    {
                        oldTile.destroy(this.level, this.hitResult.x, this.hitResult.y, this.hitResult.z, this.particleEngine);
                    }
                }
            }
            else if (this.hitResult != null)
            {
                int x = this.hitResult.x;
                int y = this.hitResult.y;
                int z = this.hitResult.z;
                if (this.hitResult.f == 0)
                {
                    --y;
                }

                if (this.hitResult.f == 1)
                {
                    ++y;
                }

                if (this.hitResult.f == 2)
                {
                    --z;
                }

                if (this.hitResult.f == 3)
                {
                    ++z;
                }

                if (this.hitResult.f == 4)
                {
                    --x;
                }

                if (this.hitResult.f == 5)
                {
                    ++x;
                }

                AABB aabb = Tile.tiles[this.paintTexture].getAABB(x, y, z);
                if (aabb == null || this.isFree(aabb))
                {
                    this.level.setTile(x, y, z, this.paintTexture);
                }
            }

        }

        public void tick()
        {
            if (this.screen == null)
            {
                if (KeyboardState.IsAnyKeyDown)
                {
                    if (KeyboardState.IsKeyDown(Keys.Escape))
                    {
                        this.releaseMouse();
                    }

                    if (KeyboardState.IsKeyDown(Keys.Enter))
                    {
                        this.attemptSaveLevel();
                    }

                    if (KeyboardState.IsKeyDown(Keys.R))
                    {
                        this.player.ResetPos();
                    }
                    //Input Mod


                    //Input Scelta Blocchi
                    if (KeyboardState.IsKeyDown(Keys.D1))
                    {
                        this.paintTexture = 1;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D2))
                    {
                        this.paintTexture = 2;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D3))
                    {
                        this.paintTexture = 3;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D4))
                    {
                        this.paintTexture = 4;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D5))
                    {
                        this.paintTexture = 5;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D6))
                    {
                        this.paintTexture = 6;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D7))
                    {
                        this.paintTexture = 7;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D8))
                    {
                        this.paintTexture = 8;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D9))
                    {
                        this.paintTexture = 9;
                    }

                    if (KeyboardState.IsKeyDown(Keys.D0))
                    {
                        Console.WriteLine(paintTexture);
                    }



                    if (KeyboardState.IsKeyDown(Keys.Y))
                    {
                        this.yMouseAxis *= -1;
                    }

                    if (KeyboardState.IsKeyDown(Keys.G))
                    {
                        this.entities.Add(new Zombie(this.level, this.textures, this.player.x, this.player.y - 2, this.player.z));
                    }

                    if (KeyboardState.IsKeyDown(Keys.F))
                    {
                        this.levelRenderer.toggleDrawDistance();
                    }
                }
            }



            //Mouse Imputs
            if (!this.mouseGrabbed && MouseState.IsAnyButtonDown)
            {
                if (screen == null)
                {
                    this.grabMouse();
                }
            }
            else
            {
                if (MouseState.IsButtonDown(MouseButton.Left) && MouseState.IsAnyButtonDown)
                {
                    this.handleMouseClick();
                }

                if (MouseState.IsButtonDown(MouseButton.Right) && MouseState.IsAnyButtonDown)
                {
                    this.editMode = (this.editMode + 1) % 2;
                }
            }


            //Update Screen
            if (this.screen != null)
            {
                this.screen.updateEvents();
                if (this.screen != null)
                {
                    this.screen.tick();
                }
            }

            //Tick things
            this.level.tick();
            this.particleEngine.tick();

            for (int i = 0; i < this.entities.Count; ++i)
            {
                ((Entity)this.entities[i]).tick();
                if (((Entity)this.entities[i]).removed)
                {
                    this.entities.RemoveAt(i--);
                }
            }

            //Tick other things
            this.player.tick(KeyboardState);
            this.redstoneTile.tick(level, 0, 0, 0); // Mod - AleBello
        }

        //IDK
        private bool isFree(AABB aabb)
        {
            if (this.player.bb.intersects(aabb))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.entities.Count; ++i)
                {
                    if (((Entity)this.entities[i]).bb.intersects(aabb))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        //Move the Camera to the player position
        private void moveCameraToPlayer(float a)
        {
            GL.Translate(0.0F, 0.0F, -0.3F);
            GL.Rotate(this.player.xRot, 1.0F, 0.0F, 0.0F);
            GL.Rotate(this.player.yRot, 0.0F, 1.0F, 0.0F);
            float x = this.player.xo + (this.player.x - this.player.xo) * a;
            float y = this.player.yo + (this.player.y - this.player.yo) * a;
            float z = this.player.zo + (this.player.z - this.player.zo) * a;
            GL.Translate(-x, -y, -z);
        }

        private void setupCamera(float a)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            gluPerspective(fov, (float)this.width / (float)this.height, 0.05F, 1024.0F);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            this.moveCameraToPlayer(a);
        }


        private static void __gluMakeIdentityf(float[] m)
        {
            Array.Copy(IDENTITY_MATRIX, m, IDENTITY_MATRIX.Length);   
        }

        public static void gluPerspective(float fovy, float aspect, float zNear, float zFar)
        {
            float sine, cotangent, deltaZ;
            float radians = fovy / 2 * (float)Math.PI / 180;

            deltaZ = zFar - zNear;
            sine = (float)Math.Sin(radians);

            if ((deltaZ == 0) || (sine == 0) || (aspect == 0))
            {
                return;
            }

            cotangent = (float)Math.Cos(radians) / sine;


            float[] matrix = new float[16];
            __gluMakeIdentityf(matrix);

            for (int i = 0; i < 16; i++)
            {
                matrix[i] = 0;
            }

            matrix[0 * 4 + 0] = cotangent / aspect;
            matrix[1 * 4 + 1] = cotangent;
            matrix[2 * 4 + 2] = -(zFar + zNear) / deltaZ;
            matrix[2 * 4 + 3] = -1;
            matrix[3 * 4 + 2] = -2 * zNear * zFar / deltaZ;
            matrix[3 * 4 + 3] = 0;

            
            GL.MultMatrix(matrix);
        }

        public static void gluPickMatrix(float x, float y, float deltaX, float deltaY, int[] viewport)
        {
            if (deltaX <= 0 || deltaY <= 0)
            {
                return;
            }

            // Translate and scale the picked region to the entire window
            GL.Translate((viewport[2] - 2 * (x - viewport[0])) / deltaX,
                        (viewport[3] - 2 * (y - viewport[1])) / deltaY, 0);
            GL.Scale(viewport[2] / deltaX, viewport[3] / deltaY, 1.0);
        }




        private void setupPickCamera(float a, int x, int y)
        {
            GL.MatrixMode((MatrixMode)5889);
            GL.LoadIdentity();
            Array.Clear(viewportBuffer);
            GL.GetInteger(GetIndexedPName.Viewport, 0, this.viewportBuffer);
            //this.viewportBuffer.Reverse();
            gluPickMatrix((float)x, (float)y, 5.0F, 5.0F, this.viewportBuffer);
            gluPerspective(70.0F, (float)this.width / (float)this.height, 0.05F, 1024.0F);
            GL.MatrixMode((MatrixMode)5888);
            GL.LoadIdentity();
            this.moveCameraToPlayer(a);
        }

        private void pick(float a)
        {
            Array.Clear(selectBuffer);
            GL.SelectBuffer(selectBuffer.Length, this.selectBuffer);
            GL.RenderMode((RenderingMode)7170);
            this.setupPickCamera(a, this.width / 2, this.height / 2);
            this.levelRenderer.pick(this.player, Frustum.GetFrustum());
            int hits = GL.RenderMode((RenderingMode)7168);
            //this.selectBuffer.Reverse();
            int[] names = new int[10];
            HitResult bestResult = null;
            int ind = 0;
            for (int i = 0; i < hits; ++i)
            {
                
                int nameCount = selectBuffer[ind];
                ind++;
                ind++;
                ind++;

                for (int j = 0; j < nameCount; ++j)
                {
                    names[j] = selectBuffer[ind];
                    ind++;
                }

                HitResult hitResult = new HitResult(names[0], names[1], names[2], names[3], names[4]);
                if (bestResult == null || hitResult.isCloserThan(player, bestResult, editMode))
                {
                    bestResult = hitResult;
                }
            }

            this.hitResult = bestResult;
        }

        public void render(float a)
        {
            GL.Viewport(0, 0, this.width, this.height);
            if (this.mouseGrabbed)
            {
                float xo = 0.0F;
                float yo = 0.0F;
                if (_firstMove)
                {
                    _lastPos = new Vector2(MouseState.X, MouseState.Y);
                    _firstMove = false;
                }
                else
                {
                    // Calculate the offset of the mouse position
                    xo = MouseState.X - _lastPos.X;
                    yo = MouseState.Y - _lastPos.Y;
                    _lastPos = new Vector2(MouseState.X, MouseState.Y);
                }

                //Rotate Camera
                this.player.turn(xo, yo * (float)this.yMouseAxis);
            }
            this.checkGlError("Set viewport");

            //Pick
            this.pick(a);
            this.checkGlError("Picked");

            //Camera
            GL.Clear(ClearBufferMask.ColorBufferBit);
            this.setupCamera(a);
            this.checkGlError("Set up camera");

            //Upadate Chunks
            GL.Enable(EnableCap.CullFace);
            Frustum frustum = Frustum.GetFrustum();
            this.levelRenderer.cull(frustum);
            this.levelRenderer.UpdateDirtyChunks(this.player);
            this.checkGlError("Update chunks");

            //Render the Level
            this.setupFog(0);
            GL.Enable(EnableCap.Fog);
            this.levelRenderer.render(this.player, 0);
            this.checkGlError("Rendered level");

            //Render Mobs
            Entity zombie;
            int i;
            for (i = 0; i < this.entities.Count; ++i)
            {
                zombie = (Entity)this.entities[i];
                if (zombie.isLit() && frustum.isVisible(zombie.bb))
                {
                    ((Entity)this.entities[i]).render(a);
                }
            }
            this.checkGlError("Rendered entities");

            //Render Particles
            this.particleEngine.render(this.player, a, 0);
            this.checkGlError("Rendered particles");

            //Fog
            this.setupFog(1);
            this.levelRenderer.render(this.player, 1);

            for (i = 0; i < this.entities.Count; ++i)
            {
                zombie = (Entity)this.entities[i];
                if (!zombie.isLit() && frustum.isVisible(zombie.bb))
                {
                    ((Entity)this.entities[i]).render(a);
                }
            }

            this.particleEngine.render(this.player, a, 1);
            this.levelRenderer.renderSurroundingGround();
            if (this.hitResult != null)
            {
                GL.Disable(EnableCap.Lighting);
                GL.Disable(EnableCap.AlphaTest);
                this.levelRenderer.renderHit(this.player, this.hitResult, this.editMode, this.paintTexture);
                this.levelRenderer.renderHitOutline(this.player, this.hitResult, this.editMode, this.paintTexture);
                GL.Enable(EnableCap.AlphaTest);
                GL.Enable(EnableCap.Lighting);
            }

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            this.setupFog(0);
            this.levelRenderer.renderSurroundingWater();
            GL.Enable(EnableCap.Blend);
            GL.ColorMask(false, false, false, false);
            this.levelRenderer.render(this.player, 2);
            GL.ColorMask(true, true, true, true);
            this.levelRenderer.render(this.player, 2);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Fog);
            if (this.hitResult != null)
            {
                GL.DepthFunc(DepthFunction.Less);
                GL.Disable(EnableCap.AlphaTest);
                this.levelRenderer.renderHit(this.player, this.hitResult, this.editMode, this.paintTexture);
                this.levelRenderer.renderHitOutline(this.player, this.hitResult, this.editMode, this.paintTexture);
                GL.Enable(EnableCap.AlphaTest);
                GL.DepthFunc(DepthFunction.Lequal);
            }
            this.checkGlError("Rendered Fog");

            //GUi
            this.drawGui(a);
            this.checkGlError("Rendered gui");
            SwapBuffers();
        }

        //Draw Gui
        private void drawGui(float a)
        {
            int screenWidth = this.width * 240 / this.height;
            int screenHeight = this.height * 240 / this.height;
            int xMouse = (int)MouseState.X * screenWidth / this.width;
            int yMouse = 240 - (screenHeight - (int)MouseState.Y * screenHeight / this.height - 1);
            Tesselator t = Tesselator.instance;

            GL.Clear(ClearBufferMask.DepthBufferBit);
            //Setup Matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0D, (double)screenWidth, (double)screenHeight, 0.0D, 100.0D, 300.0D);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            //Tranlate and rotate Selected Tile
            GL.Translate(0.0F, 0.0F, -200.0F);
            this.checkGlError("GUI: Init");
            GL.PushMatrix();
            GL.Translate((float)(screenWidth - 16), 16.0F, -50.0F);
            GL.Scale(16.0F, 16.0F, 16.0F);
            GL.Rotate(-30.0F, 1.0F, 0.0F, 0.0F);
            GL.Rotate(45.0F, 0.0F, 1.0F, 0.0F);
            GL.Translate(-1.5F, 0.5F, 0.5F);
            GL.Scale(-1.0F, -1.0F, -1.0F);

            //Bind Texture
            GL.BindTexture(TextureTarget.Texture2D, terrId);
            GL.Enable(EnableCap.Texture2D);

            //Draw 
            t.begin();
            Tile.tiles[this.paintTexture].render(t, this.level, 0, -2, 0, 0);
            t.end();
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
            this.checkGlError("GUI: Draw selected");

            //Draw Text
            this.font.drawShadow("0.0.13a", 2, 2, 16777215);
            this.font.drawShadow(this.fpsString, 2, 12, 16777215);
            this.checkGlError("GUI: Draw text");

            //Draw CrossHair
            int wc = screenWidth / 2;
            int hc = screenHeight / 2;
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            t.begin();
            t.vertex((float)(wc + 1), (float)(hc - 4), 0.0F);
            t.vertex((float)(wc - 0), (float)(hc - 4), 0.0F);
            t.vertex((float)(wc - 0), (float)(hc + 5), 0.0F);
            t.vertex((float)(wc + 1), (float)(hc + 5), 0.0F);
            t.vertex((float)(wc + 5), (float)(hc - 0), 0.0F);
            t.vertex((float)(wc - 4), (float)(hc - 0), 0.0F);
            t.vertex((float)(wc - 4), (float)(hc + 1), 0.0F);
            t.vertex((float)(wc + 5), (float)(hc + 1), 0.0F);
            t.end();
            this.checkGlError("GUI: Draw crosshair");
            if (this.screen != null)
            {
                this.screen.render(xMouse, yMouse);
            }

        }

        //Setup Fog GL Modes
        private void setupFog(int i)
        {
            Tile currentTile = Tile.tiles[this.level.getTile((int)this.player.x, (int)(this.player.y + 0.12F), (int)this.player.z)];
            if (currentTile != null && currentTile.getLiquidType() == 1)
            {
                GL.Fog(FogParameter.FogMode, 2048);
                GL.Fog(FogParameter.FogDensity, 0.1F);
                GL.Fog(FogParameter.FogColor, this.getBuffer(0.02F, 0.02F, 0.2F, 1.0F));
                GL.LightModel(LightModelParameter.LightModelAmbient, this.getBuffer(0.3F, 0.3F, 0.7F, 1.0F));
            }
            else if (currentTile != null && currentTile.getLiquidType() == 2)
            {
                GL.Fog(FogParameter.FogMode, 2048);
                GL.Fog(FogParameter.FogDensity, 2.0F);
                GL.Fog(FogParameter.FogColor, this.getBuffer(0.6F, 0.1F, 0.0F, 1.0F));
                GL.LightModel(LightModelParameter.LightModelAmbient, this.getBuffer(0.4F, 0.3F, 0.3F, 1.0F));
            }
            else if (i == 0)
            {
                GL.Fog(FogParameter.FogMode, 2048);
                GL.Fog(FogParameter.FogDensity, 0.001F);
                GL.Fog(FogParameter.FogColor, this.fogColor0);
                GL.LightModel(LightModelParameter.LightModelAmbient, this.getBuffer(1.0F, 1.0F, 1.0F, 1.0F));
            }
            else if (i == 1)
            {
                GL.Fog(FogParameter.FogMode, 2048);
                GL.Fog(FogParameter.FogDensity, 0.01F);
                GL.Fog(FogParameter.FogColor, this.fogColor1);
                float br = 0.6F;
                GL.LightModel(LightModelParameter.LightModelAmbient, this.getBuffer(br, br, br, 1.0F));
            }

            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.Ambient);
            GL.Enable(EnableCap.Lighting);
        }

        private float[] getBuffer(float a, float b, float c, float d)
        {
            Array.Clear(lb, 0, lb.Length);
            lb[0] = a;
            lb[1] = b;
            lb[2] = c;
            lb[3] = d;
            this.lb.Reverse();
            return this.lb;
        }


        //Init Loading Screen
        public void beginLevelLoading(String title)
        {
            this.title = title;
            int screenWidth = this.width * 240 / this.height;
            int screenHeight = this.height * 240 / this.height;
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0D, (double)screenWidth, (double)screenHeight, 0.0D, 100.0D, 300.0D);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0.0F, 0.0F, -200.0F);
        }

        //Render Loading Screen
        public void levelLoadUpdate(String status)
        {
            int screenWidth = this.width * 240 / this.height;
            int screenHeight = this.height * 240 / this.height;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Tesselator t = Tesselator.instance;
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, dirtId);
            t.begin();
            t.color(0x808080);
            float s = 32.0F;
            t.vertexUV(0.0F, (float)screenHeight, 0.0F, 0.0F, (float)screenHeight / s);
            t.vertexUV((float)screenWidth, (float)screenHeight, 0.0F, (float)screenWidth / s, (float)screenHeight / s);
            t.vertexUV((float)screenWidth, 0.0F, 0.0F, (float)screenWidth / s, 0.0F);
            t.vertexUV(0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
            t.end();
            GL.Enable(EnableCap.Texture2D);
            this.font.drawShadow(this.title, (screenWidth - this.font.width(this.title)) / 2, screenHeight / 2 - 4 - 8, 16777215);
            this.font.drawShadow(status, (screenWidth - this.font.width(status)) / 2, screenHeight / 2 - 4 + 4, 16777215);
            SwapBuffers();
        }

        //Gen a new Level
        public void generateNewLevel()
        {
            this.levelGen.generateLevel(this.level, this.user.name, 32, 512, 64);
            this.player.ResetPos();

            for (int i = 0; i < this.entities.Count; ++i)
            {
                this.entities.RemoveAt(i--);
            }

        }


    }

}

