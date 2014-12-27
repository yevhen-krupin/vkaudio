using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VKAUDIO.Configuration
{
    public class ApplicationConfiguration
    {
        private string _defaultSavingDir = @"C:\Downloads";

        public string SavingDirectory
        {
            get { return _defaultSavingDir; }
            set { _defaultSavingDir = value; }
        }

        public static ApplicationConfiguration Instance { get; set; }

        private ApplicationConfiguration()
        {
            
        }

        static ApplicationConfiguration()
        {
            
            if (File.Exists("ApplicationConfiguration.conf"))
            {
                Load();
            }
            else
            {
                Instance = new ApplicationConfiguration();
            }
        }

        public static void Load()
        {
           
            try
            {
                var value = File.ReadAllText("ApplicationConfiguration.conf");
                JObject jObject = JObject.Parse(value);
                Instance = jObject.ToObject<ApplicationConfiguration>();
            }
            catch (Exception)
            {
                Instance = new ApplicationConfiguration();
            }
        }

        public void Save()
        {
            try
            {
                JObject jObject = JObject.FromObject(Instance);
                string value = jObject.ToString();
                if (File.Exists("ApplicationConfiguration.conf"))
                {
                    File.Delete("ApplicationConfiguration.conf");
                }
                File.WriteAllText("ApplicationConfiguration.conf", value);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}
