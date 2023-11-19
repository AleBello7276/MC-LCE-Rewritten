using System.Drawing;
using System.IO;

using net.minecraft.src;
using net.minecraft.src.blocks.Block;
using net.minecraft.src.entity.EntityClientPlayerMP;
using net.minecraft.src.entity.EntityLiving;
using net.minecraft.src.entity.EntityPlayer;
using net.minecraft.src.entity.EntityPlayerSP;
using net.minecraft.src.entity.EntityRenderer;
using net.minecraft.src.gui.GuiAchievement;
using net.minecraft.src.gui.GuiChat;
using net.minecraft.src.gui.GuiConflictWarning;
using net.minecraft.src.gui.GuiConnecting;
using net.minecraft.src.gui.GuiErrorScreen;
using net.minecraft.src.gui.GuiGameOver;
using net.minecraft.src.gui.GuiIngame;
using net.minecraft.src.gui.GuiIngameMenu;
using net.minecraft.src.gui.GuiInventory;
using net.minecraft.src.gui.GuiMainMenu;
using net.minecraft.src.gui.GuiScreen;
using net.minecraft.src.gui.GuiSleepMP;
using net.minecraft.src.gui.GuiUnused;
using net.minecraft.src.item.ItemRenderer;
using net.minecraft.src.item.ItemStack;
using net.minecraft.src.model.ModelBiped;
using net.minecraft.src.net.NetClientHandler;
using net.minecraft.src.renderers.EffectRenderer;
using net.minecraft.src.renderers.RenderBlocks;
using net.minecraft.src.renderers.RenderEngine;
using net.minecraft.src.renderers.RenderGlobal;
using net.minecraft.src.renderers.RenderManager;
using net.minecraft.src.renderers.Tessellator;
using net.minecraft.src.textureFx.TextureCompassFX;
using net.minecraft.src.textureFx.TextureFlamesFX;
using net.minecraft.src.textureFx.TextureLavaFX;
using net.minecraft.src.textureFx.TextureLavaFlowFX;
using net.minecraft.src.textureFx.TexturePackList;
using net.minecraft.src.textureFx.TexturePortalFX;
using net.minecraft.src.textureFx.TextureWatchFX;
using net.minecraft.src.textureFx.TextureWaterFX;
using net.minecraft.src.textureFx.TextureWaterFlowFX;
using net.minecraft.src.world.World;
using net.minecraft.src.world.WorldProvider;
using net.minecraft.src.world.WorldRenderer;
using net.minecraft.src.world.chunk.ChunkCoordinates;
using net.minecraft.src.world.chunk.ChunkProviderLoadOrGenerate;
using net.minecraft.src.world.chunk.region.SaveConverterMcRegion;




using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace net.minecraft.client 
{
	public class Minecraft : GameWindow
	{
		public static byte[] field_28006_b = new byte[10485760];
		private static Minecraft theMinecraft;
		public PlayerController playerController;
		private bool fullscreen = false;
		private bool field_28004_R = false;
		public int displayWidth;
		public int displayHeight;
		private OpenGlCapsChecker glCapabilities;
		private Timer timer = new Timer(20.0F);
		public World theWorld;
		public RenderGlobal renderGlobal;
		public EntityPlayerSP thePlayer;
		public EntityLiving renderViewEntity;
		public EffectRenderer effectRenderer;
		public Session session = null;
		public String minecraftUri;
		public Canvas mcCanvas;
		public bool hideQuitButton = true;
		public volatile bool isGamePaused = false;
		public RenderEngine renderEngine;
		public FontRenderer fontRenderer;
		public GuiScreen currentScreen = null;
		public LoadingScreenRenderer loadingScreen = new LoadingScreenRenderer(this);
		public EntityRenderer entityRenderer;
		private ThreadDownloadResources downloadResourcesThread;
		private int ticksRan = 0;
		private int leftClickCounter = 0;
		private int tempDisplayWidth;
		private int tempDisplayHeight;
		public GuiAchievement guiAchievement = new GuiAchievement(this);
		public GuiIngame ingameGUI;
		public boolean skipRenderWorld = false;
		public ModelBiped field_9242_w = new ModelBiped(0.0F);
		public MovingObjectPosition objectMouseOver = null;
		public GameSettings gameSettings;
		public SoundManager sndManager = new SoundManager();
		public MouseHelper mouseHelper;
		public TexturePackList texturePackList;
		private File mcDataDir;
		private ISaveFormat saveLoader;
		public static long[] frameTimes = new long[512];
		public static long[] tickTimes = new long[512];
		public static int numRecordedFrameTimes = 0;
		public static long hasPaidCheckTime = 0L;
		public StatFileWriter statFileWriter;
		private String serverName;
		private int serverPort;
		private TextureWaterFX textureWaterFX = new TextureWaterFX();
		private TextureLavaFX textureLavaFX = new TextureLavaFX();
		private static DirectoryInfo? minecraftDir = null;
		public volatile bool running = true;
		public String debug = "";
		bool isTakingScreenshot = false;
		long prevFrameTime = -1L;
		public bool inGameHasFocus = false;
		private int mouseTicksRan = 0;
		public bool isRaining = false;
		long systemTime = System.currentTimeMillis();
		private int joinPlayerCounter = 0;


		public Minecraft(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            StatList.func_27360_a();
			this.tempDisplayHeight = var5;
			this.fullscreen = var6;
			new ThreadSleepForever(this, "Timer hack thread");
			this.mcCanvas = var2;
			this.displayWidth = var4;
			this.displayHeight = var5;
			this.fullscreen = var6;
			

			theMinecraft = this;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
			startGame();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
			run();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
			shutdown();
        }


        public void func_28003_b(UnexpectedThrowable var1) {
			this.field_28004_R = true;
			this.displayUnexpectedThrowable(var1);
		}

		public abstract void displayUnexpectedThrowable(UnexpectedThrowable var1);

		public void setServer(String var1, int var2) {
			this.serverName = var1;
			this.serverPort = var2;
		}

		public void startGame()
		{
			if(this.mcCanvas != null) {
				Graphics var1 = this.mcCanvas.getGraphics();
				
				//?? forse... boh poi sistemare!
				GL.ClearColor(Color.Black);

				
			} else if(this.fullscreen) {
				Display.setFullscreen(true);
				this.displayWidth = ClientSize.X;
				this.displayHeight = ClientSize.Y;
				if(this.displayWidth <= 0) {
					this.displayWidth = 1;
				}

				if(this.displayHeight <= 0) {
					this.displayHeight = 1;
				}
			} else {
				//ClientSize = new Vector2i(this.displayWidth, this.displayHeight);
			}


			this.mcDataDir = getMinecraftDir();
			this.saveLoader = new SaveConverterMcRegion(new File(this.mcDataDir, "saves"));
			this.gameSettings = new GameSettings(this, this.mcDataDir);
			this.texturePackList = new TexturePackList(this, this.mcDataDir);
			this.renderEngine = new RenderEngine(this.texturePackList, this.gameSettings);
			this.fontRenderer = new FontRenderer(this.gameSettings, "/font/default.png", this.renderEngine);
			ColorizerWater.func_28182_a(this.renderEngine.func_28149_a("/misc/watercolor.png"));
			ColorizerGrass.func_28181_a(this.renderEngine.func_28149_a("/misc/grasscolor.png"));
			ColorizerFoliage.func_28152_a(this.renderEngine.func_28149_a("/misc/foliagecolor.png"));
			this.entityRenderer = new EntityRenderer(this);
			RenderManager.instance.itemRenderer = new ItemRenderer(this);
			this.statFileWriter = new StatFileWriter(this.session, this.mcDataDir);
			AchievementList.openInventory.setStatStringFormatter(new StatStringFormatKeyInv(this));
			this.loadScreen();
			Keyboard.create();
			Mouse.create();
			this.mouseHelper = new MouseHelper(this.mcCanvas);

			/*try {
				Controllers.create();
			} catch (Exception var4) {
				var4.printStackTrace();
			}*/

			this.checkGLError("Pre startup");
			GL.Enable(EnableCap.Texture2D);
			GL.ShadeModel(ShadingModel.Smooth);
			GL.ClearDepth(1.0D);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.Enable(EnableCap.AlphaTest);
			GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
			GL.CullFace(CullFaceMode.Back);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Modelview);
			this.checkGLError("Startup");
			this.glCapabilities = new OpenGlCapsChecker();
			this.sndManager.loadSoundSettings(this.gameSettings);
			this.renderEngine.registerTextureFX(this.textureLavaFX);
			this.renderEngine.registerTextureFX(this.textureWaterFX);
			this.renderEngine.registerTextureFX(new TexturePortalFX());
			this.renderEngine.registerTextureFX(new TextureCompassFX(this));
			this.renderEngine.registerTextureFX(new TextureWatchFX(this));
			this.renderEngine.registerTextureFX(new TextureWaterFlowFX());
			this.renderEngine.registerTextureFX(new TextureLavaFlowFX());
			this.renderEngine.registerTextureFX(new TextureFlamesFX(0));
			this.renderEngine.registerTextureFX(new TextureFlamesFX(1));
			this.renderGlobal = new RenderGlobal(this, this.renderEngine);
			GL.Viewport(0, 0, this.displayWidth, this.displayHeight);
			this.effectRenderer = new EffectRenderer(this.theWorld, this.renderEngine);

			try {
				this.downloadResourcesThread = new ThreadDownloadResources(this.mcDataDir, this);
				this.downloadResourcesThread.start();
			} catch (Exception var3) {
			}

			this.checkGLError("Post startup");
			this.ingameGUI = new GuiIngame(this);
			if(this.serverName != null) {
				this.displayGuiScreen(new GuiConnecting(this, this.serverName, this.serverPort));
			} else {
				this.displayGuiScreen(new GuiMainMenu());
			}

		}

		private void loadScreen()  
		{
			ScaledResolution var1 = new ScaledResolution(this.gameSettings, this.displayWidth, this.displayHeight);
			GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0.0D, var1.field_25121_a, var1.field_25120_b, 0.0D, 1000.0D, 3000.0D);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Translate(0.0F, 0.0F, -2000.0F);
			GL.Viewport(0, 0, this.displayWidth, this.displayHeight);
			GL.ClearColor(0.0F, 0.0F, 0.0F, 0.0F);
			Tessellator var2 = Tessellator.instance;
			GL.Disable(EnableCap.Lighting);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.Fog);
			GL.BindTexture(TextureTarget.Texture2D, this.renderEngine.getTexture("/title/mojang.png"));
			var2.startDrawingQuads();
			var2.setColorOpaque_I(0xffffff);
			var2.addVertexWithUV(0.0D, (double)this.displayHeight, 0.0D, 0.0D, 0.0D);
			var2.addVertexWithUV((double)this.displayWidth, (double)this.displayHeight, 0.0D, 0.0D, 0.0D);
			var2.addVertexWithUV((double)this.displayWidth, 0.0D, 0.0D, 0.0D, 0.0D);
			var2.addVertexWithUV(0.0D, 0.0D, 0.0D, 0.0D, 0.0D);
			var2.draw();
			short var3 = 256;
			short var4 = 256;
			GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			var2.setColorOpaque_I(16777215);
			this.func_6274_a((var1.getScaledWidth() - var3) / 2, (var1.getScaledHeight() - var4) / 2, 0, 0, var3, var4);
			GL.Disable(EnableCap.Lighting);
			GL.Disable(EnableCap.Fog);
			GL.Enable(EnableCap.AlphaTest);
			GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
			SwapBuffers
		}

		public void func_6274_a(int var1, int var2, int var3, int var4, int var5, int var6) {
			float var7 = 0.00390625F;
			float var8 = 0.00390625F;
			Tessellator var9 = Tessellator.instance;
			var9.startDrawingQuads();
			var9.addVertexWithUV((double)(var1 + 0), (double)(var2 + var6), 0.0D, (double)((float)(var3 + 0) * var7), (double)((float)(var4 + var6) * var8));
			var9.addVertexWithUV((double)(var1 + var5), (double)(var2 + var6), 0.0D, (double)((float)(var3 + var5) * var7), (double)((float)(var4 + var6) * var8));
			var9.addVertexWithUV((double)(var1 + var5), (double)(var2 + 0), 0.0D, (double)((float)(var3 + var5) * var7), (double)((float)(var4 + 0) * var8));
			var9.addVertexWithUV((double)(var1 + 0), (double)(var2 + 0), 0.0D, (double)((float)(var3 + 0) * var7), (double)((float)(var4 + 0) * var8));
			var9.draw();
		}

		public static DirectoryInfo getMinecraftDir() {
			if(minecraftDir == null) {
				minecraftDir = getAppDir("minecraft");
			}

			return minecraftDir;
		}

		public static DirectoryInfo getAppDir(String var0) {
			String var1 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) ?? ".";
			DirectoryInfo  var2;
			switch(EnumOSMappingHelper.enumOSMappingArray[(int)getOs()]) 
			{
			case 1:
			case 2:
				var2 = new DirectoryInfo(Path.Combine(var1, '.' + var0));
				break;
			case 3:
				String var3 = Environment.GetEnvironmentVariable("APPDATA");
				if(var3 != null) {
					var2 = new DirectoryInfo(Path.Combine(var3, "." + var0));
				} else {
					var2 = new DirectoryInfo(Path.Combine(var1, '.' + var0));
				}
				break;
			case 4:
				var2 = new DirectoryInfo(Path.Combine(var1, "Library/Application Support/" + var0));
				break;
			default:
				var2 = new DirectoryInfo(Path.Combine(var1, var0));
				break;
			}

			if(!var2.Exists) 
			{
				try
				{
					var2.Create();
				}
				catch(Exception ex)
				{
					throw new InvalidOperationException($"The working directory could not be created: "+ var2, ex);
				}
			}

			return var2;
		}

		private static EnumOS2 getOs() {
			
			string var0 = Environment.OSVersion.Platform.ToString().ToLower();
			return var0.Contains("win") ? EnumOS2.windows : (var0.Contains("mac") ? EnumOS2.macos : (var0.Contains("solaris") ? EnumOS2.solaris : (var0.Contains("sunos") ? EnumOS2.solaris : (var0.Contains("linux") ? EnumOS2.linux : (var0.Contains("unix") ? EnumOS2.linux : EnumOS2.unknown)))));
		}

		public ISaveFormat getSaveLoader() {
			return this.saveLoader;
		}

		public void displayGuiScreen(GuiScreen var1) {
			if(!(this.currentScreen is GuiUnused)) {
				if(this.currentScreen != null) {
					this.currentScreen.onGuiClosed();
				}

				if(var1 is GuiMainMenu) {
					this.statFileWriter.func_27175_b();
				}

				this.statFileWriter.func_27182_c();
				if(var1 == null && this.theWorld == null) {
					var1 = new GuiMainMenu();
				} else if(var1 == null && this.thePlayer.health <= 0) {
					var1 = new GuiGameOver();
				}

				if(var1 is GuiMainMenu) {
					this.ingameGUI.func_28097_b();
				}

				this.currentScreen = (GuiScreen)var1;
				if(var1 != null) {
					this.setIngameNotInFocus();
					ScaledResolution var2 = new ScaledResolution(this.gameSettings, this.displayWidth, this.displayHeight);
					int var3 = var2.getScaledWidth();
					int var4 = var2.getScaledHeight();
					((GuiScreen)var1).setWorldAndResolution(this, var3, var4);
					this.skipRenderWorld = false;
				} else {
					this.setIngameFocus();
				}

			}
		}

		private void checkGLError(String var1) {
			int var2 = (int)GL.GetError();
			if(var2 != 0) {
				String var3 = GLU.gluErrorString(var2);
				Console.WriteLine("########## GL ERROR ##########");
				Console.WriteLine("@ " + var1);
				Console.WriteLine(var2 + ": " + var3);
			}

		}

		

		public void run() {
			this.running = true;

			try {
				//this.startGame();
			} catch (Exception var17) {
				this.func_28003_b(new UnexpectedThrowable("Failed to start game", var17));
				return;
			}

			try {
				long var1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				int var3 = 0;

				while(this.running) {
					try {
						

						AxisAlignedBB.clearBoundingBoxPool();
						Vec3D.initialize();
						
						// The Pause freeze the game ?
						if(this.isGamePaused && this.theWorld != null) {
							float var4 = this.timer.renderPartialTicks;
							this.timer.updateTimer();
							this.timer.renderPartialTicks = var4;
						} else {
							this.timer.updateTimer();
						}

						long var23 = DateTime.Now.Ticks * 100;

						for(int var6 = 0; var6 < this.timer.elapsedTicks; ++var6) {
							++this.ticksRan;

							try {
								this.runTick();
							} catch (MinecraftException var16) {
								this.theWorld = null;
								this.changeWorld1((World)null);
								this.displayGuiScreen(new GuiConflictWarning());
							}
						}

						long var24 = (DateTime.Now.Ticks * 100) - var23;
						this.checkGLError("Pre render");
						RenderBlocks.fancyGrass = this.gameSettings.fancyGraphics;
						this.sndManager.func_338_a(this.thePlayer, this.timer.renderPartialTicks);
						GL.Enable(EnableCap.Texture2D);
						if(this.theWorld != null) {
							this.theWorld.updatingLighting();
						}

						

						if(this.thePlayer != null && this.thePlayer.isEntityInsideOpaqueBlock()) {
							this.gameSettings.thirdPersonView = false;
						}

						if(!this.skipRenderWorld) {
							if(this.playerController != null) {
								this.playerController.setPartialTime(this.timer.renderPartialTicks);
							}

							this.entityRenderer.updateCameraAndRender(this.timer.renderPartialTicks);
						}

						/*if(!Display.isActive()) {
							if(this.fullscreen) {
								this.toggleFullscreen();
							}

							Thread.sleep(10L);
						}*/
						

						if(this.gameSettings.showDebugInfo) {
							this.displayDebugInfo(var24);
						} else {
							this.prevFrameTime = (DateTime.Now.Ticks * 100);
						}

						this.guiAchievement.updateAchievementWindow();
						Thread.Yield();
						

						this.screenshotListener();
						if(this.mcCanvas != null && !this.fullscreen && (this.mcCanvas.getWidth() != this.displayWidth || this.mcCanvas.getHeight() != this.displayHeight)) {
							this.displayWidth = this.mcCanvas.getWidth();
							this.displayHeight = this.mcCanvas.getHeight();
							if(this.displayWidth <= 0) {
								this.displayWidth = 1;
							}

							if(this.displayHeight <= 0) {
								this.displayHeight = 1;
							}

							this.resize(this.displayWidth, this.displayHeight);
						}

						this.checkGLError("Post render");
						++var3;

						for(this.isGamePaused = !this.isMultiplayerWorld() && this.currentScreen != null && this.currentScreen.doesGuiPauseGame(); System.currentTimeMillis() >= var1 + 1000L; var3 = 0) {
							this.debug = var3 + " fps, " + WorldRenderer.chunksUpdated + " chunk updates";
							WorldRenderer.chunksUpdated = 0;
							var1 += 1000L;
						}
					} catch (MinecraftException var18) {
						this.theWorld = null;
						this.changeWorld1((World)null);
						this.displayGuiScreen(new GuiConflictWarning());
					} catch (OutOfMemoryError var19) {
						this.func_28002_e();
						this.displayGuiScreen(new GuiErrorScreen());
						GC.Collect();
					}
				}
			} catch (MinecraftError var20) {
			} catch (Throwable var21) {
				this.func_28002_e();
				var21.printStackTrace();
				this.func_28003_b(new UnexpectedThrowable("Unexpected error", var21));
			} finally {
				this.shutdownMinecraftApplet();
			}

		}

		public void func_28002_e() {
			try {
				field_28006_b = new byte[0];
				this.renderGlobal.func_28137_f();
			} catch (Throwable var4) {
			}

			try {
				GC.Collect();
				AxisAlignedBB.func_28196_a();
				Vec3D.func_28215_a();
			} catch (Throwable var3) {
			}

			try {
				GC.Collect();
				this.changeWorld1((World)null);
			} catch (Throwable var2) {
			}

			GC.Collect();
		}

		private void screenshotListener() {
			if(KeyboardState.IsKeyDown(Keys.F2)) {
				if(!this.isTakingScreenshot) {
					this.isTakingScreenshot = true;
					this.ingameGUI.addChatMessage(ScreenShotHelper.saveScreenshot(minecraftDir, this.displayWidth, this.displayHeight));
				}
			} else {
				this.isTakingScreenshot = false;
			}

		}

		private void displayDebugInfo(long var1) {
			long var3 = 16666666L;
			if(this.prevFrameTime == -1L) {
				this.prevFrameTime = (DateTime.Now.Ticks * 100);
			}

			long var5 = (DateTime.Now.Ticks * 100);
			tickTimes[numRecordedFrameTimes & frameTimes.Length - 1] = var1;
			frameTimes[numRecordedFrameTimes++ & frameTimes.Length - 1] = var5 - this.prevFrameTime;
			this.prevFrameTime = var5;
			GL.Clear(ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0.0D, (double)this.displayWidth, (double)this.displayHeight, 0.0D, 1000.0D, 3000.0D);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Translate(0.0F, 0.0F, -2000.0F);
			GL.LineWidth(1.0F);
			GL.Disable(EnableCap.Texture2D);
			Tessellator var7 = Tessellator.instance;
			var7.startDrawing(7);
			int var8 = (int)(var3 / 200000L);
			var7.setColorOpaque_I(536870912);
			var7.addVertex(0.0D, (double)(this.displayHeight - var8), 0.0D);
			var7.addVertex(0.0D, (double)this.displayHeight, 0.0D);
			var7.addVertex((double)frameTimes.Length, (double)this.displayHeight, 0.0D);
			var7.addVertex((double)frameTimes.Length, (double)(this.displayHeight - var8), 0.0D);
			var7.setColorOpaque_I(538968064);
			var7.addVertex(0.0D, (double)(this.displayHeight - var8 * 2), 0.0D);
			var7.addVertex(0.0D, (double)(this.displayHeight - var8), 0.0D);
			var7.addVertex((double)frameTimes.Length, (double)(this.displayHeight - var8), 0.0D);
			var7.addVertex((double)frameTimes.Length, (double)(this.displayHeight - var8 * 2), 0.0D);
			var7.draw();
			long var9 = 0L;

			int var11;
			for(var11 = 0; var11 < frameTimes.Length; ++var11) {
				var9 += frameTimes[var11];
			}

			var11 = (int)(var9 / 200000L / (long)frameTimes.Length);
			var7.startDrawing(7);
			var7.setColorOpaque_I(541065216);
			var7.addVertex(0.0D, (double)(this.displayHeight - var11), 0.0D);
			var7.addVertex(0.0D, (double)this.displayHeight, 0.0D);
			var7.addVertex((double)frameTimes.Length, (double)this.displayHeight, 0.0D);
			var7.addVertex((double)frameTimes.Length, (double)(this.displayHeight - var11), 0.0D);
			var7.draw();
			var7.startDrawing(1);

			for(int var12 = 0; var12 < frameTimes.Length; ++var12) {
				int var13 = (var12 - numRecordedFrameTimes & frameTimes.Length - 1) * 255 / frameTimes.Length;
				int var14 = var13 * var13 / 255;
				var14 = var14 * var14 / 255;
				int var15 = var14 * var14 / 255;
				var15 = var15 * var15 / 255;
				if(frameTimes[var12] > var3) {
					var7.setColorOpaque_I(-16777216 + var14 * 65536);
				} else {
					var7.setColorOpaque_I(-16777216 + var14 * 256);
				}

				long var16 = frameTimes[var12] / 200000L;
				long var18 = tickTimes[var12] / 200000L;
				var7.addVertex((double)((float)var12 + 0.5F), (double)((float)((long)this.displayHeight - var16) + 0.5F), 0.0D);
				var7.addVertex((double)((float)var12 + 0.5F), (double)((float)this.displayHeight + 0.5F), 0.0D);
				var7.setColorOpaque_I(-16777216 + var14 * 65536 + var14 * 256 + var14 * 1);
				var7.addVertex((double)((float)var12 + 0.5F), (double)((float)((long)this.displayHeight - var16) + 0.5F), 0.0D);
				var7.addVertex((double)((float)var12 + 0.5F), (double)((float)((long)this.displayHeight - (var16 - var18)) + 0.5F), 0.0D);
			}

			var7.draw();
			GL.Enable(EnableCap.Texture2D);
		}

		public void shutdown() {
			this.running = false;
		}

		public void setIngameFocus() {
			if(Display.isActive()) {
				if(!this.inGameHasFocus) {
					this.inGameHasFocus = true;
					this.mouseHelper.grabMouseCursor();
					this.displayGuiScreen((GuiScreen)null);
					this.leftClickCounter = 10000;
					this.mouseTicksRan = this.ticksRan + 10000;
				}
			}
		}

		public void setIngameNotInFocus() {
			if(this.inGameHasFocus) {
				if(this.thePlayer != null) {
					this.thePlayer.resetPlayerKeyState();
				}

				this.inGameHasFocus = false;
				this.mouseHelper.ungrabMouseCursor();
			}
		}

		public void displayInGameMenu() {
			if(this.currentScreen == null) {
				this.displayGuiScreen(new GuiIngameMenu());
			}
		}

		private void func_6254_a(int var1, bool var2) {
			if(!this.playerController.field_1064_b) {
				if(!var2) {
					this.leftClickCounter = 0;
				}

				if(var1 != 0 || this.leftClickCounter <= 0) {
					if(var2 && this.objectMouseOver != null && this.objectMouseOver.typeOfHit == EnumMovingObjectType.TILE && var1 == 0) {
						int var3 = this.objectMouseOver.blockX;
						int var4 = this.objectMouseOver.blockY;
						int var5 = this.objectMouseOver.blockZ;
						this.playerController.sendBlockRemoving(var3, var4, var5, this.objectMouseOver.sideHit);
						this.effectRenderer.addBlockHitEffects(var3, var4, var5, this.objectMouseOver.sideHit);
					} else {
						this.playerController.resetBlockRemoving();
					}

				}
			}
		}

		private void clickMouse(int var1) {
			if(var1 != 0 || this.leftClickCounter <= 0) {
				if(var1 == 0) {
					this.thePlayer.swingItem();
				}

				bool var2 = true;
				if(this.objectMouseOver == null) {
					if(var1 == 0 && !(this.playerController is PlayerControllerTest)) {
						this.leftClickCounter = 10;
					}
				} else if(this.objectMouseOver.typeOfHit == EnumMovingObjectType.ENTITY) {
					if(var1 == 0) {
						this.playerController.attackEntity(this.thePlayer, this.objectMouseOver.entityHit);
					}

					if(var1 == 1) {
						this.playerController.interactWithEntity(this.thePlayer, this.objectMouseOver.entityHit);
					}
				} else if(this.objectMouseOver.typeOfHit == EnumMovingObjectType.TILE) {
					int var3 = this.objectMouseOver.blockX;
					int var4 = this.objectMouseOver.blockY;
					int var5 = this.objectMouseOver.blockZ;
					int var6 = this.objectMouseOver.sideHit;
					if(var1 == 0) {
						this.playerController.clickBlock(var3, var4, var5, this.objectMouseOver.sideHit);
					} else {
						ItemStack var7 = this.thePlayer.inventory.getCurrentItem();
						int var8 = var7 != null ? var7.stackSize : 0;
						if(this.playerController.sendPlaceBlock(this.thePlayer, this.theWorld, var7, var3, var4, var5, var6)) {
							var2 = false;
							this.thePlayer.swingItem();
						}

						if(var7 == null) {
							return;
						}

						if(var7.stackSize == 0) {
							this.thePlayer.inventory.mainInventory[this.thePlayer.inventory.currentItem] = null;
						} else if(var7.stackSize != var8) {
							this.entityRenderer.itemRenderer.func_9449_b();
						}
					}
				}

				if(var2 && var1 == 1) {
					ItemStack var9 = this.thePlayer.inventory.getCurrentItem();
					if(var9 != null && this.playerController.sendUseItem(this.thePlayer, this.theWorld, var9)) {
						this.entityRenderer.itemRenderer.func_9450_c();
					}
				}

			}
		}

		public void toggleFullscreen() {
			try {
				this.fullscreen = !this.fullscreen;
				if(this.fullscreen) {
					Display.setDisplayMode(Display.getDesktopDisplayMode());
					this.displayWidth = Display.getDisplayMode().getWidth();
					this.displayHeight = Display.getDisplayMode().getHeight();
					if(this.displayWidth <= 0) {
						this.displayWidth = 1;
					}

					if(this.displayHeight <= 0) {
						this.displayHeight = 1;
					}
				} else {
					if(this.mcCanvas != null) {
						this.displayWidth = this.mcCanvas.getWidth();
						this.displayHeight = this.mcCanvas.getHeight();
					} else {
						this.displayWidth = this.tempDisplayWidth;
						this.displayHeight = this.tempDisplayHeight;
					}

					if(this.displayWidth <= 0) {
						this.displayWidth = 1;
					}

					if(this.displayHeight <= 0) {
						this.displayHeight = 1;
					}
				}

				if(this.currentScreen != null) {
					this.resize(this.displayWidth, this.displayHeight);
				}

				Display.setFullscreen(this.fullscreen);
				SwapBuffers();
			} catch (Exception var2) {
				var2.printStackTrace();
			}

		}

		private void resize(int var1, int var2) {
			if(var1 <= 0) {
				var1 = 1;
			}

			if(var2 <= 0) {
				var2 = 1;
			}

			this.displayWidth = var1;
			this.displayHeight = var2;
			if(this.currentScreen != null) {
				ScaledResolution var3 = new ScaledResolution(this.gameSettings, var1, var2);
				int var4 = var3.getScaledWidth();
				int var5 = var3.getScaledHeight();
				this.currentScreen.setWorldAndResolution(this, var4, var5);
			}

		}

		private void clickMiddleMouseButton() {
			if(this.objectMouseOver != null) {
				int var1 = this.theWorld.getBlockId(this.objectMouseOver.blockX, this.objectMouseOver.blockY, this.objectMouseOver.blockZ);
				if(var1 == Block.grass.blockID) {
					var1 = Block.dirt.blockID;
				}

				if(var1 == Block.stairDouble.blockID) {
					var1 = Block.stairSingle.blockID;
				}

				if(var1 == Block.bedrock.blockID) {
					var1 = Block.stone.blockID;
				}

				this.thePlayer.inventory.setCurrentItem(var1, this.playerController instanceof PlayerControllerTest);
			}

		}

		private void func_28001_B() {
			(new ThreadCheckHasPaid(this)).start();
		}

		public void runTick() {
			if(this.ticksRan == 6000) {
				this.func_28001_B();
			}

			this.statFileWriter.func_27178_d();
			this.ingameGUI.updateTick();
			this.entityRenderer.getMouseOver(1.0F);
			int var3;
			if(this.thePlayer != null) {
				IChunkProvider var1 = this.theWorld.getIChunkProvider();
				if(var1 is ChunkProviderLoadOrGenerate) {
					ChunkProviderLoadOrGenerate var2 = (ChunkProviderLoadOrGenerate)var1;
					var3 = MathHelper.floor_float((float)((int)this.thePlayer.posX)) >> 4;
					int var4 = MathHelper.floor_float((float)((int)this.thePlayer.posZ)) >> 4;
					var2.setCurrentChunkOver(var3, var4);
				}
			}

			if(!this.isGamePaused && this.theWorld != null) {
				this.playerController.updateController();
			}

			GL.BindTexture(TextureTarget.Texture2D, this.renderEngine.getTexture("/terrain.png"));
			if(!this.isGamePaused) {
				this.renderEngine.updateDynamicTextures();
			}

			if(this.currentScreen == null && this.thePlayer != null) {
				if(this.thePlayer.health <= 0) {
					this.displayGuiScreen((GuiScreen)null);
				} else if(this.thePlayer.isPlayerSleeping() && this.theWorld != null && this.theWorld.multiplayerWorld) {
					this.displayGuiScreen(new GuiSleepMP());
				}
			} else if(this.currentScreen != null && this.currentScreen is GuiSleepMP && !this.thePlayer.isPlayerSleeping()) {
				this.displayGuiScreen((GuiScreen)null);
			}

			if(this.currentScreen != null) {
				this.leftClickCounter = 10000;
				this.mouseTicksRan = this.ticksRan + 10000;
			}

			if(this.currentScreen != null) {
				this.currentScreen.handleInput();
				if(this.currentScreen != null) {
					this.currentScreen.field_25091_h.func_25088_a();
					this.currentScreen.updateScreen();
				}
			}

			if(this.currentScreen == null || this.currentScreen.field_948_f) {
				label301:
				while(true) {
					while(true) {
						while(true) {
							long var5;
							do {
								if(!MouseState.IsAnyButtonDown) {
									if(this.leftClickCounter > 0) {
										--this.leftClickCounter;
									}

									while(true) {
										while(true) {
											do {
												if(!KeyboardState.IsAnyKeyDown) {
													if(this.currentScreen == null) {
														if(MouseState.IsButtonDown(MouseButton.Left) && (float)(this.ticksRan - this.mouseTicksRan) >= this.timer.ticksPerSecond / 4.0F && this.inGameHasFocus) {
															this.clickMouse(0);
															this.mouseTicksRan = this.ticksRan;
														}

														if(MouseState.IsButtonDown(MouseButton.Right) && (float)(this.ticksRan - this.mouseTicksRan) >= this.timer.ticksPerSecond / 4.0F && this.inGameHasFocus) {
															this.clickMouse(1);
															this.mouseTicksRan = this.ticksRan;
														}
													}

													this.func_6254_a(0, this.currentScreen == null && MouseState.IsButtonDown(MouseButton.Left) && this.inGameHasFocus);
													goto label301;
												}

												this.thePlayer.handleKeyPress(Keyboard.getEventKey(), Keyboard.getEventKeyState());
											} while(!KeyboardState.IsAnyKeyDown);

											if(KeyboardState.IsKeyPressed(Keys.F11)) {
												this.toggleFullscreen();
											} else {
												if(this.currentScreen != null) {
													this.currentScreen.handleKeyboardInput();
												} else {
													if(KeyboardState.IsKeyPressed(Keys.Escape)) {
														this.displayInGameMenu();
													}

													if(KeyboardState.IsKeyPressed(Keys.S) && KeyboardState.IsKeyPressed(Keys.F3)) {
														this.forceReload();
													}

													if(KeyboardState.IsKeyPressed(Keys.F1)) {
														this.gameSettings.hideGUI = !this.gameSettings.hideGUI;
													}

													if(KeyboardState.IsKeyPressed(Keys.F3)) {
														this.gameSettings.showDebugInfo = !this.gameSettings.showDebugInfo;
													}

													if(KeyboardState.IsKeyPressed(Keys.F5)) {
														this.gameSettings.thirdPersonView = !this.gameSettings.thirdPersonView;
													}

													if(KeyboardState.IsKeyPressed(Keys.F8)) {
														this.gameSettings.smoothCamera = !this.gameSettings.smoothCamera;
													}

													if(KeyboardState.IsKeyPressed(this.gameSettings.keyBindInventory.keyCode)) {
														this.displayGuiScreen(new GuiInventory(this.thePlayer));
													}

													if(KeyboardState.IsKeyPressed(this.gameSettings.keyBindDrop.keyCode)) {
														this.thePlayer.dropCurrentItem();
													}

													if(this.isMultiplayerWorld() && KeyboardState.IsKeyPressed(this.gameSettings.keyBindChat.keyCode)) {
														this.displayGuiScreen(new GuiChat());
													}
												}

												for(int var6 = 0; var6 < 9; ++var6) {
													if(KeyboardState.IsKeyPressed(Keys.D1 + var6)) {
														this.thePlayer.inventory.currentItem = var6;
													}
												}

												if(KeyboardState.IsKeyPressed(this.gameSettings.keyBindToggleFog.keyCode)) {
													this.gameSettings.setOptionValue(EnumOptions.RENDER_DISTANCE, !KeyboardState.IsKeyDown(Keys.LeftShift) && !KeyboardState.IsKeyDown(Keys.RightShift) ? 1 : -1);
												}
											}
										}
									}
								}

								var5 = System.currentTimeMillis() - this.systemTime;
							} while(var5 > 200L);

							var3 = Mouse.getEventDWheel();
							if(var3 != 0) {
								this.thePlayer.inventory.changeCurrentItem(var3);
								if(this.gameSettings.field_22275_C) {
									if(var3 > 0) {
										var3 = 1;
									}

									if(var3 < 0) {
										var3 = -1;
									}

									this.gameSettings.field_22272_F += (float)var3 * 0.25F;
								}
							}

							if(this.currentScreen == null) {
								if(!this.inGameHasFocus && Mouse.getEventButtonState()) {
									this.setIngameFocus();
								} else {
									if(Mouse.getEventButton() == 0 && Mouse.getEventButtonState()) {
										this.clickMouse(0);
										this.mouseTicksRan = this.ticksRan;
									}

									if(Mouse.getEventButton() == 1 && Mouse.getEventButtonState()) {
										this.clickMouse(1);
										this.mouseTicksRan = this.ticksRan;
									}

									if(Mouse.getEventButton() == 2 && Mouse.getEventButtonState()) {
										this.clickMiddleMouseButton();
									}
								}
							} else if(this.currentScreen != null) {
								this.currentScreen.handleMouseInput();
							}
						}
					}
				}
			}

			if(this.theWorld != null) {
				if(this.thePlayer != null) {
					++this.joinPlayerCounter;
					if(this.joinPlayerCounter == 30) {
						this.joinPlayerCounter = 0;
						this.theWorld.joinEntityInSurroundings(this.thePlayer);
					}
				}

				this.theWorld.difficultySetting = this.gameSettings.difficulty;
				if(this.theWorld.multiplayerWorld) {
					this.theWorld.difficultySetting = 3;
				}

				if(!this.isGamePaused) {
					this.entityRenderer.updateRenderer();
				}

				if(!this.isGamePaused) {
					this.renderGlobal.updateClouds();
				}

				if(!this.isGamePaused) {
					if(this.theWorld.field_27172_i > 0) {
						--this.theWorld.field_27172_i;
					}

					this.theWorld.updateEntities();
				}

				if(!this.isGamePaused || this.isMultiplayerWorld()) {
					this.theWorld.setAllowedMobSpawns(this.gameSettings.difficulty > 0, true);
					this.theWorld.tick();
				}

				if(!this.isGamePaused && this.theWorld != null) {
					this.theWorld.randomDisplayUpdates(MathHelper.floor_double(this.thePlayer.posX), MathHelper.floor_double(this.thePlayer.posY), MathHelper.floor_double(this.thePlayer.posZ));
				}

				if(!this.isGamePaused) {
					this.effectRenderer.updateEffects();
				}
			}

			this.systemTime = System.currentTimeMillis();
		}

		private void forceReload() {
			Console.WriteLine("FORCING RELOAD!");
			this.sndManager = new SoundManager();
			this.sndManager.loadSoundSettings(this.gameSettings);
			this.downloadResourcesThread.reloadResources();
		}

		public bool isMultiplayerWorld() {
			return this.theWorld != null && this.theWorld.multiplayerWorld;
		}

		public void startWorld(String var1, String var2, long var3) {
			this.changeWorld1((World)null);
			GC.Collect();
			if(this.saveLoader.isOldMapFormat(var1)) {
				this.convertMapFormat(var1, var2);
			} else {
				ISaveHandler var5 = this.saveLoader.getSaveLoader(var1, false);
				World var6 = null;
				var6 = new World(var5, var2, var3);
				if(var6.isNewWorld) {
					this.statFileWriter.func_25100_a(StatList.createWorldStat, 1);
					this.statFileWriter.func_25100_a(StatList.startGameStat, 1);
					this.changeWorld2(var6, "Generating level");
				} else {
					this.statFileWriter.func_25100_a(StatList.loadWorldStat, 1);
					this.statFileWriter.func_25100_a(StatList.startGameStat, 1);
					this.changeWorld2(var6, "Loading level");
				}
			}

		}

		public void usePortal() {
			Console.WriteLine("Toggling dimension!!");
			if(this.thePlayer.dimension == -1) {
				this.thePlayer.dimension = 0;
			} else {
				this.thePlayer.dimension = -1;
			}

			this.theWorld.setEntityDead(this.thePlayer);
			this.thePlayer.isDead = false;
			double var1 = this.thePlayer.posX;
			double var3 = this.thePlayer.posZ;
			double var5 = 8.0D;
			World var7;
			if(this.thePlayer.dimension == -1) {
				var1 /= var5;
				var3 /= var5;
				this.thePlayer.setLocationAndAngles(var1, this.thePlayer.posY, var3, this.thePlayer.rotationYaw, this.thePlayer.rotationPitch);
				if(this.thePlayer.isEntityAlive()) {
					this.theWorld.updateEntityWithOptionalForce(this.thePlayer, false);
				}

				var7 = null;
				var7 = new World(this.theWorld, WorldProvider.getProviderForDimension(-1));
				this.changeWorld(var7, "Entering the Nether", this.thePlayer);
			} else {
				var1 *= var5;
				var3 *= var5;
				this.thePlayer.setLocationAndAngles(var1, this.thePlayer.posY, var3, this.thePlayer.rotationYaw, this.thePlayer.rotationPitch);
				if(this.thePlayer.isEntityAlive()) {
					this.theWorld.updateEntityWithOptionalForce(this.thePlayer, false);
				}

				var7 = null;
				var7 = new World(this.theWorld, WorldProvider.getProviderForDimension(0));
				this.changeWorld(var7, "Leaving the Nether", this.thePlayer);
			}

			this.thePlayer.worldObj = this.theWorld;
			if(this.thePlayer.isEntityAlive()) {
				this.thePlayer.setLocationAndAngles(var1, this.thePlayer.posY, var3, this.thePlayer.rotationYaw, this.thePlayer.rotationPitch);
				this.theWorld.updateEntityWithOptionalForce(this.thePlayer, false);
				(new Teleporter()).func_4107_a(this.theWorld, this.thePlayer);
			}

		}

		public void changeWorld1(World var1) {
			this.changeWorld2(var1, "");
		}

		public void changeWorld2(World var1, String var2) {
			this.changeWorld(var1, var2, (EntityPlayer)null);
		}

		public void changeWorld(World var1, String var2, EntityPlayer var3) 
		{
			this.statFileWriter.func_27175_b();
			this.statFileWriter.func_27182_c();
			this.renderViewEntity = null;
			this.loadingScreen.printText(var2);
			this.loadingScreen.displayLoadingString("");
			this.sndManager.playStreaming((String)null, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);

			if(this.theWorld != null) 
			{
				this.theWorld.func_651_a(this.loadingScreen);
			}

			this.theWorld = var1;

			if(var1 != null) 
			{
				this.playerController.func_717_a(var1);
				if(!this.isMultiplayerWorld()) {
					if(var3 == null) {
						this.thePlayer = (EntityPlayerSP)var1.func_4085_a(typeof(EntityPlayerSP));
					}
				} else if(this.thePlayer != null) {
					this.thePlayer.preparePlayerToSpawn();
					if(var1 != null) {
						var1.entityJoinedWorld(this.thePlayer);
					}
				}

				if(!var1.multiplayerWorld) {
					this.func_6255_d(var2);
				}

				if(this.thePlayer == null) {
					this.thePlayer = (EntityPlayerSP)this.playerController.createPlayer(var1);
					this.thePlayer.preparePlayerToSpawn();
					this.playerController.flipPlayer(this.thePlayer);
				}

				this.thePlayer.movementInput = new MovementInputFromOptions(this.gameSettings);
				if(this.renderGlobal != null) {
					this.renderGlobal.changeWorld(var1);
				}

				if(this.effectRenderer != null) {
					this.effectRenderer.clearEffects(var1);
				}

				this.playerController.func_6473_b(this.thePlayer);
				if(var3 != null) {
					var1.func_6464_c();
				}

				IChunkProvider var4 = var1.getIChunkProvider();
				if(var4 is ChunkProviderLoadOrGenerate) {
					ChunkProviderLoadOrGenerate var5 = (ChunkProviderLoadOrGenerate)var4;
					int var6 = MathHelper.floor_float((float)((int)this.thePlayer.posX)) >> 4;
					int var7 = MathHelper.floor_float((float)((int)this.thePlayer.posZ)) >> 4;
					var5.setCurrentChunkOver(var6, var7);
				}

				var1.spawnPlayerWithLoadedChunks(this.thePlayer);
				if(var1.isNewWorld) {
					var1.func_651_a(this.loadingScreen);
				}

				this.renderViewEntity = this.thePlayer;
			} else {
				this.thePlayer = null;
			}

			GC.Collect();
			this.systemTime = 0L;
		}

		private void convertMapFormat(String var1, String var2) {
			this.loadingScreen.printText("Converting World to " + this.saveLoader.func_22178_a());
			this.loadingScreen.displayLoadingString("This may take a while :)");
			this.saveLoader.convertMapFormat(var1, this.loadingScreen);
			this.startWorld(var1, var2, 0L);
		}

		private void func_6255_d(String var1) {
			this.loadingScreen.printText(var1);
			this.loadingScreen.displayLoadingString("Building terrain");
			short var2 = 128;
			int var3 = 0;
			int var4 = var2 * 2 / 16 + 1;
			var4 *= var4;
			IChunkProvider var5 = this.theWorld.getIChunkProvider();
			ChunkCoordinates var6 = this.theWorld.getSpawnPoint();
			if(this.thePlayer != null) {
				var6.x = (int)this.thePlayer.posX;
				var6.z = (int)this.thePlayer.posZ;
			}

			if(var5 is ChunkProviderLoadOrGenerate) {
				ChunkProviderLoadOrGenerate var7 = (ChunkProviderLoadOrGenerate)var5;
				var7.setCurrentChunkOver(var6.x >> 4, var6.z >> 4);
			}

			for(int var10 = -var2; var10 <= var2; var10 += 16) {
				for(int var8 = -var2; var8 <= var2; var8 += 16) {
					this.loadingScreen.setLoadingProgress(var3++ * 100 / var4);
					this.theWorld.getBlockId(var6.x + var10, 64, var6.z + var8);

					while(this.theWorld.updatingLighting()) {
					}
				}
			}

			this.loadingScreen.displayLoadingString("Simulating world for a bit");
			bool var9 = true;
			this.theWorld.func_656_j();
		}

		public void installResource(string var1, FileInfo var2)
		{
			int var3 = var1.IndexOf("/");
			string var4 = var1.Substring(0, var3);
			var1 = var1.Substring(var3 + 1);
			
			if (string.Equals(var4, "sound", StringComparison.OrdinalIgnoreCase))
			{
				this.sndManager.AddSound(var1, var2);
			}
			else if (string.Equals(var4, "newsound", StringComparison.OrdinalIgnoreCase))
			{
				this.sndManager.AddSound(var1, var2);
			}
			else if (string.Equals(var4, "streaming", StringComparison.OrdinalIgnoreCase))
			{
				this.sndManager.AddStreaming(var1, var2);
			}
			else if (string.Equals(var4, "music", StringComparison.OrdinalIgnoreCase))
			{
				this.sndManager.AddMusic(var1, var2);
			}
			else if (string.Equals(var4, "newmusic", StringComparison.OrdinalIgnoreCase))
			{
				this.sndManager.AddMusic(var1, var2);
			}
		}


		public OpenGlCapsChecker getOpenGlCapsChecker() {
			return this.glCapabilities;
		}

		public String func_6241_m() {
			return this.renderGlobal.getDebugInfoRenders();
		}

		public String func_6262_n() {
			return this.renderGlobal.getDebugInfoEntities();
		}

		public String func_21002_o() {
			return this.theWorld.func_21119_g();
		}

		public String func_6245_o() {
			return "P: " + this.effectRenderer.getStatistics() + ". T: " + this.theWorld.func_687_d();
		}

		public void respawn(bool var1, int var2) {
			if(!this.theWorld.multiplayerWorld && !this.theWorld.worldProvider.canRespawnHere()) {
				this.usePortal();
			}

			ChunkCoordinates var3 = null;
			ChunkCoordinates var4 = null;
			bool var5 = true;
			if(this.thePlayer != null && !var1) {
				var3 = this.thePlayer.getPlayerSpawnCoordinate();
				if(var3 != null) {
					var4 = EntityPlayer.func_25060_a(this.theWorld, var3);
					if(var4 == null) {
						this.thePlayer.addChatMessage("tile.bed.notValid");
					}
				}
			}

			if(var4 == null) {
				var4 = this.theWorld.getSpawnPoint();
				var5 = false;
			}

			IChunkProvider var6 = this.theWorld.getIChunkProvider();
			if(var6 is ChunkProviderLoadOrGenerate) {
				ChunkProviderLoadOrGenerate var7 = (ChunkProviderLoadOrGenerate)var6;
				var7.setCurrentChunkOver(var4.x >> 4, var4.z >> 4);
			}

			this.theWorld.setSpawnLocation();
			this.theWorld.updateEntityList();
			int var8 = 0;
			if(this.thePlayer != null) {
				var8 = this.thePlayer.entityId;
				this.theWorld.setEntityDead(this.thePlayer);
			}

			this.renderViewEntity = null;
			this.thePlayer = (EntityPlayerSP)this.playerController.createPlayer(this.theWorld);
			this.thePlayer.dimension = var2;
			this.renderViewEntity = this.thePlayer;
			this.thePlayer.preparePlayerToSpawn();
			if(var5) {
				this.thePlayer.setPlayerSpawnCoordinate(var3);
				this.thePlayer.setLocationAndAngles((double)((float)var4.x + 0.5F), (double)((float)var4.y + 0.1F), (double)((float)var4.z + 0.5F), 0.0F, 0.0F);
			}

			this.playerController.flipPlayer(this.thePlayer);
			this.theWorld.spawnPlayerWithLoadedChunks(this.thePlayer);
			this.thePlayer.movementInput = new MovementInputFromOptions(this.gameSettings);
			this.thePlayer.entityId = var8;
			this.thePlayer.func_6420_o();
			this.playerController.func_6473_b(this.thePlayer);
			this.func_6255_d("Respawning");
			if(this.currentScreen is GuiGameOver) {
				this.displayGuiScreen((GuiScreen)null);
			}

		}

		public static void func_6269_a(String var0, String var1) {
			startMainThread(var0, var1, (String)null);
		}

		public static void startMainThread(String var0, String var1, String var2) {
			boolean var3 = false;
			Frame var5 = new Frame("Minecraft");
			Canvas var6 = new Canvas();
			var5.setLayout(new BorderLayout());
			var5.add(var6, "Center");
			var6.setPreferredSize(new Dimension(854, 480));
			var5.pack();
			var5.setLocationRelativeTo((Component)null);
			MinecraftImpl mcImpl = new MinecraftImpl(var5, var6, (MinecraftApplet)null, 854, 480, var3, var5);
			Thread mainThread = new Thread(mcImpl);
			mainThread.Name = "Minecraft main thread"; // Set Thread Name 
			mainThread.Priority = ThreadPriority.AboveNormal; // Equivalent to Java's priority level 10
			mcImpl.minecraftUri = "www.minecraft.net";
			if(var0 != null && var1 != null) {
				mcImpl.session = new Session(var0, var1);
			} else {
				mcImpl.session = new Session("Player" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 1000L, ""); // "System.currentTimeMillis()" in c# = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
			}

			if(var2 != null) {
				String[] var9 = var2.Split(":");
				mcImpl.setServer(var9[0], int.Parse(var9[1]));
			}

			var5.setVisible(true);
			var5.addWindowListener(new GameWindowListener(mcImpl, mainThread));
			mainThread.Start();
		}

		public NetClientHandler func_20001_q() {
			return this.thePlayer is EntityClientPlayerMP ? ((EntityClientPlayerMP)this.thePlayer).sendQueue : null;
		}

		public static void main(String[] var0) {
			String var1 = null;
			String var2 = null;
			var1 = "Player" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 1000L;
			if(var0.Length > 0) {
				var1 = var0[0];
			}

			var2 = "-";
			if(var0.Length > 1) {
				var2 = var0[1];
			}

			func_6269_a(var1, var2);
		}

		public static bool isGuiEnabled() {
			return theMinecraft == null || !theMinecraft.gameSettings.hideGUI;
		}

		public static bool isFancyGraphicsEnabled() {
			return theMinecraft != null && theMinecraft.gameSettings.fancyGraphics;
		}

		public static bool isAmbientOcclusionEnabled() {
			return theMinecraft != null && theMinecraft.gameSettings.ambientOcclusion;
		}

		public static bool isDebugInfoEnabled() {
			return theMinecraft != null && theMinecraft.gameSettings.showDebugInfo;
		}

		public bool lineIsCommand(string var1) {
			if(var1.StartsWith("/")) {
			}

			return false;
		}
	}

}

