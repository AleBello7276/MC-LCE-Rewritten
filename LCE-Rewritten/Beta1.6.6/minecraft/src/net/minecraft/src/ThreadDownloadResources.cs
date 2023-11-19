using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Threading;

using net.minecraft.client;

namespace net.minecraft.src
{
    public class ThreadDownloadResources 
    {
        private Thread thread;
        private DirectoryInfo resourcesFolder;
        private Minecraft mc;
        private bool closing = false;

        

		public ThreadDownloadResources(DirectoryInfo var1, Minecraft var2)
        {
            this.mc = var2;
			this.thread = new Thread(this.run);
            this.thread.Name = "Resource download thread";
            this.thread.IsBackground = true;
            this.resourcesFolder = new DirectoryInfo(Path.Combine(var1.FullName, "resources"));
            if (!this.resourcesFolder.Exists)
            {
				try
				{
					resourcesFolder.Create();
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException($"The working directory could not be created: " + this.resourcesFolder.FullName, ex);
				}

            }
        }

		public void start()
        {
            this.thread.Start();
        }

        public void join()
        {
            this.thread.Join();
        }

        public void run()
        {
			//Try to downlaod Game File from http://s3.amazonaws.com/MinecraftResources/, else load res frop, resourcesFolder -AleBel
            try
            {
                /*Uri uri = new Uri("http://s3.amazonaws.com/MinecraftResources/");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(uri.ToString());

                XmlNodeList contentsList = xmlDoc.GetElementsByTagName("Contents");

                for (int i = 0; i < 2; ++i)
                {
                    foreach (XmlNode contentNode in contentsList)
                    {
                        if (contentNode.NodeType == XmlNodeType.Element)
                        {
                            XmlElement contentElement = (XmlElement)contentNode;
                            string key = contentElement.GetElementsByTagName("Key")[0].ChildNodes[0].InnerText;
                            long size = long.Parse(contentElement.GetElementsByTagName("Size")[0].ChildNodes[0].InnerText);

                            if (size > 0)
                            {
                                this.DownloadAndInstallResource(uri, key, size, i);
                                if (this.closing)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }*/
				this.loadResource(this.resourcesFolder, "");
            }
            catch (Exception ex)
            {
                this.loadResource(this.resourcesFolder, "");
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void reloadResources()
        {
            this.loadResource(this.resourcesFolder, "");
        }

        private void loadResource(DirectoryInfo directory, string path)
        {
            foreach (var file in directory.GetFiles())
            {
                try
                {
                    this.mc.installResource(Path.Combine(path, file.Name), file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to add " + Path.Combine(path, file.Name) + ": " + ex.Message);
                }
            }

            foreach (var subdirectory in directory.GetDirectories())
            {
                this.loadResource(subdirectory, Path.Combine(path, subdirectory.Name) + "/");
            }
        }

        private void downloadAndInstallResource(Uri uri, string key, long size, int index)
        {
            try
            {
                int slashIndex = key.IndexOf("/");
                string prefix = key.Substring(0, slashIndex);

                if (!prefix.Equals("sound") && !prefix.Equals("newsound"))
                {
                    if (index != 1)
                    {
                        return;
                    }
                }
                else if (index != 0)
                {
                    return;
                }

                FileInfo file = new FileInfo(Path.Combine(this.resourcesFolder.FullName, key));

                if (!file.Exists || file.Length != size)
                {
                    file.Directory.Create();
                    string encodedKey = key.Replace(" ", "%20");
                    this.downloadResource(new Uri(uri, encodedKey), file, size);
                    if (this.closing)
                    {
                        return;
                    }
                }

                this.mc.installResource(key, file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void downloadResource(Uri uri, FileInfo file, long size)
		{
			byte[] buffer = new byte[4096];

			using (var inputStream = new WebClient().OpenRead(uri))
			using (var outputStream = new FileStream(file.FullName, FileMode.Create))
			{
				do
				{
					int bytesRead = inputStream.Read(buffer, 0, buffer.Length);

					if (bytesRead < 0)
					{
						inputStream.Close();
						outputStream.Close();
						return;
					}

					outputStream.Write(buffer, 0, bytesRead);
				} while (!closing);
			}
		}


        public void closeMinecraft()
        {
            this.closing = true;
        }
    }
}
