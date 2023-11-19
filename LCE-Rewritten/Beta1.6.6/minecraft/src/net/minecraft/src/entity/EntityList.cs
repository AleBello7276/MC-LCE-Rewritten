

using net.minecraft.src.world.chunk;


using net.minecraft.src.nbt;
using net.minecraft.src.world;

namespace net.minecraft.src.entity
{
	public class EntityList {
		private static Dictionary<string, Type> stringToClassMapping = new Dictionary<string, Type>();
		private static Dictionary<Type, string> classToStringMapping = new Dictionary<Type, string>();
		private static Dictionary<int, Type> IDtoClassMapping = new  Dictionary<int, Type>();
		private static Dictionary<Type, int> classToIDMapping = new Dictionary<Type, int>();

		private static void addMapping(Type var0, String var1, int var2) {
			stringToClassMapping[var1] = var0;
			classToStringMapping[var0] = var1;
			IDtoClassMapping[var2] = var0;
			classToIDMapping[var0] = var2;
		}

		public static Entity createEntityInWorld(String var0, World var1) {
			Entity var2 = null;

			try {
				Type  var3 = (Type)stringToClassMapping[var0];
				if(var3 != null) {
					var2 = (Entity)Activator.CreateInstance(var2, var1);
				}
			} catch (Exception var4) {
				Console.WriteLine(var4.StackTrace);
			}

			return var2;
		}

		public static Entity createEntityFromNBT(NBTTagCompound var0, World var1) {
			Entity var2 = null;

			try {
				Type var3 = (Type)stringToClassMapping[var0.getString("id")];
				if(var3 != null) {
					var2 = (Entity)Activator.CreateInstance(var3, var1);
				}
			} catch (Exception var4) {
				Console.WriteLine(var4.StackTrace);
			}

			if(var2 != null) {
				var2.readFromNBT(var0);
			} else {
				Console.WriteLine("Skipping Entity with id " + var0.getString("id"));
			}

			return var2;
		}

		public static Entity createEntity(int var0, World var1) {
			Entity var2 = null;

			try {
				Type  var3 = (Type)IDtoClassMapping[var0];
				if(var3 != null) {
					var2 = (Entity)Activator.CreateInstance(var3, var1);
				}
			} catch (Exception var4) {
				Console.WriteLine(var4.StackTrace);
			}

			if(var2 == null) {
				Console.WriteLine("Skipping Entity with id " + var0);
			}

			return var2;
		}

		public static int getEntityID(Entity var0) {
			return (int)classToIDMapping[var0.GetType()];
		}

		public static String getEntityString(Entity var0) {
			return (String)classToStringMapping[var0.GetType()];
		}

		static EntityList()
		{
			addMapping(typeof(EntityArrow), "Arrow", 10);
			addMapping(typeof(EntitySnowball), "Snowball", 11);
			addMapping(typeof(EntityItem), "Item", 1);
			addMapping(typeof(EntityPainting), "Painting", 9);
			addMapping(typeof(EntityLiving), "Mob", 48);
			addMapping(typeof(EntityMob), "Monster", 49);
			addMapping(typeof(EntityCreeper), "Creeper", 50);
			addMapping(typeof(EntitySkeleton), "Skeleton", 51);
			addMapping(typeof(EntitySpider), "Spider", 52);
			addMapping(typeof(EntityGiantZombie), "Giant", 53);
			addMapping(typeof(EntityZombie), "Zombie", 54);
			addMapping(typeof(EntitySlime), "Slime", 55);
			addMapping(typeof(EntityGhast), "Ghast", 56);
			addMapping(typeof(EntityPigZombie), "PigZombie", 57);
			addMapping(typeof(EntityPig), "Pig", 90);
			addMapping(typeof(EntitySheep), "Sheep", 91);
			addMapping(typeof(EntityCow), "Cow", 92);
			addMapping(typeof(EntityChicken), "Chicken", 93);
			addMapping(typeof(EntitySquid), "Squid", 94);
			addMapping(typeof(EntityWolf), "Wolf", 95);
			addMapping(typeof(EntityTNTPrimed), "PrimedTnt", 20);
			addMapping(typeof(EntityFallingSand), "FallingSand", 21);
			addMapping(typeof(EntityMinecart), "Minecart", 40);
			addMapping(typeof(EntityBoat), "Boat", 41);
		}
	}

}
